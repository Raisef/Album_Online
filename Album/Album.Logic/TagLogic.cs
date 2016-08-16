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
    public class TagLogic : ITagLogic
    {
        private readonly ManagerDao DaoManager = ManagerDao.Instance;
        private readonly ILog Log = Logger.Instance.Log();

        public int Create(string name)
        {
            if(string.IsNullOrWhiteSpace(name)){
                return 0;
            }
            try
            {
                return DaoManager.TagDao().Create(name);
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
            if(id <= 0)
            {
                return false;
            }
            try
            {
                return DaoManager.TagDao().Delete(id);
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

        public Tag Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            try
            {
                return DaoManager.TagDao().Get(id);
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

        public int GetAlbumTagCount(int id)
        {
            if(id <= 0)
            {
                return -1;
            }
            try
            {
                return DaoManager.TagDao().GetAlbumTagCount(id);
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

        public int GetImageTagCount(int id)
        {
            if (id <= 0)
            {
                return -1;
            }
            try
            {
                return DaoManager.TagDao().GetImageTagCount(id);
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

        public List<Tag> GetAll()
        {
            List<Tag> tags = null;
            try
            {
                tags = DaoManager.TagDao().GetAll();
                if (tags == null || tags.Count == 0)
                {
                    return null;
                }
                return tags;
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
