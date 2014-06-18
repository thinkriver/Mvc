using System;
using Microsoft.AspNet.Razor.Parser;

namespace Microsoft.AspNet.Mvc.Razor
{
    public class DefaultRazorCodeParserProvider : IRazorCodeParserProvider
    {
        public ParserBase GetCodeParser(ParserBase incomingCodeParser, string baseType)
        {
            return new MvcRazorCodeParser(baseType);
        }
    }
}