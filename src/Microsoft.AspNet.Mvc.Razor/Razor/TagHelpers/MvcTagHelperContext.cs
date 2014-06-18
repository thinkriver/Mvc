using System;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    /// <summary>
    /// Summary description for MvcTagHelperContext
    /// </summary>
    public class MvcTagHelperContext : TagHelperContext
    {
        public MvcTagHelperContext(ViewDataDictionary viewData, IModelMetadataProvider metadataProvider)
            : base()
        {
            ViewData = viewData;
            MetadataProvider = metadataProvider;
        }

        public object Model
        {
            get
            {
                return ViewData.Model;
            }
        }
        public ViewDataDictionary ViewData { get; private set; }
        public IModelMetadataProvider MetadataProvider { get; private set; }
    }
}