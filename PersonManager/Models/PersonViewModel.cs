using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PersonManager.Models
{
    public class PersonViewModel
    {
        [Required(ErrorMessage = "Full name cannot be empty")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ID card number cannot be empty")]
        [Display(Name = "ID Card Number")]
        [RegularExpression(@"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", ErrorMessage = "ID card number invalid")]
        public string IdCardNum { get; set; }

        [Required(ErrorMessage = "Full name cannot be empty")]
        [Display(Name = "ID Card Image Attachment")]
        //[FileExtensions(Extensions = "jpg,png", ErrorMessage = "File format error")]
        [DataType(DataType.Upload)]
        public IFormFile IdCardImg { get; set; }
    }
}