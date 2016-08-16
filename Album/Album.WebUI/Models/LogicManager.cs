using Album.Logic;
using Album.LogicContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;
using Album.LogService;

namespace Album.WebUI.Models
{
    public class LogicManager
    {
        private readonly ILog Log = Logger.Instance.Log();
        private static LogicManager _instance = new LogicManager();
        private IUserLogic _userLogic;
        private IAppAlbumLogic _appAlbumLogic;
        private IImageLogic _imageLogic;
        private ITagLogic _tagLogic;

        private LogicManager()
        {
            try
            {
                _userLogic = new UserLogic();
                _appAlbumLogic = new AppAlbumLogic();
                _imageLogic = new ImageLogic();
                _tagLogic = new TagLogic();
            }
            catch (ConfigurationErrorsException ex)
            {
                Log.Fatal("Error in configurations", ex);
                throw;
            }
        }

        public static LogicManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public IAppAlbumLogic AppAlbumLogic() => _appAlbumLogic;

        public IUserLogic UserLogic() => _userLogic;

        public IImageLogic ImageLogic() => _imageLogic;

        public ITagLogic TagLogic() => _tagLogic;
    }
}