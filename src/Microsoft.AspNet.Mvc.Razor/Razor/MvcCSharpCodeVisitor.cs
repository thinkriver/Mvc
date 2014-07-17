﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Razor.Generator;
using Microsoft.AspNet.Razor.Generator.Compiler;
using Microsoft.AspNet.Razor.Generator.Compiler.CSharp;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class MvcCSharpCodeVisitor : MvcCodeVisitor
    {
        private const string TagHelperAttributeWriter = "__attributeWriter";
        private string _rendererName;
        private IChunkVisitor _defaultVisitor;
        private IChunkVisitor _razorVisitor;
        private MvcGeneratedClassContext _classContext;

        public MvcCSharpCodeVisitor(CSharpCodeWriter writer, CodeGeneratorContext context)
            : base(writer, context)
        {
            _razorVisitor = new CSharpCodeVisitor(writer, context);
            _classContext = context.Host.GeneratedClassContext as MvcGeneratedClassContext;
            _rendererName = _classContext.TagHelperRendererName;
        }

        // This visitor takes care of anything that is handled in the execute method body
        private IChunkVisitor BodyVisitor
        {
            get
            {
                if (_defaultVisitor == null)
                {
                    _defaultVisitor = Context.VisitorProvider.ProvideBodyVisitor(Writer);
                }

                return _defaultVisitor;
            }
        }

        protected override void Visit(TagHelperChunk chunk)
        {
            var tagHelperDescriptor = Context.Host.GeneratedTagHelperContext.GetTagHelper(chunk.TagName);

            Writer.Write(_rendererName)
                  .Write(".")
                  .Write(_classContext.TagHelperRendererPrepareMethodName)
                  .Write("<")
                  .Write(tagHelperDescriptor.TagHelperName)
                  .Write(">(")
                  .WriteStringLiteral(tagHelperDescriptor.TagName)
                  .WriteParameterSeparator()
                  .Write(_classContext.TagHelperViewDataAccessorName)
                  .WriteLine(");");

            var tagAttributes = chunk.Attributes;
            string currentWriter;

            // Build out attributes that the tag helper expects
            foreach (var attribute in tagHelperDescriptor.Attributes)
            {
                var mvcAttribute = attribute as MvcTagHelperAttributeInfo;

                Writer.Write(_rendererName)
                      .Write(".")
                      .Write(_classContext.TagHelperRendererAddAttributeBuilderName)
                      .Write("(")
                      .WriteStringLiteral(mvcAttribute.AttributeName)
                      .WriteParameterSeparator();

                using (Writer.BuildLambda(endLine: false, parameterNames: TagHelperAttributeWriter))
                {
                    currentWriter = Context.TargetWriterName;
                    Context.TargetWriterName = TagHelperAttributeWriter;

                    // TODO: Validate that the user provided the tag helper's attributes?
                    BodyVisitor.Accept(tagAttributes[mvcAttribute.AttributeName]);
                    // Remove attributes that are part of the tag helper
                    tagAttributes.Remove(mvcAttribute.AttributeName);

                    Context.TargetWriterName = currentWriter;

                    Writer.WriteStartReturn();

                    if (mvcAttribute.AttributeType == MvcTagHelperAttributeType.Text)
                    {
                        Writer.WriteStartNewObject(typeof(TagHelperLiteralExpression).FullName);
                    }
                    else if (mvcAttribute.AttributeType == MvcTagHelperAttributeType.Expression)
                    {
                        Writer.WriteStartNewObject(typeof(TagHelperModelExpression).FullName);
                    }

                    Writer.Write(TagHelperAttributeWriter)
                          .WriteLine(".ToString()")
                          .WriteEndMethodInvocation();
                }

                Writer.WriteEndMethodInvocation();
            }

            // Build out the attributes for the tag builder
            foreach (var attribute in tagAttributes)
            {
                Writer.Write(_rendererName)
                      .Write(".")
                      .Write(_classContext.TagHelperRendererAddAttributeName)
                      .Write("(")
                      .WriteStringLiteral(attribute.Key)
                      .WriteParameterSeparator();

                using (Writer.BuildLambda(endLine: false, parameterNames: TagHelperAttributeWriter))
                {
                    currentWriter = Context.TargetWriterName;
                    Context.TargetWriterName = TagHelperAttributeWriter;

                    BodyVisitor.Accept(attribute.Value);

                    Context.TargetWriterName = currentWriter;

                    Writer.WriteStartReturn()
                          .Write(TagHelperAttributeWriter)
                          .WriteLine(".ToString();");
                }

                Writer.WriteEndMethodInvocation();
            }

            Writer.Write(_rendererName)
                  .Write(".")
                  .WriteMethodInvocation(_classContext.TagHelperRendererStartMethodName);

            currentWriter = Context.TargetWriterName;
            Context.TargetWriterName = _rendererName + "." + _classContext.TagHelperRendererTagBuilderName;

            // Render all of the children
            BodyVisitor.Accept(chunk.Children);

            Context.TargetWriterName = currentWriter;

            Writer.Write("if (")
                  .Write(_rendererName)
                  .Write(".")
                  .WriteMethodInvocation(_classContext.TagHelperRendererEndMethodName, endLine: false)
                  .WriteLine(")");

            using (Writer.BuildScope())
            {
                if (!string.IsNullOrEmpty(Context.TargetWriterName))
                {
                    Writer.WriteStartMethodInvocation(Context.Host.GeneratedClassContext.WriteLiteralToMethodName)
                          .Write(Context.TargetWriterName)
                          .WriteParameterSeparator();
                }
                else
                {
                    Writer.WriteStartMethodInvocation(Context.Host.GeneratedClassContext.WriteLiteralMethodName);
                }

                Writer.Write(_rendererName)
                      .Write(".")
                      .WriteMethodInvocation(_classContext.TagHelperRendererOutputMethodName, endLine: false)
                      .WriteEndMethodInvocation();
            }
        }
    }
}