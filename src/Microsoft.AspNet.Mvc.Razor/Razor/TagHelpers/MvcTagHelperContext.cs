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
        public MvcTagHelperContext(ViewContext viewContext, IModelMetadataProvider metadataProvider)
            : base()
        {
            ViewContext = viewContext;
            MetadataProvider = metadataProvider;
        }

        public object Model
        {
            get
            {
                return ViewData.Model;
            }
        }
        public ViewContext ViewContext { get; set; }
        public ViewDataDictionary ViewData
        {
            get
            {
                return ViewContext.ViewData;
            }
        }
        public IModelMetadataProvider MetadataProvider { get; private set; }
    }
}