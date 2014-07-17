using System;
using System.Linq;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;

namespace MvcSample.Web
{
    public class InputTagHelper : MvcTagHelper
    {
        public InputTagHelper()
            : base("inputsdfsdf")
        {
        }

        public override MvcTagHelperAttributeInfo[] Attributes
        {
            get
            {
                return new[]
                {
                    new MvcTagHelperAttributeInfo("for")
                    {
                        AttributeType = MvcTagHelperAttributeType.Expression
                    }
                };
            }
        }

        public override void Process(TagBuilder builder, MvcTagHelperContext context)
        {
            var modelExpressionBuilder = context.AttributeExpressionBuilders["for"] as TagHelperModelExpression;
            builder.Attributes.Add("type", "text");
            builder.Attributes.Add("placeholder", "Enter in text...");
            builder.Attributes.Add("value", modelExpressionBuilder.Build(context));
            builder.Attributes.Add("id", modelExpressionBuilder.Expression);
            builder.Attributes.Add("name", modelExpressionBuilder.Expression);
        }
    }
}