using System;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class MvcTagHelperAttributeInfo : TagHelperAttributeInfo
    {
        public MvcTagHelperAttributeInfo(string attribute)
            : base (attribute)
        {
        }

        public Type AttributeExpressionType { get; set; }
    }
}