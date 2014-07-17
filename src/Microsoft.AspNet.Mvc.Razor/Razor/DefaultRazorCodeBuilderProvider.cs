using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Razor.Generator;
using Microsoft.AspNet.Razor.Generator.Compiler;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class DefaultRazorCodeBuilderProvider : IRazorCodeBuilderProvider
    {
        public CodeBuilder GetCodeBuilder(CodeBuilder incomingCodeBuilder, 
                                          MvcRazorHostOptions options, 
                                          CodeGeneratorContext context)
        {
            AddDefaultInjectedProperties(context, options);

            return new MvcCSharpCodeBuilder(context, options);
        }

        // TODO: Refactor this out into something else
        private void AddDefaultInjectedProperties(CodeGeneratorContext context,
                                                  MvcRazorHostOptions options)
        {
            var currentChunks = context.CodeTreeBuilder.CodeTree.Chunks;
            var existingInjects = new HashSet<string>(currentChunks.OfType<InjectChunk>()
                                                                   .Select(c => c.MemberName),
                                                      StringComparer.Ordinal);

            var modelChunk = currentChunks.OfType<ModelChunk>()
                                          .LastOrDefault();
            var model = options.DefaultModel;
            if (modelChunk != null)
            {
                model = modelChunk.ModelType;
            }
            model = '<' + model + '>';

            // Locate properties by name that haven't already been injected in to the View.
            var propertiesToAdd = options.DefaultInjectedProperties
                                              .Where(c => !existingInjects.Contains(c.MemberName));
            foreach (var property in propertiesToAdd)
            {
                var typeName = property.TypeName.Replace("<TModel>", model);
                currentChunks.Add(new InjectChunk(typeName, property.MemberName));
            }
        }
    }
}