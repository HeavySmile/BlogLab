using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Models.Account
{
    public class ApplicationUserLogin
    {
        private const string _requiredError = "Username is required";
        private const string _usernameLengthError = "Must be at least 5-20 characters";
        private const string _passwordLengthError = "Must be at least 10-50 characters";

        [Required(ErrorMessage = _requiredError)]
        [MinLength(5, ErrorMessage = _usernameLengthError)]
        [MaxLength(20, ErrorMessage = _usernameLengthError)]
        public string Username { get; set; }
        
        [Required(ErrorMessage = _requiredError)]
        [MinLength(10, ErrorMessage = _passwordLengthError)]
        [MaxLength(50, ErrorMessage = _passwordLengthError)]
        public string Password { get; set; }
    }
}
