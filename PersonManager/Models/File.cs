using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PersonManager.Models
{
    public class File
    {
        //[Required(ErrorMessage = "Image cannot be empty")]
        [Display(Name = "ID Card Image")]
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }
    }
}