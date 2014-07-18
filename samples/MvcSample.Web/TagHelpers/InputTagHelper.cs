using System;
using System.Linq;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;

namespace MvcSample.Web
{
    public class InputTagHelper : MvcTagHelper
    {
        public InputTagHelper()
            : base("input")
        {
        }

        public TagHelperModelExpression For { get; set; }

        public override void Process(TagBuilder builder, MvcTagHelperContext context)
        {
            builder.Attributes.Add("type", "text");
            builder.Attributes.Add("placeholder", "Enter in text...");
            builder.Attributes.Add("value", For.Build(context));
            builder.Attributes.Add("id", For.Expression);
            builder.Attributes.Add("name", For.Expression);
        }
    }
}