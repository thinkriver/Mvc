using System;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public abstract class MvcTagHelperExpression : TagHelperExpression
    {
        public override string Build(TagHelperContext context)
        {
            return Build(context as MvcTagHelperContext);
        }

        public abstract string Build(MvcTagHelperContext context);
    }
}