using System;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class DefaultRazorBaseTypeResolver : IRazorBaseTypeResolver
    {
        public string Resolve()
        {
            return typeof(RazorView).FullName;
        }
    }
}