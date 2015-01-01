// <auto-generated />
namespace Microsoft.AspNet.Mvc.WebApiCompatShim
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Microsoft.AspNet.Mvc.WebApiCompatShim.Resources", typeof(Resources).GetTypeInfo().Assembly);

        /// <summary>
        /// The request is invalid.
        /// </summary>
        internal static string HttpError_BadRequest
        {
            get { return GetString("HttpError_BadRequest"); }
        }

        /// <summary>
        /// The request is invalid.
        /// </summary>
        internal static string FormatHttpError_BadRequest()
        {
            return GetString("HttpError_BadRequest");
        }

        /// <summary>
        /// An error has occurred.
        /// </summary>
        internal static string HttpError_GenericError
        {
            get { return GetString("HttpError_GenericError"); }
        }

        /// <summary>
        /// An error has occurred.
        /// </summary>
        internal static string FormatHttpError_GenericError()
        {
            return GetString("HttpError_GenericError");
        }

        /// <summary>
        /// The model state is valid.
        /// </summary>
        internal static string HttpError_ValidModelState
        {
            get { return GetString("HttpError_ValidModelState"); }
        }

        /// <summary>
        /// The model state is valid.
        /// </summary>
        internal static string FormatHttpError_ValidModelState()
        {
            return GetString("HttpError_ValidModelState");
        }

        /// <summary>
        /// Could not find a formatter matching the media type '{0}' that can write an instance of '{1}'.
        /// </summary>
        internal static string HttpRequestMessage_CouldNotFindMatchingFormatter
        {
            get { return GetString("HttpRequestMessage_CouldNotFindMatchingFormatter"); }
        }

        /// <summary>
        /// Could not find a formatter matching the media type '{0}' that can write an instance of '{1}'.
        /// </summary>
        internal static string FormatHttpRequestMessage_CouldNotFindMatchingFormatter(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HttpRequestMessage_CouldNotFindMatchingFormatter"), p0, p1);
        }

        /// <summary>
        /// The {0} instance is not properly initialized. Use {1} to create an {0} for the current request.
        /// </summary>
        internal static string HttpRequestMessage_MustHaveHttpContext
        {
            get { return GetString("HttpRequestMessage_MustHaveHttpContext"); }
        }

        /// <summary>
        /// The {0} instance is not properly initialized. Use {1} to create an {0} for the current request.
        /// </summary>
        internal static string FormatHttpRequestMessage_MustHaveHttpContext(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HttpRequestMessage_MustHaveHttpContext"), p0, p1);
        }

        /// <summary>
        /// The {0} only supports writing objects of type {1}.
        /// </summary>
        internal static string HttpResponseMessageFormatter_UnsupportedType
        {
            get { return GetString("HttpResponseMessageFormatter_UnsupportedType"); }
        }

        /// <summary>
        /// The {0} only supports writing objects of type {1}.
        /// </summary>
        internal static string FormatHttpResponseMessageFormatter_UnsupportedType(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("HttpResponseMessageFormatter_UnsupportedType"), p0, p1);
        }

        /// <summary>
        /// The key is invalid JQuery syntax because it is missing a closing bracket.
        /// </summary>
        internal static string JQuerySyntaxMissingClosingBracket
        {
            get { return GetString("JQuerySyntaxMissingClosingBracket"); }
        }

        /// <summary>
        /// The key is invalid JQuery syntax because it is missing a closing bracket.
        /// </summary>
        internal static string FormatJQuerySyntaxMissingClosingBracket()
        {
            return GetString("JQuerySyntaxMissingClosingBracket");
        }

        /// <summary>
        /// The number of keys in a NameValueCollection has exceeded the limit of '{0}'. You can adjust it by modifying the MaxHttpCollectionKeys property on the '{1}' class.
        /// </summary>
        internal static string MaxHttpCollectionKeyLimitReached
        {
            get { return GetString("MaxHttpCollectionKeyLimitReached"); }
        }

        /// <summary>
        /// The number of keys in a NameValueCollection has exceeded the limit of '{0}'. You can adjust it by modifying the MaxHttpCollectionKeys property on the '{1}' class.
        /// </summary>
        internal static string FormatMaxHttpCollectionKeyLimitReached(object p0, object p1)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("MaxHttpCollectionKeyLimitReached"), p0, p1);
        }

        /// <summary>
        /// Processing of the HTTP request resulted in an exception. Please see the HTTP response returned by the 'Response' property of this exception for details.
        /// </summary>
        internal static string HttpResponseExceptionMessage
        {
            get { return GetString("HttpResponseExceptionMessage"); }
        }

        /// <summary>
        /// Processing of the HTTP request resulted in an exception. Please see the HTTP response returned by the 'Response' property of this exception for details.
        /// </summary>
        internal static string FormatHttpResponseExceptionMessage()
        {
            return GetString("HttpResponseExceptionMessage");
        }

        /// <summary>
        /// Failed to generate a URL using route '{0}'.
        /// </summary>
        internal static string CreatedAtRoute_RouteFailed
        {
            get { return GetString("CreatedAtRoute_RouteFailed"); }
        }

        /// <summary>
        /// Failed to generate a URL using route '{0}'.
        /// </summary>
        internal static string FormatCreatedAtRoute_RouteFailed(object p0)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("CreatedAtRoute_RouteFailed"), p0);
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
    }
}
