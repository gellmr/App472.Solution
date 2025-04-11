using App472.WebUI.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace App472.WebUI.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter your First Name")]
        [RegularExpression(OkInputs.Name, ErrorMessage=OkInputs.NameErr)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your Last Name")]
        [RegularExpression(OkInputs.Name, ErrorMessage = OkInputs.NameErr)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]
        [RegularExpression(OkInputs.Line, ErrorMessage = OkInputs.LineErr)]
        [Display(Name = "Line 1")]
        public string Line1 { get; set; }

        [RegularExpression(OkInputs.Line, ErrorMessage = OkInputs.LineErr)]
        [Display(Name = "Line 2")]
        public string Line2 { get; set; }

        [RegularExpression(OkInputs.Line, ErrorMessage = OkInputs.LineErr)]
        [Display(Name = "Line 3")]
        public string Line3 { get; set; }

        [RegularExpression(OkInputs.Name, ErrorMessage = OkInputs.NameErr)]
        [Required(ErrorMessage = "Please enter a city name")]
        public string City { get; set; }

        [RegularExpression(OkInputs.Name, ErrorMessage = OkInputs.NameErr)]
        [Required(ErrorMessage = "Please enter a state name")]
        public string State { get; set; }
        public string Zip { get; set; }

        [RegularExpression(OkInputs.Name, ErrorMessage = OkInputs.NameErr)]
        [Required(ErrorMessage = "Please enter a country name")]
        public string Country { get; set; }

        [RegularExpression(OkInputs.Email, ErrorMessage = OkInputs.EmailErr)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public bool GiftWrap { get; set; }
    }
}
