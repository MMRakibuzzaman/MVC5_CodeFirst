using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _1291263_MVC_CodeFirst.Models
{
    public enum JerseySize { S, M, L, XL, XXL }

    public class Team
    {
        public int TeamId { get; set; }

        [Required, StringLength(50), Display(Name = "Team Name")]
        public string TeamName { get; set; }

        public virtual ICollection<Jersey> Jerseys { get; set; } = new List<Jersey>();
    }

    public class Jersey
    {
        public int JerseyId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, Column(TypeName = "date"), Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public bool IsAvailable { get; set; }

        public string Picture { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }

        public virtual Team Team { get; set; }
        public virtual ICollection<JerseyStock> JerseyStocks { get; set; } = new List<JerseyStock>();
    }

    public class JerseyStock
    {
        public int JerseyStockId { get; set; }

        [Required]
        public JerseySize Size { get; set; }

        [Required]
        [Range(0, 99999)]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [ForeignKey("Jersey")]
        public int JerseyId { get; set; }

        public virtual Jersey Jersey { get; set; }
    }
}