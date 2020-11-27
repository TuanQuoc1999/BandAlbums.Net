using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Models
{
    public class AlbumCreatingDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid BandId { get; set; }
    }
}
