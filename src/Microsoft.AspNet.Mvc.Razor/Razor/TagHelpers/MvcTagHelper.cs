using System;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    /// <summary>
    /// Summary description for MvcTagHelper
    /// </summary>
    public abstract class MvcTagHelper : TagHelper
    {
        public MvcTagHelper(params string[] targets)
            : base(targets)
        { }

        public abstract void Process(TagBuilder builder, MvcTagHelperContext context);
    }
}