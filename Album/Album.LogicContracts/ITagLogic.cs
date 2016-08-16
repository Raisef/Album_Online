using Album.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Album.LogicContracts
{
    public interface ITagLogic
    {
        int Create(string name);
        bool Delete(int id);
        Tag Get(int id);
        List<Tag> GetAll();
        int GetAlbumTagCount(int id);
        int GetImageTagCount(int id);
    }
}
