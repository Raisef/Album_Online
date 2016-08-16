using Album.LogicContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Album.Entities;
using System.Data.SqlClient;
using log4net;
using Album.LogService;

namespace Album.Logic
{
    public class ImageLogic : IImageLogic
    {
        private readonly ManagerDao DaoManager = ManagerDao.Instance;
        private readonly ILog Log = Logger.Instance.Log();

        public int Add(int albumId, byte[] imgContent, string imageType)
        {
            if(albumId <= 0 || string.IsNullOrWhiteSpace(imageType) || imgContent == null)
            {
                return 0;
            }
            try
            {
                return DaoManager.ImageDao().Add(albumId, imgContent, imageType);
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

        public bool Remove(int id)
        {
            if(id <= 0)
            {
                return false;
            }
            try
            {
                return DaoManager.ImageDao().Remove(id);
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

        public Image Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            try
            {
                return DaoManager.ImageDao().Get(id);
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

        public bool Rate(string userName, int id, int rate)
        {
            if(string.IsNullOrWhiteSpace(userName) || userName.Length < 2 || id <= 0 || rate <= 0 || rate > 5)
            {
                return false;
            }
            try
            {
                return DaoManager.ImageDao().Rate(userName, id, rate);
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

        public bool ChangeAlbum(int imgId, int albumId)
        {
            if (imgId <= 0 || albumId <= 0)
            {
                return false;
            }
            try
            {
                return DaoManager.ImageDao().ChangeAlbum(imgId, albumId);
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

        public Dictionary<User, int> GetAllRates(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            try
            {
                return DaoManager.ImageDao().GetAllRates(id);
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

        public double GetAverageRate(int id)
        {
            if (id <= 0)
            {
                return 0;
            }
            try
            {
                Dictionary<User, int> rates = GetAllRates(id);
                if(rates == null)
                {
                    return 0;
                }
                int sum = 0;
                int votes = 0;
                foreach (var x in rates)
                {
                    sum += x.Value;
                    votes++;
                }
                double rate = sum / votes;
                return Math.Round(rate,1);                
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

        public List<Tag> GetTags(int id)
        {
            if(id <= 0)
            {
                return null;
            }
            try
            {
                return DaoManager.ImageDao().GetTags(id);
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

        public bool AddTag(int imgId, int tagId)
        {
            if(imgId <= 0 || tagId <= 0)
            {
                return false;
            }
            try
            {
                return DaoManager.ImageDao().AddTag(imgId, tagId);
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

        public bool RemoveTag(int imgId, int tagId)
        {
            if (imgId <= 0 || tagId <= 0)
            {
                return false;
            }
            try
            {
                return DaoManager.ImageDao().RemoveTag(imgId, tagId);
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

        public List<Image> GetAllByAlbum(int albumId)
        {
            if(albumId <= 0)
            {
                return null;
            }
            List<Image> albumsImages = null;
            try
            {
                albumsImages = DaoManager.ImageDao().GetAllByAlbum(albumId);
                if (albumsImages == null || albumsImages.Count == 0)
                {
                    return null;
                }
                return albumsImages;
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

        public List<Image> GetAllByTag(int tagId)
        {
            if (tagId <= 0)
            {
                return null;
            }
            List<Image> Images = null;
            try
            {
                Images = DaoManager.ImageDao().GetAllByTag(tagId);
                if (Images == null || Images.Count == 0)
                {
                    return null;
                }
                return Images;
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

        public List<Image> GetAllByUser(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }
            List<Image> Images = null;
            try
            {
                Images = DaoManager.ImageDao().GetAllByUser(userId);
                if (Images == null || Images.Count == 0)
                {
                    return null;
                }
                return Images;
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

        public List<int> AddSeveral(List<Image> images)
        {
            if(images == null || !images.Any())
            {
                return null;
            }
            try
            {
                return DaoManager.ImageDao().AddSeveral(images);
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
            if (userId <= 0)
            {
                return -1;
            }
            try
            {
                return DaoManager.ImageDao().GetCountByUser(userId);
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
