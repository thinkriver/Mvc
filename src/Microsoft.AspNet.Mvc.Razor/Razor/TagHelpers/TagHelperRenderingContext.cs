using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using System.IO;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class TagHelperRenderingContext
    {
        private Stack<TagBuilder> _tagBuilders;
        private TagBuilder _currentTagBuilder;
        private MvcTagHelperContext _currentTagHelperContext;
        private MvcTagHelper _tagHelper;
        private IServiceProvider _serviceProvider;
        private ITypeActivator _typeActivator;

        public TagHelperRenderingContext(IModelMetadataProvider metadataProvider, 
                                         IServiceProvider serviceProvider,
                                         ITypeActivator typeActivator)
	    {
            MetadataProvider = metadataProvider;
            // TODO: Pull these out into a tag helper activator
            _serviceProvider = serviceProvider;
            _typeActivator = typeActivator;
            _tagBuilders = new Stack<TagBuilder>();
        }

        public IModelMetadataProvider MetadataProvider { get; private set; }

        public void PrepareTagHelper<T>(string tagName, ViewContext viewContext) where T : MvcTagHelper
        {
            _currentTagBuilder = new TagBuilder(tagName);
            _tagBuilders.Push(_currentTagBuilder);

            _currentTagHelperContext = new MvcTagHelperContext(viewContext, MetadataProvider);

            _tagHelper = _typeActivator.CreateInstance<T>(_serviceProvider);
        }

        public string StartTagHelper()
        {
            _tagHelper.Process(_currentTagBuilder, _currentTagHelperContext);

            // We render the start tag and the body
            return _currentTagBuilder.ToString(TagRenderMode.StartTag) + _currentTagBuilder.ToString(TagRenderMode.Body);
        }

        public string EndTagHelper()
        {
            _tagBuilders.Pop();

            var endTag = _currentTagBuilder.ToString(TagRenderMode.EndTag);

            if (_tagBuilders.Count > 0)
            {
                _currentTagBuilder = _tagBuilders.Peek();
            }

            return endTag;
        }

        public void AddAttributeBuilder(string name, Func<TextWriter, MvcTagHelperExpression> expressionBuilder)
        {
            _currentTagHelperContext.AttributeExpressionBuilders.Add(name, expressionBuilder(new StringWriter()));
        }

        public void AddAttribute(string name, Func<TextWriter, string> valueBuilder)
        {
            _currentTagBuilder.Attributes.Add(name, valueBuilder(new StringWriter()));
        }
    }
}