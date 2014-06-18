using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using System.Linq;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class TagHelperRenderingContext
    {
        private Stack<TagBuilder> _tagBuilders;
        private Stack<TagBuilderStringWriter> _tagBodyWriters;
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
            _tagBodyWriters = new Stack<TagBuilderStringWriter>();
        }

        public IModelMetadataProvider MetadataProvider { get; private set; }
        public TagBuilderStringWriter TagBodyWriter { get; private set; }

        public void PrepareHelper<T>(string tagName, ViewDataDictionary viewData) where T : MvcTagHelper
        {
            _currentTagBuilder = new TagBuilder(tagName);
            _tagBuilders.Push(_currentTagBuilder);

            _currentTagHelperContext = new MvcTagHelperContext(viewData, MetadataProvider);

            TagBodyWriter = new TagBuilderStringWriter(_currentTagBuilder);
            _tagBodyWriters.Push(TagBodyWriter);

            _tagHelper = _typeActivator.CreateInstance<T>(_serviceProvider);
        }

        public void StartHelper()
        {
            _tagHelper.Process(_currentTagBuilder, _currentTagHelperContext);
        }

        public bool EndHelper()
        {
            _tagBuilders.Pop();
            _tagBodyWriters.Pop();

            if (_tagBodyWriters.Any())
            {
                _tagBodyWriters.Peek().Write(_currentTagBuilder.ToString());
                TagBodyWriter = _tagBodyWriters.Peek();
                _currentTagBuilder = _tagBuilders.Peek();

                return false;
            }

            return true;
        }

        public string OutputHelper()
        {
            return _currentTagBuilder.ToString();
        }

        public void AddAttributeBuilder(string name, MvcTagHelperExpression expression)
        {
            _currentTagHelperContext.AttributeExpressionBuilders.Add(name, expression);
        }

        public void AddAttribute(string name, string value)
        {
            _currentTagBuilder.Attributes.Add(name, value);
        }
    }
}