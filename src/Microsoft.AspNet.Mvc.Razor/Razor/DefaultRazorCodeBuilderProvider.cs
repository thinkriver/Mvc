using System;
using Microsoft.AspNet.Razor.Generator;
using Microsoft.AspNet.Razor.Generator.Compiler;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class DefaultRazorCodeBuilderProvider : IRazorCodeBuilderProvider
    {
        public CodeBuilder GetCodeBuilder(CodeBuilder incomingCodeBuilder, CodeGeneratorContext context)
        {
            return new MvcCSharpCodeBuilder(context);
        }
    }
}