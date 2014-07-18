using System;
using System.Collections.Generic;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class MvcTagHelperDescriptor : TagHelperDescriptor
    {
        public MvcTagHelperDescriptor(string tagName, 
                                      string tagHelperName, 
                                      IEnumerable<TagHelperAttributeInfo> attributes)
            : base(tagName, tagHelperName)
        {
            foreach(var attribute in attributes)
            {
                Attributes.Add(attribute);
            }
        }
    }
}