using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class MvcTagHelperProviderContext : ITagHelperProviderContext
    {
        private ITagHelperSelector _selector;

	    public MvcTagHelperProviderContext(ITagHelperSelector selector)
	    {
            _selector = selector;
	    }

        public IEnumerable<TagHelperDescriptor> GetTagHelpers(string tagName)
        {
            return _selector.SelectTagHelper(tagName) ?? Enumerable.Empty<TagHelperDescriptor>();
        }
    }
}