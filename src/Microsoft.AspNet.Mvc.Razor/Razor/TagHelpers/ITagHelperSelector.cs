using System;
using System.Collections.Generic;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public interface ITagHelperSelector
    {
        IEnumerable<MvcTagHelperDescriptor> SelectTagHelper([NotNull] string tagName);
    }
}