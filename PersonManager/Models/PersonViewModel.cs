using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PersonManager.Models
{
    public class PersonViewModel
    {
        public Person Person { get; set; }

        [Required(ErrorMessage = "Image cannot be empty")]
        [Display(Name = "ID Card Image Attachment")]
        [DataType(DataType.Upload)]
        public IFormFile IdCardImg { get; set; }
    }
}