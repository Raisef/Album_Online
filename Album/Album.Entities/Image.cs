using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Album.Entities
{
    public class Image
    {
        public string Owner { get; set; }
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public string ImageType { get; set; }
        public string Album { get; set; }
        public int AlbumId { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
