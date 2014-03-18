using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(LetterAmazer.Websites.Client.App_Start.RegisterClientValidationExtensions), "Start")]
 
namespace LetterAmazer.Websites.Client.App_Start {
    public static class RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}