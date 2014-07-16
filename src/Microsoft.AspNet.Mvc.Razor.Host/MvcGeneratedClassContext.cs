using System;
using Microsoft.AspNet.Razor.Generator;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class MvcGeneratedClassContext : GeneratedClassContext
    {
	    public MvcGeneratedClassContext()
            : base(
                executeMethodName: "ExecuteAsync",
                writeMethodName: "Write",
                writeLiteralMethodName: "WriteLiteral",
                writeToMethodName: "WriteTo",
                writeLiteralToMethodName: "WriteLiteralTo",
                templateTypeName: "HelperResult",
                defineSectionMethodName: "DefineSection")
	    {
            ResolveUrlMethodName = "Href";
            TagHelperRendererName = "__tagHelperRenderer";
            TagHelperViewDataAccessorName = "ViewContext.ViewData";
            TagHelperRendererAddAttributeBuilderName = "AddAttributeBuilder";
            TagHelperRendererAddAttributeName = "AddAttribute";
            TagHelperRendererTagBuilderName = "TagBodyWriter";
            TagHelperRendererPrepareMethodName = "PrepareTagHelper";
            TagHelperRendererStartMethodName = "StartTagHelper";
            TagHelperRendererEndMethodName = "EndTagHelper";
            TagHelperRendererOutputMethodName = "OutputTagHelper";
        }

        public string TagHelperRendererName { get; set; }
        public string TagHelperViewDataAccessorName { get; set; }
        public string TagHelperRendererAddAttributeBuilderName { get; set; }
        public string TagHelperRendererAddAttributeName { get; set; }
        public string TagHelperRendererTagBuilderName { get; set; }
        public string TagHelperRendererPrepareMethodName { get; set; }
        public string TagHelperRendererStartMethodName { get; set; }
        public string TagHelperRendererEndMethodName { get; set; }
        public string TagHelperRendererOutputMethodName { get; set; }
    }
}