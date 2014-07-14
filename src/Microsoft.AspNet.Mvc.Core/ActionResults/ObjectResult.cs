﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.HeaderValueAbstractions;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.OptionsModel;

namespace Microsoft.AspNet.Mvc
{
    public class ObjectResult : ActionResult
    {
        public object Value { get; set; }

        public List<OutputFormatter> Formatters { get; set; }

        public List<MediaTypeHeaderValue> ContentTypes { get; set; }

        public Type DeclaredType { get; set; }

        public ObjectResult(object value)
        {
            Value = value;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var formatters = GetDefaultFormatters(context);
            var formatterContext = new OutputFormatterContext()
            {
                DeclaredType = DeclaredType,
                HttpContext = context.HttpContext,
                ObjectResult = this
            };

            var incomingAcceptHeader = HeaderParsingHelpers.GetAcceptHeaders(context.HttpContext.Request.Accept);
            var sortedAcceptHeaders = incomingAcceptHeader != null ?
                                            SortMediaTypeWithQualityHeaderValues(incomingAcceptHeader.ToArray())
                                            .Where(header => header.Quality != FormattingUtilities.NoMatch)
                                            .ToArray() :
                                            new MediaTypeWithQualityHeaderValue[] { };

            OutputFormatter selectedFormatter = null;

            // Enable the scenario where there is no content type set. 
            if (ContentTypes == null || ContentTypes.Count == 0)
            {
                MediaTypeHeaderValue[] contentTypes = sortedAcceptHeaders;
                if (sortedAcceptHeaders.Length == 0)
                {
                    var incomingContentType = MediaTypeHeaderValue.Parse(context.HttpContext.Request.ContentType);
                    contentTypes = new MediaTypeHeaderValue[] { incomingContentType };
                }

                selectedFormatter = SelectFormatterUsingSortedAcceptHeaders(
                                                                        formatterContext,
                                                                        formatters,
                                                                        contentTypes);
            }
            else if (ContentTypes.Count == 1)
            {
                // There is only one value that can be supported.
                selectedFormatter = SelectFormatterUsingAnyAcceptableContentType(
                                                                            formatterContext,
                                                                            formatters,
                                                                            ContentTypes);
            }
            else
            {
                // Do an intersection. 
                var filteredAndSortedAcceptHeaders = sortedAcceptHeaders
                                                        .Where(acceptHeader =>
                                                                ContentTypes
                                                                    .Any(contentType =>
                                                                           contentType.IsSubsetOf(acceptHeader)))
                                                        .ToArray();

                if (filteredAndSortedAcceptHeaders.Length > 0)
                {
                    selectedFormatter = SelectFormatterUsingSortedAcceptHeaders(
                                                                        formatterContext,
                                                                        formatters,
                                                                        filteredAndSortedAcceptHeaders);
                }

                if (selectedFormatter == null)
                {
                    // Either there were no acceptHeaders that were present OR 
                    // There were no accept headers which matched OR
                    // There were acceptHeaders which matched but there was no formatter 
                    // which supported any of them.
                    // In any of these cases, if the user has specified content types,
                    // do a last effort to find a formatter which can write any of the user specified content type.
                    selectedFormatter = SelectFormatterUsingAnyAcceptableContentType(
                                                                        formatterContext,
                                                                        formatters,
                                                                        ContentTypes);
                }
            }

            if (selectedFormatter == null)
            {
                // No formatter supports this. throw 406.
                // TODO: This is a stop gap, we need to decide on how to return different status codes here.
                throw new InvalidOperationException();
            }

            // TODO: When deciding the response charset we need to look at accept charset headers as well.
            context.HttpContext.Response.ContentType = formatterContext.SelectedContentType.RawValue;
            await selectedFormatter.WriteAsync(formatterContext, CancellationToken.None);
        }

        private OutputFormatter SelectFormatterUsingSortedAcceptHeaders(
                                                            OutputFormatterContext formatterContext,
                                                            IEnumerable<OutputFormatter> formatters,
                                                            IEnumerable<MediaTypeHeaderValue> sortedAcceptHeaders)
        {
            OutputFormatter selectedFormatter = null;
            foreach (var contentType in sortedAcceptHeaders)
            {
                // Loop through each of the formatters and see if any one will support this 
                // mediaType Value. 
                selectedFormatter = formatters.FirstOrDefault(
                                                    formatter =>
                                                        formatter.CanWriteResult(formatterContext, contentType));
                if (selectedFormatter != null)
                {
                    // we found our match. 
                    break;
                }
            }

            return selectedFormatter;
        }

        private OutputFormatter SelectFormatterUsingAnyAcceptableContentType(
                                                            OutputFormatterContext formatterContext,
                                                            IEnumerable<OutputFormatter> formatters,
                                                            IEnumerable<MediaTypeHeaderValue> acceptableContentTypes)
        {
            var selectedFormatter = formatters.FirstOrDefault(
                                            formatter => 
                                                    acceptableContentTypes
                                                    .Any(contentType =>
                                                            formatter.CanWriteResult(formatterContext, contentType)));
            return selectedFormatter;
        }

        private static MediaTypeWithQualityHeaderValue[] SortMediaTypeWithQualityHeaderValues
                                                    (MediaTypeWithQualityHeaderValue[] headerValues)
        {
            if (headerValues.Length > 1)
            {
                // Use OrderBy() instead of Array.Sort() as it performs fewer comparisons. In this case the comparisons
                // are quite expensive so OrderBy() performs better.
                return headerValues.OrderByDescending(headerValue => 
                                                        headerValue,
                                                        MediaTypeWithQualityHeaderValueComparer.QualityComparer)
                                   .ToArray();
            }
            else
            {
                return headerValues;
            }
        }

        private IEnumerable<OutputFormatter> GetDefaultFormatters(ActionContext context)
        {
            IEnumerable<OutputFormatter> formatters = null;
            if (Formatters == null || Formatters.Count == 0)
            {
                formatters = context.HttpContext
                                    .RequestServices
                                    .GetService<IOptionsAccessor<MvcOptions>>()
                                    .Options
                                    .OutputFormatters
                                    .Select(descriptor => descriptor.OutputFormatter);
            }
            else
            {
                formatters = Formatters;
            }

            return formatters;
        }
    }
}