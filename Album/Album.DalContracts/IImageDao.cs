using Album.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Album.DalContracts
{
    public interface IImageDao
    {
        int Add(int albumId, byte[] imgContent, string imageType);
        List<int> AddSeveral(List<Image> images);
        bool Remove(int id);
        bool Rate(string userName, int id, int rate);
        Dictionary<User, int> GetAllRates(int id);
        bool ChangeAlbum(int imgId, int albumId);
        Image Get(int id);
        List<Image> GetAllByAlbum(int albumId);
        List<Image> GetAllByTag(int tagId);
        List<Image> GetAllByUser(int userId);
        List<Tag> GetTags(int id);
        bool AddTag(int imgId, int tagId);
        bool RemoveTag(int imgId, int tagId);
        int GetCountByUser(int userId);
    }
}
