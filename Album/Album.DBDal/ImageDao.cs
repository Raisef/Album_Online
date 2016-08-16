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
    public class ImageDao : IImageDao
    {
        private string _connectionString;

        public ImageDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Add(int albumId, byte[] imgContent, string imageType)
        {
            if (albumId <= 0 ||
                string.IsNullOrWhiteSpace(imageType) ||
                imgContent == null)
            {
                return 0;
            }
            int id = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_Add";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumId", albumId);
                command.Parameters.AddWithValue("@ImageContent", imgContent);
                command.Parameters.AddWithValue("@ImageType", imageType);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output });
                connection.Open();

                command.ExecuteNonQuery();
                id = (int)command.Parameters["@Id"].Value;

            }
            return id;
        }

        public bool AddTag(int imgId, int tagId)
        {
            if(imgId <= 0 || tagId <= 0)
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_AddTag";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ImageId", imgId);
                command.Parameters.AddWithValue("@TagId", tagId);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public bool ChangeAlbum(int imgId, int albumId)
        {
            if(imgId <= 0 || albumId <= 0)
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_ChangeAlbum";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ImageId", imgId);
                command.Parameters.AddWithValue("@AlbumId", albumId);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;

        }

        public bool Remove(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_Remove";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ImageId", id);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public Dictionary<User, int> GetAllRates(int id)
        {
            if(id <= 0)
            {
                return null;
            }
            Dictionary<User, int> rates = new Dictionary<User, int>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_GetAllRates";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ImageId", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        rates.Add(new User { Id = (int)reader["UserId"], Login = reader["UserName"] as string,
                            Role = reader["UserRole"] as string,
                            Avatar = new Image { ImageType = reader["AvatarType"] as string, Content = reader["AvatarContent"] as byte[] } }, 
                            (int)reader["Rate"]);
                    }
                }
            }
            return rates.Count != 0 ? rates : null;
        }

        public Image Get(int id)
        {
            if(id <= 0)
            {
                return null;
            }
            Image img = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_Get";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ImageId", id);
                connection.Open();
                var reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    string owner = reader["UserName"] as string;
                    string album = reader["Album"] as string;
                    string imageType = reader["ImageType"] as string;
                    byte[] content = reader["ImageContent"] as byte[];
                    img = new Image { Id = id, Album = album, Content = content, ImageType = imageType, Owner = owner };
                }                
            }
            if (img == null)
            {
                return null;
            }
            List<Tag> tags = GetTags(id);
            img.Tags = tags;
            return img;
        }

        public List<Image> GetAllByAlbum(int albumId)
        {
            if(albumId <= 0)
            {
                return null;
            }
            List<Image> images = new List<Image>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_GetAllByAlbum";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumId", albumId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["ImageId"];
                    string owner = reader["UserName"] as string;
                    string albumName = reader["AlbumName"] as string;
                    string imageType = reader["ImageType"] as string;
                    byte[] imageContent = reader["ImgContent"] as byte[];
                    var tags = GetTags(id);
                    images.Add(new Image { Id = id, Album = albumName, Owner = owner, Tags = tags, ImageType = imageType, Content = imageContent});
                }
            }
            return images.Count != 0 ? images : null;
        }

        public List<Image> GetAllByTag(int tagId)
        {
            if (tagId <= 0)
            {
                return null;
            }
            List<Image> images = new List<Image>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_GetAllByTag";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TagId", tagId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["ImageId"];
                    string owner = reader["UserName"] as string;
                    string albumName = reader["AlbumName"] as string;
                    string imageType = reader["ImageType"] as string;
                    byte[] imageContent = reader["ImgContent"] as byte[];
                    var tags = GetTags(id);
                    images.Add(new Image { Id = id, Album = albumName, Owner = owner, Tags = tags, ImageType = imageType, Content = imageContent });
                }
            }
            return images.Count != 0 ? images : null;
        }

        public List<Image> GetAllByUser(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }
            List<Image> images = new List<Image>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_GetAllByUser";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["ImageId"];
                    string owner = reader["UserName"] as string;
                    string albumName = reader["AlbumName"] as string;
                    string imageType = reader["ImageType"] as string;
                    byte[] imageContent = reader["ImgContent"] as byte[];
                    var tags = GetTags(id);
                    images.Add(new Image { Id = id, Album = albumName, Owner = owner, Tags = tags, ImageType = imageType, Content = imageContent });
                }
            }
            return images.Count != 0 ? images : null;
        }

        public List<Tag> GetTags(int id)
        {
            if(id <= 0)
            {
                return null;
            }
            List<Tag> tags = new List<Tag>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_GetTags";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ImageId", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tags.Add(new Tag { Id = (int)reader["Id"], Name = reader["Name"] as string });
                }
            }
            return tags.Count != 0 ? tags : null;
        }

        public bool Rate(string userName, int id, int rate)
        {
            if(string.IsNullOrWhiteSpace(userName) || id <= 0 || rate <= 0 || rate > 5)
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_Rate";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ImageId", id);
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@Rate", rate);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public bool RemoveTag(int imgId, int tagId)
        {
            if(imgId <= 0 || tagId <= 0)
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_RemoveTag";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ImageId", imgId);
                command.Parameters.AddWithValue("@TagId", tagId);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public List<int> AddSeveral(List<Image> images)
        {
            if(images == null || !images.Any())
            {
                return null;
            }
            List<int> imagesId = new List<int>();
            for(var i = 0; i < images.Count; i++)
            {
                
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = connection.CreateCommand();
                    command.CommandText = "Image_Add";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AlbumId", images[i].AlbumId);
                    command.Parameters.AddWithValue("@ImageContent", images[i].Content);
                    command.Parameters.AddWithValue("@ImageType", images[i].ImageType);
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output });
                    connection.Open();

                    command.ExecuteNonQuery();
                    imagesId.Add((int)command.Parameters["@Id"].Value);
                }
            }
            return imagesId.Count > 0 ? imagesId : null;
        }

        public int GetCountByUser(int userId)
        {
            if (userId <= 0)
            {
                return -1;
            }
            int? count = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Image_GetCountByUser";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();

                count = command.ExecuteScalar() as int?;
            }

            return count ?? 0;
        }
    }
}
