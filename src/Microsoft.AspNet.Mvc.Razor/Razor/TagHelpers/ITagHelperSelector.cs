using System;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public interface ITagHelperSelector
    {
        MvcTagHelperDescriptor SelectTagHelper([NotNull] string tagName);
    }
}