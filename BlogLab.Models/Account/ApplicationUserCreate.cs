using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Models.Account
{
    public class ApplicationUserCreate : ApplicationUserLogin
    {
        private const string _fullnameLengthError = "Must be at least 10-30 characters";
        private const string _emailLengthError = "Can be at most 30 characters";
        private const string _emailRequiredError = "Email is required";
        private const string _emailAddressValidationError = "Invalid email format";

        [MinLength(10, ErrorMessage = _fullnameLengthError)]
        [MaxLength(30, ErrorMessage = _fullnameLengthError)]
        public string Fullname { get; set; }

        [Required(ErrorMessage = _emailRequiredError)]
        [MaxLength(30, ErrorMessage = _emailLengthError)]
        [EmailAddress(ErrorMessage = _emailAddressValidationError)]
        public string Email { get; set; }
    }
}
