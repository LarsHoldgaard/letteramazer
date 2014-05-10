using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace LetterAmazer.Websites.Client.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static void AddBusinessError(this ModelStateDictionary modelState, string message)
        {
            modelState.AddModelError("Business", message);
        }

        public static HtmlString BusinessErrorMessageBox(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewData.ModelState.ContainsKey("Business"))
            {
                ModelState modelState = htmlHelper.ViewData.ModelState["Business"];
                return MessageBox(htmlHelper, MessageBoxType.Error, "An error occured",
                    string.Format("<p>{0}</p>", htmlHelper.Encode(modelState.Errors[0].ErrorMessage)), false);
            }
            return null;
        }

        public static HtmlString MessageBox(this HtmlHelper htmlHelper, MessageBoxType type, string title, string message, bool encodeMessage)
        {
            return new HtmlString(new StringBuilder()
                .AppendFormat("<div class=\"alert alert-block alert-{0}\"><h4>{1}</h4>{2}</div>", type.ToString().ToLower(),
                    htmlHelper.Encode(title), encodeMessage ? "<p>" + htmlHelper.Encode(message) + "</p>" : message)
                .ToString());
        }

        public static HtmlString MessageBox(this HtmlHelper htmlHelper, MessageBoxType type, string title, string message)
        {
            return MessageBox(htmlHelper, type, title, message, true);
        }

        public static HtmlString MessageBox(this HtmlHelper htmlHelper, MessageBoxType type, string message, bool encodeMessage)
        {
            return MessageBox(htmlHelper, type, type.ToString(), message, encodeMessage);
        }

        public static HtmlString MessageBox(this HtmlHelper htmlHelper, MessageBoxType type, string message)
        {
            return MessageBox(htmlHelper, type, message, true);
        }

        public enum MessageBoxType
        {
            Info,
            Error,
            Warning,
            Success
        }
    }
}