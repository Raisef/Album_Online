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
    public class TagDao : ITagDao
    {
        private string _connectionString;

        public TagDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Create(string tagName)
        {
            if (string.IsNullOrWhiteSpace(tagName))
            {
                return 0;
            }
            int id = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Tag_Create";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TagName", tagName);
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
                command.CommandText = "Tag_Delete";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TagId", id);
                connection.Open();

                result = command.ExecuteNonQuery();
            }
            return result > 0 ? true : false;
        }

        public Tag Get(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            Tag tag = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Tag_Get";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TagId", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tagName = reader["TagName"] as string;
                    tag = new Tag { Id = id, Name = tagName};
                }
            }
            return tag;
        }

        public int GetAlbumTagCount(int id)
        {
            if(id <= 0)
            {
                return -1;
            }
            int? count = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Tag_GetAlbumTagCount";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TagId", id);
                connection.Open();

                count = command.ExecuteScalar() as int?;
            }

            return count ?? 0;
        }

        public int GetImageTagCount(int id)
        {
            if (id <= 0)
            {
                return -1;
            }
            int? count = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Tag_GetImageTagCount";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TagId", id);
                connection.Open();

                count = command.ExecuteScalar() as int?;
            }

            return count ?? 0;
        }

        public List<Tag> GetAll()
        {
            List<Tag> tags = new List<Tag>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "Tag_GetAll";
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var tagId = (int)reader["Id"];
                    var tagName = reader["Name"] as string;
                    tags.Add(new Tag { Id = tagId, Name = tagName });
                }
            }
            return tags.Count > 0 ? tags : null;
        }
    }
}
