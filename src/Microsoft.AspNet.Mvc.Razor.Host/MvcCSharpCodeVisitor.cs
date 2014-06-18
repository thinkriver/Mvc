using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Razor.Generator;
using Microsoft.AspNet.Razor.Generator.Compiler;
using Microsoft.AspNet.Razor.Generator.Compiler.CSharp;

namespace Microsoft.AspNet.Mvc.Razor
{
    /// <summary>
    /// Summary description for MvcCSharpCodeVisitor
    /// </summary>
    public class MvcCSharpCodeVisitor : MvcCodeVisitor
    {
        private const string RendererName = "__renderer";
        
        private IChunkVisitor _defaultVisitor;
        private IChunkVisitor _razorVisitor;

        public MvcCSharpCodeVisitor(CSharpCodeWriter writer, CodeGeneratorContext context)
            : base(writer, context)
        {
            _razorVisitor = new CSharpCodeVisitor(writer, context);
        }

        // This visitor takes care of anything that is handled in the execute method body
        private IChunkVisitor BodyVisitor
        {
            get
            {
                if(_defaultVisitor == null)
                {
                    _defaultVisitor = Context.VisitorProvider.ProvideBodyVisitor(Writer);
                }

                return _defaultVisitor;
            }
        }

        protected override void Visit(TagHelperChunk chunk)
        {
            var tagHelperDescriptor = Context.Host.GeneratedTagHelperContext.GetTagHelper(chunk.TagName);

            Writer.Write(RendererName)
                  .Write(".")
                  .Write("PrepareHelper") // TODO: Extensibility point
                  .Write("<")
                  .Write(tagHelperDescriptor.TagHelperName)
                  .Write(">(")
                  .WriteStringLiteral(tagHelperDescriptor.TagName)
                  .WriteParameterSeparator()
                  .Write("ViewContext.ViewData") // TODO: Extensibility point
                  .WriteLine(");");

            var tagAttributes = chunk.Attributes;

            // Build out attributes that the tag helper expects
            foreach(var attribute in tagHelperDescriptor.Attributes)
            {
                var mvcAttribute = attribute as MvcTagHelperAttributeInfo;

                Writer.Write(RendererName)
                      .Write(".")
                      .Write("AddAttributeBuilder") // TODO: Extensibility point
                      .Write("(")
                      .WriteStringLiteral(mvcAttribute.AttributeName)
                      .WriteParameterSeparator();

                if (mvcAttribute.AttributeType == MvcTagHelperAttributeType.Text)
                {
                    Writer.WriteStartNewObject(typeof(TagHelperLiteralExpression).FullName);
                }
                else if(mvcAttribute.AttributeType == MvcTagHelperAttributeType.Expression)
                {
                    Writer.WriteStartNewObject(typeof(TagHelperModelExpression).FullName);
                }

                // TODO: Validate that the user provided the tag helper's attributes?
                var currentRenderingMode = Context.RenderingMode;
                Context.RenderingMode = RenderingMode.InjectCode;

                BodyVisitor.Accept(tagAttributes[mvcAttribute.AttributeName]);
                // Remove attributes that are part of the tag helper
                tagAttributes.Remove(mvcAttribute.AttributeName);

                Context.RenderingMode = currentRenderingMode;

                Writer.WriteEndMethodInvocation(endLine: false)
                      .WriteEndMethodInvocation();
            }

            // Build out the attributes for the tag builder
            foreach(var attribute in tagAttributes)
            {
                Writer.Write(RendererName)
                      .Write(".")
                      .Write("AddAttribute") // TODO: Extensibility point
                      .Write("(")
                      .WriteStringLiteral(attribute.Key)
                      .WriteParameterSeparator();

                var currentRenderingMode = Context.RenderingMode;
                Context.RenderingMode = RenderingMode.InjectCode;

                BodyVisitor.Accept(attribute.Value);

                Context.RenderingMode = currentRenderingMode;

                Writer.WriteEndMethodInvocation();
            }

            Writer.Write(RendererName)
                  .Write(".")
                  .WriteMethodInvocation("StartHelper");

            var currentWriter = Context.TargetWriterName;
            Context.TargetWriterName = RendererName + "." + "TagBodyWriter"; // TODO: TagBodyWriter is extensibility point

            // Render all of the children
            BodyVisitor.Accept(chunk.Children);

            Context.TargetWriterName = currentWriter;

            Writer.Write("if (")
                  .Write(RendererName)
                  .Write(".")
                  .WriteMethodInvocation("EndHelper", endLine: false)
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

                Writer.Write(RendererName)
                      .Write(".")
                      .WriteMethodInvocation("OutputHelper", endLine: false) // TODO: Extensibility point
                      .WriteEndMethodInvocation();
            }
        }
    }
}