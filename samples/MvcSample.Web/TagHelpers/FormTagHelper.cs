using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;

namespace MvcSample.Web
{
    public class FormTagHelper : MvcTagHelper
    {
        private IUrlHelper _urlHelper;

        public FormTagHelper(IUrlHelper urlHelper)
            : base("formsdfd")
        {
            _urlHelper = urlHelper;
        }

        public override MvcTagHelperAttributeInfo[] Attributes
        {
            get
            {
                return new[]
                {
                    new MvcTagHelperAttributeInfo("action")
                    {
                        AttributeType = MvcTagHelperAttributeType.Text
                    },
                    new MvcTagHelperAttributeInfo("controller")
                    {
                        AttributeType = MvcTagHelperAttributeType.Text
                    },
                };
            }
        }

        public override void Process(TagBuilder builder, MvcTagHelperContext context)
        {
            builder.Attributes.Add("action", _urlHelper.Action(
                context.AttributeExpressionBuilders["action"].Build(context),
                context.AttributeExpressionBuilders["controller"].Build(context)));
            builder.Attributes.Add("method", "post");
            builder.InnerHtml = "<input type=\"hidden\" value=\"something\" />";
        }
    }
}