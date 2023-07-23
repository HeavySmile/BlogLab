using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Models.Blog
{
    public class BlogCreate
    {
        private const string _titleRequiredError = "Title is required";
        private const string _contentRequiredError = "Content is required";
        private const string _titleLengthError = "Must be at least 10-50 characters";
        private const string _contentLengthError = "Must be at least 300-3000 characters";

        public int BlogId { get; set; }

        [Required(ErrorMessage = _titleRequiredError)]
        [MinLength(10, ErrorMessage = _titleLengthError)]
        [MaxLength(50, ErrorMessage = _titleLengthError)]
        public string Title { get; set; }

        [Required(ErrorMessage = _contentRequiredError)]
        [MinLength(300, ErrorMessage = _contentLengthError)]
        [MaxLength(3000, ErrorMessage = _contentLengthError)]
        public string Content { get; set; }
        
        public int? PhotoId { get; set; }
    }
}
