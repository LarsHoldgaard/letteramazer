using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LetterAmazer.Websites.Client.Extensions
{
    public static class FormatExtensions
    {
        public static string ToFriendlyMoney(this decimal money)
        {
            //return money.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            return money.ToString("F2");
        }

        public static string ToFriendlyDateTime(this DateTime date)
        {
            //return money.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
            return date.ToString("dd-MM-yyyy H:mm");
        }
    }
}