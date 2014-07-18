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
            : base("form")
        {
            _urlHelper = urlHelper;
        }

        public TagHelperLiteralExpression Action { get; set; }
        public TagHelperLiteralExpression Controller { get; set; }

        public override void Process(TagBuilder builder, MvcTagHelperContext context)
        {
            builder.Attributes.Add("action", _urlHelper.Action(Action.Build(context), Controller.Build(context)));
            builder.Attributes.Add("method", "post");
            builder.InnerHtml = "<input type=\"hidden\" value=\"something\" />";
        }
    }
}