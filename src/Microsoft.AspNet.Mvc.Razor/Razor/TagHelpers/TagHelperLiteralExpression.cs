using System;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class TagHelperLiteralExpression : MvcTagHelperExpression
    {
        public static readonly string FullName = typeof(TagHelperLiteralExpression).FullName;

        public TagHelperLiteralExpression(string literal)
        {
            Literal = literal;
        }

        public string Literal { get; private set; }

        public override string Build(MvcTagHelperContext context)
        {
            return Literal;
        }
    }
}