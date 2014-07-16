using System;
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

        public TagHelperDescriptor GetTagHelper(string tagName)
        {
            return _selector.SelectTagHelper(tagName);
        }

        public bool IsTagHelper(string tagName)
        {
            return _selector.SelectTagHelper(tagName) != null;
        }

        public void Register(TagHelperDescriptor descriptor)
        {
            // TODO: Register something?
        }
    }
}