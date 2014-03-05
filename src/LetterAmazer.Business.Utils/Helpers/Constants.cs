using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetterAmazer.Business.Utils.Helpers
{
    public class Constants
    {
        public const string Sitename = "letteramazer.com";

        public const string GAnlyticsId = "UA-17514238-1";
        public const string GoogleMapsApiCall ="https://maps.googleapis.com/maps/api/js?key=AIzaSyBulh9LO7V3Eb80BF0-h6BW6MyiO-qNTC0&sensor=false";

        public class Path
        {
            public class Admin
            {
            }

            public class Data
            {
            }

            public class Images
            {

            }

            public class Website
            {
                public const string Frontpage = "~/";
            }
        }

        public class Texts
        {
            public class SeoTitle
            {

            }

            public class MetaDescription
            {

            }

            public class ErrorMessages
            {

            }

            public class PracticalInformation
            {
                public const string PhoneNr = "(+45) 6179 3650";
                public const string Address1 = "Måløv Hovedgade 58C st. th.";
                public const string City = "Måløv";
                public const string Zipcode = "2760";
                public const string VatNumber = "33113544";
                public const string BankName = "Nordea";
                public const string BankReg = "2279";
                public const string BankAccount = "8971983559";
                public const string Iban = "DK4420008971983559";
                public const string Swift = "NDEADKKK";

                public const string CompanyName = "LetterAmazer IvS";
                public const string Country = "Denmark";
                public const int CountryId = 59;
                public const string AttPerson = "Lars Holdgaard";

            }

            public class InfoTexts
            {

            }

            public class Email
            {



                public class Subjects
                {

                }
            }
        }

        public class RouteVariables
        {
            public const string Article = "article";
            public const string OfferForm = "offers";
            public const string Forumcategory = "forumname";
            public const string Post = "posturl";
            public const string UserProfileId = "userid";
            public const string UserProfileProfilename = "userprofilename";
            public const string Tags = "tags";
            public const string Assignment = "assignment";
            public const string TutorSubject = "tutorsubject";
            public const string TutorCity = "tutorcity";
            public const string TutorLevel = "tutorlevel";

            public const string SubjectAction = "subjectionaction";

            public const string SubjectChosen = "subjectchosen";


        }

        public class Sessions
        {
            public const string UploadPathSession = "UploadedFilePath";
        }

        public class Cookies
        {

        }

        public class Querystrings
        {
        }

        public class Regex
        {
            public const string EmailRegex = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        }

        public class Cache
        {

        }

        public class Payment
        {
            public const decimal StdVat = 25m;

        }
    }
}
