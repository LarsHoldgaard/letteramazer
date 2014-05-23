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
                return MessageBox(htmlHelper, MessageBoxType.Danger,
                    string.Format("<p>{0}</p>", htmlHelper.Encode(modelState.Errors[0].ErrorMessage)), false);
            }
            return null;
        }

        public static HtmlString MessageBox(this HtmlHelper htmlHelper, MessageBoxType type, string message, bool encodeMessage)
        {
            return new HtmlString(new StringBuilder()
                .AppendFormat("<div class=\"bg-{0}\">{1}</div>", type.ToString().ToLower()
                    , encodeMessage ? "<p>" + htmlHelper.Encode(message) + "</p>" : message)
                .ToString());
        }



        public static HtmlString MessageBox(this HtmlHelper htmlHelper, MessageBoxType type, string message)
        {
            return MessageBox(htmlHelper, type, message, true);
        }

        public enum MessageBoxType
        {
            Primary,
            Danger,
            Info,
            Warning,
            Success
        }
    }
}