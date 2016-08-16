using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using log4net;
using log4net.Config;


namespace Album.LogService
{
    public class Logger
    {
        private static Logger _instance = new Logger();
        private ILog _log;

        private Logger()
        {
            _log = LogManager.GetLogger(typeof(Logger));
            XmlConfigurator.Configure();
        }

        public static Logger Instance
        {
            get
            {
                return _instance;
            }
        }

        public ILog Log() => _log;
    }
}
