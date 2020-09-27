using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonManager.Models
{
    public class Person
    {
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Full name cannot be empty")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ID card number cannot be empty")]
        [Display(Name = "ID Card")]
        [RegularExpression(@"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", ErrorMessage = "ID card number invalid")]
        public string IdCardNum { get; set; }

        [Required]
        [Display(Name = "ID Card Url")]
        public string IdCardImgUrl { get; set; }
    }
}
