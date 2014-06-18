using System;
using System.Diagnostics;
using System.Reflection;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public static class TagHelperConventions
    {
        public static bool IsTagHelper([NotNull] TypeInfo typeInfo)
        {
            if (!typeInfo.IsClass ||
                typeInfo.IsAbstract ||
                typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            return typeof(MvcTagHelper).IsAssignableFrom(typeInfo);
        }
    }
}