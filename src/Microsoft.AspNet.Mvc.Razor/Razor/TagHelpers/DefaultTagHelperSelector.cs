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
        private IDictionary<string, List<MvcTagHelperDescriptor>> _tagHelperCache;

        public DefaultTagHelperSelector(IControllerAssemblyProvider assemblyProvider,
                                        ITypeActivator typeActivator,
                                        IServiceProvider serviceProvider)
        {
            _typeActivator = typeActivator;
            _serviceProvider = serviceProvider;
            _assemblyProvider = assemblyProvider;
        }

        public IEnumerable<MvcTagHelperDescriptor> SelectTagHelper([NotNull] string tagName)
        {
            if(_tagHelperCache == null)
            {
                BuildTagHelperCache();
            }

            // TODO: Validate that the tag exists inthe cache

            List<MvcTagHelperDescriptor> descriptors = null;

            _tagHelperCache.TryGetValue(tagName, out descriptors);

            return descriptors;
        }

        private void BuildTagHelperCache()
        {
            _tagHelperCache = new Dictionary<string, List<MvcTagHelperDescriptor>>();

            var assemblies = _assemblyProvider.CandidateAssemblies;
            var types = assemblies.SelectMany(a => a.DefinedTypes);
            var descriptors = types.Where(TagHelperConventions.IsTagHelper)
                                   .SelectMany(GetTagHelperDescriptors);

            foreach(var descriptor in descriptors)
            {
                if(!_tagHelperCache.ContainsKey(descriptor.TagName))
                {
                    _tagHelperCache[descriptor.TagName] = new List<MvcTagHelperDescriptor>();
                }

                _tagHelperCache[descriptor.TagName].Add(descriptor);
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