using Album.Entities;
using Album.LogicContracts;
using Album.LogService;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Album.Logic
{
    public class UserLogic : IUserLogic
    {
        private readonly ManagerDao DaoManager = ManagerDao.Instance;
        private readonly ILog Log = Logger.Instance.Log();

        public bool ChangePassword(int id, string oldPass, string newPass)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(oldPass) || oldPass.Length < 4 ||
                string.IsNullOrWhiteSpace(newPass) || newPass.Length < 4 || newPass == oldPass)
            {
                return false;
            }
            try
            {
                string oldPassHash = GetPassHash(oldPass);
                string newPassHash = GetPassHash(newPass);
                return DaoManager.UserDao().ChangePassword(id, oldPassHash, newPassHash);
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

        public bool ChangeRole(int id, string roleName)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }
            try
            {
                return DaoManager.UserDao().ChangeRole(id, roleName);
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

        public int Create(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 2 ||
                string.IsNullOrWhiteSpace(password) || password.Length < 4)
            {
                return 0;
            }
            try
            {
                string passHash = GetPassHash(password);
                return DaoManager.UserDao().Create(userName, passHash);
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

        public User SignIn(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 2 ||
                string.IsNullOrWhiteSpace(password) || password.Length < 4)
            {
                return null;
            }
            try
            {
                string passHash = GetPassHash(password);
                return DaoManager.UserDao().SignIn(userName, passHash);
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

        public User GetById(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            try
            {
                return DaoManager.UserDao().GetById(id);
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

        public User GetByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }
            try
            {
                return DaoManager.UserDao().GetByName(userName);
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

        public List<User> GetAll()
        {
            List<User> users = null;
            try
            {
                users = DaoManager.UserDao().GetAll();
                if (users == null || users.Count == 0)
                {
                    return null;
                }
                return users;
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

        public List<User> GetAllByNamePath(string userName)
        {
            List<User> users = null;
            try
            {
                string userNamePath = "%%";
                userNamePath = userNamePath.Insert(1, userName);
                users = DaoManager.UserDao().GetAllByNamePath(userNamePath);
                if (users == null || users.Count == 0)
                {
                    return null;
                }
                return users;
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

        public string GetRole(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 2)
            {
                return null;
            }
            try
            {
                return DaoManager.UserDao().GetRole(userName);
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

        public bool IsUserInRole(string userName, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 2 ||
                string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }
            try
            {
                return DaoManager.UserDao().IsUserInRole(userName, roleName);
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
                var images = DaoManager.ImageDao().GetAllByUser(id);
                if (images != null)
                {
                    for (var i = 0; i < images.Count; i++)
                    {
                        DaoManager.ImageDao().Remove(images[i].Id);
                    }
                }
                var albums = DaoManager.AppAlbumDao().GetAllByUser(id);
                if(albums != null)
                {
                    for(var i = 0; i <albums.Count; i++)
                    {
                        DaoManager.AppAlbumDao().Delete(albums[i].Id);
                    }
                }
                return DaoManager.UserDao().Delete(id);
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

        public Image GetAvatar(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            try
            {
                return DaoManager.UserDao().GetAvatar(id);
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

        private string GetPassHash(string userPass, string key = "origano", string salt = "Bliss", string hashAlgorithm = "SHA1", int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY", int keySize = 256)
        {
            if (string.IsNullOrEmpty(userPass))
                return "";

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(userPass);

            PasswordDeriveBytes derivedPassword = new PasswordDeriveBytes(key, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = derivedPassword.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmetricKey.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }

        public bool SetAvatar(int id, Image avatar)
        {
            if (id <= 0 || avatar == null
                || avatar.Content == null || string.IsNullOrWhiteSpace(avatar.ImageType))
            {
                return false;
            }
            try
            {
                return DaoManager.UserDao().SetAvatar(id, avatar);
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

        public List<string> GetAllRoles()
        {
            try
            {
                return DaoManager.UserDao().GetAllRoles();
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