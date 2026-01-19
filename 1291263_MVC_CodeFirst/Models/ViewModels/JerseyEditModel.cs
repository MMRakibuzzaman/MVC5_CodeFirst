using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _1291263_MVC_CodeFirst.Models.ViewModels
{
    public class JerseyEditModel
    {
        public int JerseyId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        public bool IsAvailable { get; set; }

        public HttpPostedFileBase Picture { get; set; }

        public string ExistingPicture { get; set; }

        [Required, Display(Name = "Team")]
        public int TeamId { get; set; }

        public virtual List<JerseyStock> Stocks { get; set; } = new List<JerseyStock>();
    }
}