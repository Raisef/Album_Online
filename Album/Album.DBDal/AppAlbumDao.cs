using Album.DalContracts;
using Album.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;

namespace Album.DBDal
{
    public class AppAlbumDao : IAppAlbumDao
    {
        private string _connectionString;

        public AppAlbumDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool AddTag(int albumId, int tagId)
        {
            if (albumId <= 0 || tagId <= 0)
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_AddTag";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumId", albumId);
                command.Parameters.AddWithValue("@TagId", tagId);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public int Create(int userId, string albumName)
        {
            if (userId <= 0 || string.IsNullOrWhiteSpace(albumName))
            {
                return 0;
            }
            int id = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_Create";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@AlbumName", albumName);
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output } );
                connection.Open();

                command.ExecuteNonQuery();
                id = (int)command.Parameters["@id"].Value;

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
                command.CommandText = "Album_Delete";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumId", id);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public AppAlbum Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            string userName = string.Empty;
            string albumName = string.Empty;
            int? titleImageId = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_Touch";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumId", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        userName = reader["UserName"] as string;
                        albumName = reader["AlbumName"] as string;
                        titleImageId = reader["TitleImageId"] as int?;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(albumName))
            {
                return null;
            }
            int count = GetAlbumCount(id);
            var album = new AppAlbum { Id = id, Count = count, Name = albumName, TitleImageId = 0, Owner = userName, Tags = null };
            if (titleImageId != null)
            {
                album.TitleImageId = (int)titleImageId;
            }
            var tags = GetTags(id);
            if (tags != null)
            {
                album.Tags = tags;
            }
            return album;
        }

        public int GetAlbumCount(int albumId)
        {
            int count = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_GetImagesCount";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumId", albumId);
                connection.Open();

                count = (int)command.ExecuteScalar();
            }

            return count;
        }

        public List<AppAlbum> GetAllByAlbumName(string albumName)
        {
            if (string.IsNullOrWhiteSpace(albumName))
            {
                return null;
            }
            List<AppAlbum> albums = new List<AppAlbum>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_GetAllByAlbumName";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumName", albumName);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["AlbumId"];
                    string name = reader["AlbumName"] as string;
                    int count = GetAlbumCount(id);
                    string owner = reader["UserName"] as string;
                    var tags = GetTags(id);
                    int? imageId = reader["TitleImageId"] as int?;
                    
                    albums.Add(new AppAlbum { Id = id, Name = name, Count = count, Owner = owner, Tags = tags,  TitleImageId = imageId??0});
                }
            }
            return albums.Count != 0 ? albums : null;
        }

        public List<AppAlbum> GetAllByUser(int userId)
        {
            if (userId <= 0)
            {
                return null;
            }
            List<AppAlbum> albums = new List<AppAlbum>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_GetAllByUser";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["AlbumId"];
                    string name = reader["AlbumName"] as string;
                    int count = GetAlbumCount(id);
                    string owner = reader["UserName"] as string;
                    var tags = GetTags(id);
                    int? imageId = reader["TitleImageId"] as int?;
                    albums.Add(new AppAlbum { Id = id, Name = name, Count = count, Owner = owner, Tags = tags, TitleImageId = imageId??0 });
                }
            }
            return albums.Count != 0 ? albums : null;
        }

        public List<AppAlbum> GetAllByTag(int tagId)
        {
            if (tagId <= 0)
            {
                return null;
            }
            List<AppAlbum> albums = new List<AppAlbum>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_GetAllByTag";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TagId", tagId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["AlbumId"];
                    string name = reader["AlbumName"] as string;
                    int count = GetAlbumCount(id);
                    string owner = reader["UserName"] as string;
                    var tags = GetTags(id);
                    int? imageId = reader["TitleImageId"] as int?;
                    albums.Add(new AppAlbum { Id = id, Name = name, Count = count, Owner = owner, Tags = tags, TitleImageId = imageId??0 });
                }
            }
            return albums.Count != 0 ? albums : null;
        }

        public List<AppAlbum> GetAllByUserAndTag(int userId, int tagId)
        {
            if (userId <= 0 || tagId <= 0)
            {
                return null;
            }
            List<AppAlbum> albums = new List<AppAlbum>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_GetAllByUserAndTag";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@TagId", tagId);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["AlbumId"];
                    string name = reader["AlbumName"] as string;
                    int count = GetAlbumCount(id);
                    string owner = reader["UserName"] as string;
                    var tags = GetTags(id);
                    int? imageId = reader["TitleImageId"] as int?;
                    albums.Add(new AppAlbum { Id = id, Name = name, Count = count, Owner = owner, Tags = tags, TitleImageId = imageId ?? 0 });
                }
            }
            return albums.Count != 0 ? albums : null;
        }

        public bool RemoveTag(int albumId, int tagId)
        {
            if (albumId <= 0 || tagId <= 0)
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_RemoveTag";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumId", albumId);
                command.Parameters.AddWithValue("@TagId", tagId);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public bool Rename(int id, string newAlbumName)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(newAlbumName))
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_Rename";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumId", id);
                command.Parameters.AddWithValue("@NewAlbumName", newAlbumName);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public bool SetTitleImage(int imageId)
        {
            if (imageId <= 0)
            {
                return false;
            }
            int result = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_SetTitleImage";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ImageId", imageId);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
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
                command.CommandText = "Album_GetTags";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumId", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tags.Add(new Tag { Id = (int)reader["TagId"], Name = reader["TagName"] as string });
                }
            }
            return tags.Count != 0 ? tags : null;
        }

        public Image GetTitleImage(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            Image title = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_GetTitleImage";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AlbumId", id);
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var titleContent = reader["TitleContent"] as byte[];
                    var titleType = reader["TitleType"] as string;
                    if(titleContent != null && !string.IsNullOrWhiteSpace(titleType))
                    {
                        title = new Image { Content = titleContent, ImageType = titleType };
                    }
                }
                
            }
            return title;
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
                command.CommandText = "Album_GetCountByUser";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();

                count = command.ExecuteScalar() as int?;
            }

            return count ?? 0;
        }

        public List<AppAlbum> GetAllByTagName(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                return null;
            }
            List<AppAlbum> albums = new List<AppAlbum>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Album_GetAllByTagName";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TagName", tagName);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = (int)reader["AlbumId"];
                    string name = reader["AlbumName"] as string;
                    int count = GetAlbumCount(id);
                    string owner = reader["UserName"] as string;
                    var tags = GetTags(id);
                    int? imageId = reader["TitleImageId"] as int?;

                    albums.Add(new AppAlbum { Id = id, Name = name, Count = count, Owner = owner, Tags = tags, TitleImageId = imageId ?? 0 });
                }
            }
            return albums.Count != 0 ? albums : null;
        }
    }
}