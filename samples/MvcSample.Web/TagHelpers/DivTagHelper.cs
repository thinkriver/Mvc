using System;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;

namespace MvcSample.Web
{
    public class DivTagHelper : MvcTagHelper
    {
	    public DivTagHelper()
            : base("divsdfsdf")
	    {
	    }

        public override MvcTagHelperAttributeInfo[] Attributes
        {
            get
            {
                return new MvcTagHelperAttributeInfo[0];
            }
        }

        public override void Process(TagBuilder builder, MvcTagHelperContext context)
        {
            builder.TagName = "em";
        }
    }
}