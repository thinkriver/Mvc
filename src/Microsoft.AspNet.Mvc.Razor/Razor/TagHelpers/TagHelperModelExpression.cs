﻿using System;
using System.Globalization;
using System.Net;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.Rendering.Expressions;
using Microsoft.AspNet.Razor.TagHelpers;

namespace Microsoft.AspNet.Mvc.Razor.TagHelpers
{
    public class TagHelperModelExpression : MvcTagHelperExpression
    {
        public TagHelperModelExpression(string expression)
        {
            Expression = expression;
        }

        public string Expression { get; private set; }

        public override string Build(MvcTagHelperContext context)
        {
            return ResolveValue(Expression, context);
        }

        // TODO: Pulled from HtmlHelper GenerateValue
        private string ResolveValue(string name, MvcTagHelperContext context)
        {
            string attemptedValue = null;
            ModelState modelState;

            if (context.ViewData.ModelState.TryGetValue(name, out modelState) && modelState.Value != null)
            {
                attemptedValue = (string)modelState.Value.ConvertTo(typeof(string), culture: null);
            }

            string resolvedValue;
            if (attemptedValue != null)
            {
                // case 1: if ModelState has a value then it's already formatted so ignore format string
                resolvedValue = attemptedValue;
            }
            else
            {
                if (name.Length == 0)
                {
                    var metadata = context.ViewData.ModelMetadata;
                    resolvedValue = ViewDataDictionary.FormatValue(metadata.Model, format: null);
                }
                else
                {
                    resolvedValue = Convert.ToString(context.ViewData.Eval(name, format: null), CultureInfo.CurrentCulture);
                }
            }

            var encoded = (!string.IsNullOrEmpty(resolvedValue)) ? WebUtility.HtmlEncode(resolvedValue) : string.Empty;

            return encoded;
        }
    }
}