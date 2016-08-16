using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Album.Entities
{
    public class AppAlbum
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int TitleImageId { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
