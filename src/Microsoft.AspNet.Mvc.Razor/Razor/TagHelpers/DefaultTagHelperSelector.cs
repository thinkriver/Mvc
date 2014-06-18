using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNet.Razor.TagHelpers;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class DefaultTagHelperSelector : ITagHelperSelector
    {
        private ITypeActivator _typeActivator;
        private IServiceProvider _serviceProvider;
        private IControllerAssemblyProvider _assemblyProvider;
        private IDictionary<string, MvcTagHelperDescriptor> _tagHelperCache;

        public DefaultTagHelperSelector(IControllerAssemblyProvider assemblyProvider,
                                        ITypeActivator typeActivator,
                                        IServiceProvider serviceProvider)
        {
            _typeActivator = typeActivator;
            _serviceProvider = serviceProvider;
            _assemblyProvider = assemblyProvider;
        }

        public MvcTagHelperDescriptor SelectTagHelper([NotNull]string tagName)
        {
            if(_tagHelperCache == null)
            {
                BuildTagHelperCache();
            }

            // TODO: Validate that the tag exists inthe cache

            MvcTagHelperDescriptor descriptor = null;

            _tagHelperCache.TryGetValue(tagName, out descriptor);

            return descriptor;
        }

        private void BuildTagHelperCache()
        {
            _tagHelperCache = new Dictionary<string, MvcTagHelperDescriptor>();

            var assemblies = _assemblyProvider.CandidateAssemblies;
            var types = assemblies.SelectMany(a => a.DefinedTypes);
            var descriptors = types.Where(TagHelperConventions.IsTagHelper)
                                   .SelectMany(GetTagHelperDescriptors);

            foreach(var descriptor in descriptors)
            {
                _tagHelperCache.Add(descriptor.TagName, descriptor);
            }
        }

        private IEnumerable<MvcTagHelperDescriptor> GetTagHelperDescriptors(Type type)
        {
            var tagHelper = _typeActivator.CreateInstance(_serviceProvider, type) as MvcTagHelper;
            var tagHelperName = type.FullName;

            return tagHelper.Targets.Select(tagName => 
                new MvcTagHelperDescriptor(tagName, tagHelperName, tagHelper.Attributes));
        }
    }
}