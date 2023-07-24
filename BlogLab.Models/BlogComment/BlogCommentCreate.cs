using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Models.BlogComment
{
    public class BlogCommentCreate
    {
        private const string _contentRequiredError = "Content is required";
        private const string _contentLengthError = "Must be at least 10-300 characters";

        public int BlogCommentId { get; set; }
        public int? ParentBlogCommentId { get; set; }
        public int BlogId { get; set; }

        [Required(ErrorMessage = _contentRequiredError)]
        [MinLength(10, ErrorMessage = _contentLengthError)]
        [MaxLength(300, ErrorMessage = _contentLengthError)]
        public string Content { get; set; }
    }
}
