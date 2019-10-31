using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MusicCollection2017.Models
{

    public enum Rating
    {
        A, B, C, D, E
    }

    public class Recording
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Display(Name="Backed up in OneDrive?")]
        public bool InCloud { get; set; }
        public Rating? Rating { get; set; }

        // foreign keys 
        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; }
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        [DataType(DataType.Url)]
        public string Url { get; set; }
    }
}