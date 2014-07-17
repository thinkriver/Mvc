using System;
using Microsoft.AspNet.Mvc.Razor.TagHelpers;
using Microsoft.AspNet.Mvc.Rendering;

namespace MvcSample.Web
{
    public class ButtonTagHelper : MvcTagHelper
    {
	    public ButtonTagHelper()
            : base("aasdfasdf", "pasdfasd")
	    {
	    }

        public override MvcTagHelperAttributeInfo[] Attributes
        {
            get
            {
                return new MvcTagHelperAttributeInfo[0];
            }
        }

        public override void Process(TagBuilder builder, MvcTagHelperContext context)
        {
            bool modify = false;

            if (builder.TagName == "a")
            {
                string cls;

                builder.Attributes.TryGetValue("class", out cls);

                if(cls != null && cls.Contains("btn"))
                {
                    modify = true;
                }
            }
            else
            {
                modify = true;
            }

            if(modify)
            {
                if (builder.Attributes.ContainsKey("style"))
                {
                    builder.Attributes["style"] += ";font-size:2em;";
                }
                else
                {
                    builder.Attributes.Add("style", "font-size:2em;");
                }
            }
        }
    }
}