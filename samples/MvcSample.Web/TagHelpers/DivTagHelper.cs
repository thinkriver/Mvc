using System;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;

namespace MvcSample.Web
{
    public class DivTagHelper : MvcTagHelper
    {
	    public DivTagHelper()
            : base("div")
	    {
	    }

        public override void Process(TagBuilder builder, MvcTagHelperContext context)
        {
            builder.TagName = "em";
        }
    }
}