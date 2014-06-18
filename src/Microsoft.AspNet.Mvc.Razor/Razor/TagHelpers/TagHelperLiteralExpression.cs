using System;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    /// <summary>
    /// Summary description for TagHelperLiteralExpression
    /// </summary>
    public class TagHelperLiteralExpression : MvcTagHelperExpression
    {
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