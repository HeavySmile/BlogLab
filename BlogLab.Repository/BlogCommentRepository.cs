using BlogLab.Models.BlogComment;
using BlogLab.Models.Photo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using BlogLab.Models.Account;

namespace BlogLab.Repository
{
    public class BlogCommentRepository : IBlogCommentRepository
    {
        private readonly IConfiguration _config;

        public BlogCommentRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int blogCommentId)
        {
            int affectedRows = 0;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                affectedRows = await connection.ExecuteAsync("BlogComment_Delete", new { BlogCommentId = blogCommentId }, commandType: CommandType.StoredProcedure);
            }

            return affectedRows;
        }

        public async Task<IList<BlogComment>> GetAllAsync(int blogId)
        {
            IList<BlogComment> blogComments;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                blogComments = (IList<BlogComment>)await connection.QueryAsync<BlogComment>
                (
                    "BlogComment_GetByUserId", 
                    new { BlogId = blogId }, 
                    commandType: CommandType.StoredProcedure
                );
            }

            return blogComments;
        }

        public async Task<BlogComment> GetAsync(int blogCommentId)
        {
            BlogComment blogComment;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                blogComment = await connection.QueryFirstOrDefaultAsync<BlogComment>
                (
                    "BlogComment_Get", 
                    new { BlogCommentId = blogCommentId }, 
                    commandType: CommandType.StoredProcedure
                );
            }

            return blogComment;
        }

        public async Task<BlogComment> UpsertAsync(BlogCommentCreate blogCommentCreate, int applicationUserId)
        {
            var table = new DataTable();
            table.Columns.Add("BlogCommentId", typeof(int));
            table.Columns.Add("ParentBlogCommentId", typeof(int));
            table.Columns.Add("BlogId", typeof(int));
            table.Columns.Add("Content", typeof(string));

            table.Rows.Add(blogCommentCreate.BlogCommentId, blogCommentCreate.ParentBlogCommentId, blogCommentCreate.BlogId, blogCommentCreate.Content);

            BlogComment newBlogComment;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                //ExecuteScalarAsync<int>
                newBlogComment = await GetAsync
                (
                    await connection.ExecuteAsync
                    (
                        "BlogComment_Insert",
                        new { Photo = table.AsTableValuedParameter("dbo.BlogCommentType") },
                        commandType: CommandType.StoredProcedure
                    )
                );
            }

            // move getAsync call here

            return newBlogComment;
        }
    }
}
