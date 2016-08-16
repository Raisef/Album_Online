using Album.DalContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Album.Entities;
using System.Data.SqlClient;
using System.Data;

namespace Album.DBDal
{
    public class UserDao : IUserDao
    {
        private string _connectionString;

        public UserDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool ChangePassword(int id, string oldPass, string newPass)
        {
            if(id <= 0 || string.IsNullOrWhiteSpace(oldPass) || string.IsNullOrWhiteSpace(newPass))
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_ChangePassword";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", id);
                command.Parameters.AddWithValue("@OldPass", oldPass);
                command.Parameters.AddWithValue("@NewPass", newPass);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public bool ChangeRole(int id, string roleName)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_ChangeRole";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", id);
                command.Parameters.AddWithValue("@RoleName", roleName);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public int Create(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName)
                || string.IsNullOrWhiteSpace(password))
            {
                return 0;
            }
            int id = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_Create";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output });
                connection.Open();

                command.ExecuteNonQuery();
                id = (int)command.Parameters["@Id"].Value;
            }
            return id;
        }

        public bool Delete(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_Delete";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", id);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public User SignIn(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName)
                || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }
            User user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_SignIn";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@Password", password);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var role = reader["RoleName"] as string;
                    var avatarContent = reader["AvatarContent"] as byte[];
                    var avatarType = reader["AvatarType"] as string;
                    user = new User { Login = userName, Role = role, Avatar = new Image { Content = avatarContent, ImageType = avatarType } };
                }
            }
            return user;
        }

        public User GetById(int id)
        {
            if(id <= 0)
            {
                return null;
            }
            User user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_GetById";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string userName = reader["UserName"] as string;
                    var role = reader["RoleName"] as string;
                    var avatarContent = reader["AvatarContent"] as byte[];
                    var avatarType = reader["AvatarType"] as string;
                    user = new User { Id = id, Login = userName, Role = role, Avatar = new Image { Content = avatarContent, ImageType = avatarType } };
                }
            }
            return user;
        }

        public User GetByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }
            User user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_GetByName";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", userName);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["UserId"];
                    var role = reader["RoleName"] as string;
                    var avatarContent = reader["AvatarContent"] as byte[];
                    var avatarType = reader["AvatarType"] as string;
                    user = new User { Id = id, Login = userName, Role = role, Avatar = new Image { Content = avatarContent, ImageType = avatarType } };
                }
            }
            return user;
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_GetAll";
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var userId = (int)reader["UserId"];
                    var userName = reader["UserName"] as string;
                    var role = reader["RoleName"] as string;
                    var avatarContent = reader["AvatarContent"] as byte[];
                    var avatarType = reader["AvatarType"] as string;
                    users.Add(new User { Id = userId, Login = userName, Role = role, Avatar = new Image { Content = avatarContent, ImageType = avatarType } });
                }
            }
            return users.Count > 0 ? users : null;
        }

        public List<User> GetAllByNamePath(string userNamePath)
        {
            List<User> users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_GetAllByNamePath";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserNamePath", userNamePath);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var userId = (int)reader["UserId"];
                    var userName = reader["UserName"] as string;
                    var role = reader["RoleName"] as string;
                    var avatarContent = reader["AvatarContent"] as byte[];
                    var avatarType = reader["AvatarType"] as string;
                    users.Add(new User { Id = userId, Login = userName, Role = role, Avatar = new Image { Content = avatarContent, ImageType = avatarType } });
                }
            }
            return users.Count > 0 ? users : null;
        }

        public Image GetAvatar(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            Image avatar = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_GetAvatar";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var imgContent = reader["ImageContent"] as byte[];
                    var imgType = reader["ImageType"] as string;
                    if(imgContent != null && !string.IsNullOrWhiteSpace(imgType))
                    {
                        avatar = new Image { Content = imgContent, ImageType = imgType };
                    }
                }
            }
            return avatar;
        }

        public string GetRole(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }
            string role = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_GetRole";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", userName);
                connection.Open();
                role = command.ExecuteScalar() as string;
            }
            return role;
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            if(string.IsNullOrWhiteSpace(userName)
                || string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_IsUserInRole";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@RoleName", roleName);
                connection.Open();

                result = (int)command.ExecuteScalar();
            }
            return result > 0 ? true : false;
        }

        public bool SetAvatar(int id, Image avatar)
        {
            if (id <= 0 || avatar.Content == null || string.IsNullOrWhiteSpace(avatar.ImageType))
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "User_SetAvatar";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", id);
                command.Parameters.AddWithValue("@AvatarContent", avatar.Content);
                command.Parameters.AddWithValue("@AvatarType", avatar.ImageType);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public List<string> GetAllRoles()
        {
            List<string> roles = new List<string>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Roles_GetAll";
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    roles.Add(reader["RoleName"] as string);
                }
            }
            return roles.Count > 0 ? roles : null;
        }
    }
}
