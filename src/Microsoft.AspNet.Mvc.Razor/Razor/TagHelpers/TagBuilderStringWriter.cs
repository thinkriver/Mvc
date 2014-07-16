﻿using System;
using System.IO;
using Microsoft.AspNet.Mvc.Rendering;

namespace Microsoft.AspNet.Mvc.Razor
{
    /// <summary>
    /// Summary description for TagBuilderStringWriter
    /// </summary>
    public class TagBuilderStringWriter : StringWriter
    {
        private TagBuilder _builder;
	    public TagBuilderStringWriter(TagBuilder tagBuilder)
	    {
            _builder = tagBuilder;
	    }

        public override void Write(string value)
        {
            _builder.InnerHtml += value;
        }

        // TODO: Override more?
    }
}