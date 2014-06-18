using System;
using System.Linq;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;

namespace MvcSample.Web
{
    public class FooTagHelper : MvcTagHelper
    {
        public FooTagHelper()
            : base("foo")
        {
        }

        public override MvcTagHelperAttributeInfo[] Attributes
        {
            get
            {
                return new[]
                {
                    new MvcTagHelperAttributeInfo("bar")
                    {
                        AttributeType = MvcTagHelperAttributeType.Expression
                    }
                };
            }
        }

        public override void Process(TagBuilder builder, MvcTagHelperContext context)
        {
            builder.TagName = "h1";
            builder.InnerHtml = context.AttributeExpressionBuilders["bar"].Build(context);
        }
    }
}