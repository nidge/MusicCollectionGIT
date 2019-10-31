using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using System.Data.Entity.Infrastructure;
using System.Text;
using System.Xml;

// namespace for the EdmxWriter class

namespace MusicCollection2017.Models
{
    public class Artist
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength=1, ErrorMessage = "Artist title cannot be longer than 100 characters")]
        public string Title { get; set; }

        public ICollection<Recording> Recordings { get; set; }
    }
}