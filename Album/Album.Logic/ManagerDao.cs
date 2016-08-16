using Album.DalContracts;
using Album.DBDal;
using Album.LogService;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Album.Logic
{
    public class ManagerDao
    {
        private static ManagerDao _instance = new ManagerDao();
        private IAppAlbumDao _appAlbumDao;
        private IImageDao _imageDao;
        private IUserDao _userDao;
        private ITagDao _tagDao;

        private ManagerDao()
        {
            string dalType = ConfigurationManager.AppSettings["DalType"];
            switch (dalType.ToLower())
            {
                case "db":
                    var connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new ConfigurationErrorsException("Invalid ConnectionString");
                    }
                    _appAlbumDao = new AppAlbumDao(connectionString);
                    _imageDao = new ImageDao(connectionString);
                    _userDao = new UserDao(connectionString);
                    _tagDao = new TagDao(connectionString);
                    break;

                default:
                    throw new ConfigurationErrorsException($"Invalid dalType:{dalType}");
            }
        }
        public static ManagerDao Instance
        {
            get
            {
                return _instance;
            }
        }
        public IAppAlbumDao AppAlbumDao() => _appAlbumDao;
        public IUserDao UserDao() => _userDao;
        public IImageDao ImageDao() => _imageDao;
        public ITagDao TagDao() => _tagDao;
    }
}

