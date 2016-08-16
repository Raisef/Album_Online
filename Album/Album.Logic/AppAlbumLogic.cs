using Album.LogicContracts;
using System;
using Album.Entities;
using Album.LogService;
using log4net;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Album.Logic
{
    public class AppAlbumLogic : IAppAlbumLogic
    {
        private readonly ManagerDao DaoManager = ManagerDao.Instance;
        private readonly ILog Log = Logger.Instance.Log();

        public bool AddTag(int albumId, int tagId)
        {
            if (albumId <= 0 || tagId <= 0)
            {
                return false;
            }
            try
            {
                return DaoManager.AppAlbumDao().AddTag(albumId, tagId);
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public int Create(int userId, string albumName)
        {
            if (userId <= 0 || string.IsNullOrWhiteSpace(albumName))
            {
                return 0;
            }
            try
            {
                return DaoManager.AppAlbumDao().Create(userId, albumName);
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            try
            {
                var images = DaoManager.ImageDao().GetAllByAlbum(id);
                if (images != null)
                {
                    for(var i = 0; i < images.Count; i++)
                    {
                        DaoManager.ImageDao().Remove(images[i].Id);
                    }
                }
                return DaoManager.AppAlbumDao().Delete(id);
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public AppAlbum Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            try
            {
                return DaoManager.AppAlbumDao().Get(id);
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public List<AppAlbum> GetAllByAlbumName(string albumName)
        {
            if (string.IsNullOrWhiteSpace(albumName) || albumName.Length < 2)
            {
                return null;
            }
            string pathOfName = "%%";
            pathOfName = pathOfName.Insert(1, albumName);
            List<AppAlbum> Albums = null;
            try
            {
                Albums = DaoManager.AppAlbumDao().GetAllByAlbumName(pathOfName);
                if (Albums == null || Albums.Count == 0)
                {
                    return null;
                }
                return Albums;
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public List<AppAlbum> GetAllByUser(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            List<AppAlbum> usersAlbums = null;
            try
            {
                usersAlbums = DaoManager.AppAlbumDao().GetAllByUser(id);
                if (usersAlbums == null || usersAlbums.Count == 0)
                {
                    return null;
                }
                return usersAlbums;
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public List<AppAlbum> GetAllByTag(int tagId)
        {
            if (tagId <= 0)
            {
                return null;
            }
            List<AppAlbum> Albums = null;
            try
            {
                Albums = DaoManager.AppAlbumDao().GetAllByTag(tagId);
                if (Albums == null || Albums.Count == 0)
                {
                    return null;
                }
                return Albums;
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public List<AppAlbum> GetAllByUserAndTag(int userId, int tagId)
        {
            if (tagId <= 0 || userId <= 0)
            {
                return null;
            }
            List<AppAlbum> usersAlbums = null;
            try
            {
                usersAlbums = DaoManager.AppAlbumDao().GetAllByUserAndTag(userId, tagId);
                if (usersAlbums == null || usersAlbums.Count == 0)
                {
                    return null;
                }
                return usersAlbums;
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public bool RemoveTag(int albumId, int tagId)
        {
            if (albumId <= 0 || tagId <= 0)
            {
                return false;
            }
            try
            {
                return DaoManager.AppAlbumDao().RemoveTag(albumId, tagId);
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public bool Rename(int id, string newAlbumName)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(newAlbumName))
            {
                return false;
            }
            try
            {
                return DaoManager.AppAlbumDao().Rename(id, newAlbumName);
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public bool SetTitleImage(int imageId)
        {
            if (imageId <= 0)
            {
                return false;
            }
            try
            {
                return DaoManager.AppAlbumDao().SetTitleImage(imageId);
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public int GetAlbumCount(int id)
        {
            if (id <= 0)
            {
                return -1;
            }
            try
            {
                return DaoManager.AppAlbumDao().GetAlbumCount(id);
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public Image GetTitleImage(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            try
            {
                return DaoManager.AppAlbumDao().GetTitleImage(id);
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public int GetCountByUser(int userId)
        {
            if(userId <= 0)
            {
                return -1;
            }
            try
            {
                return DaoManager.AppAlbumDao().GetCountByUser(userId);
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }

        public List<AppAlbum> GetAllByTagName(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                return null;
            }
            string pathOfName = "%%";
            pathOfName = pathOfName.Insert(1, tagName);
            List<AppAlbum> Albums = null;
            try
            {
                Albums = DaoManager.AppAlbumDao().GetAllByTagName(pathOfName);
                if (Albums == null || Albums.Count == 0)
                {
                    return null;
                }
                return Albums;
            }
            catch (System.IO.IOException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (SqlException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Log.Fatal("Fatal error on adding tag.", ex);
                throw;
            }
        }
    }
}