using Album.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Album.LogicContracts
{
    public interface IAppAlbumLogic
    {
        AppAlbum Get(int id);
        List<AppAlbum> GetAllByAlbumName(string albumName);
        List<AppAlbum> GetAllByUser(int userId);
        List<AppAlbum> GetAllByTag(int tagId);
        List<AppAlbum> GetAllByUserAndTag(int userId, int tagId);
        List<AppAlbum> GetAllByTagName(string tagName);
        int GetAlbumCount(int id);
        int Create(int userId, string albumName);
        bool Delete(int id);
        bool Rename(int id, string newAlbumName);
        bool SetTitleImage(int imageId);
        bool AddTag(int albumId, int tagId);
        bool RemoveTag(int albumId, int tagId);
        int GetCountByUser(int userId);
    }
}
