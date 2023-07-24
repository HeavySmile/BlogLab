using BlogLab.Models.Account;
using BlogLab.Models.Photo;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IConfiguration _config;
        
        public PhotoRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int photoId)
        {
            int affectedRows = 0;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                affectedRows = await connection.ExecuteAsync("Photo_Delete", new { PhotoId = photoId }, commandType: CommandType.StoredProcedure);
            }

            return affectedRows;
        }

        public async Task<IList<Photo>> GetAllByUserIdAsync(int applicationUserId)
        {
            IList<Photo> photos;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                photos = (IList<Photo>)await connection.QueryAsync<Photo>("Photo_GetByUserId", new { ApplicationUserId = applicationUserId }, commandType: CommandType.StoredProcedure);
            }

            return photos;
        }

        public async Task<Photo> GetAsync(int photoId)
        {
            Photo photo;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                photo = await connection.QueryFirstOrDefaultAsync<Photo>("Photo_Get", new { PhotoId = photoId }, commandType: CommandType.StoredProcedure);
            }

            return photo;
        }

        public async Task<Photo> InsertAsync(PhotoCreate photoCreate, int applicationUserId)
        {
            var table = new DataTable();
            table.Columns.Add("PublicId", typeof(string));
            table.Columns.Add("ImageUrl", typeof(string));
            table.Columns.Add("Description", typeof(string));

            table.Rows.Add(photoCreate.PublicId, photoCreate.ImageUrl, photoCreate.Description);

            Photo newPhoto;
            
            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                //ExecuteScalarAsync<int>
                newPhoto = await GetAsync
                (
                    await connection.ExecuteAsync("Photo_Insert", 
                    new { Photo = table.AsTableValuedParameter("dbo.PhotoType") }, 
                    commandType: CommandType.StoredProcedure)
                );
            }

            // move getAsync photo call here

            return newPhoto;
        }
    }
}
