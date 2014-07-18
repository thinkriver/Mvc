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
        private IServiceProvider _serviceProvider;
        private ITypeActivator _typeActivator;
        private IDictionary<string, MvcTagHelperExpression> _attributeBuilders;

        public TagHelperRenderingContext(IModelMetadataProvider metadataProvider, 
                                         IServiceProvider serviceProvider,
                                         ITypeActivator typeActivator)
	    {
            MetadataProvider = metadataProvider;
            // TODO: Pull these out into a tag helper activator
            _serviceProvider = serviceProvider;
            _typeActivator = typeActivator;
            _tagBuilders = new Stack<TagBuilder>();
            _attributeBuilders = new Dictionary<string, MvcTagHelperExpression>(StringComparer.OrdinalIgnoreCase);
        }

        public IModelMetadataProvider MetadataProvider { get; private set; }

        public void PrepareTagHelper(string tagName, ViewContext viewContext)
        {
            _currentTagBuilder = new TagBuilder(tagName);
            _tagBuilders.Push(_currentTagBuilder);

            _currentTagHelperContext = new MvcTagHelperContext(viewContext, MetadataProvider);
        }

        public string StartTagHelper(Type[] tagHelperTypes)
        {
            foreach (var tagHelperType in tagHelperTypes)
            {
                var tagHelper = BuildTagHelper(tagHelperType);

                tagHelper.Process(_currentTagBuilder, _currentTagHelperContext);
            }

            // Remove all tag attribute builders, no need to track them anymore
            _attributeBuilders.Clear();

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
            _attributeBuilders.Add(name, expressionBuilder(new StringWriter()));
        }

        public void AddAttribute(string name, Func<TextWriter, string> valueBuilder)
        {
            _currentTagBuilder.Attributes.Add(name, valueBuilder(new StringWriter()));
        }

        private MvcTagHelper BuildTagHelper(Type tagHelperType)
        {
            var tagHelper = (MvcTagHelper)_typeActivator.CreateInstance(_serviceProvider, tagHelperType);

            foreach(var attributeBuilder in _attributeBuilders)
            {
                // TODO: Cache these
                var setter = PropertyHelper.MakeFastPropertySetter(tagHelper.GetType().GetProperty(attributeBuilder.Key));
                setter(tagHelper, attributeBuilder.Value);
            }

            return tagHelper;
        }
    }
}