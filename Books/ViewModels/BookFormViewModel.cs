using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Books.Models;
using System.Web;

namespace Books.ViewModels
{
    public class BookFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [Required]
        [MaxLength(128)]
        public string Author { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        [Display(Name = "Categories")]
        public byte CategoryId { get; set; }

        public IEnumerable<Category> categories { get; set; }
    }
}