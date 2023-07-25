using BlogLab.Models.Blog;
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
using System.Reflection.Metadata;

namespace BlogLab.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private readonly IConfiguration _config;

        public BlogRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<int> DeleteAsync(int blogId)
        {
            int affectedRows = 0;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                affectedRows = await connection.ExecuteAsync("Blog_Delete", new { BlogId = blogId }, commandType: CommandType.StoredProcedure);
            }

            return affectedRows;
        }

        public async Task<PagedResults<Blog>> GetAllAsync(BlogPaging blogPaging)
        {
            var results = new PagedResults<Blog>();

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();

                var multiple = await connection.QueryMultipleAsync("Blog_All",
                    new
                    {
                        Offset = (blogPaging.Page - 1) * (blogPaging.PageSize),
                        PageSize = blogPaging.PageSize
                    }, commandType: CommandType.StoredProcedure);

                results.Items = multiple.Read<Blog>();
                results.TotalCount = multiple.ReadFirst<int>();
            }

            return results;
        }

        public async Task<IList<Blog>> GetAllByUserIdAsync(int applicationUserId)
        {   
            IList<Blog> blogs;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                blogs = (IList<Blog>)await connection.QueryAsync<Blog>("Blog_GetByUserId", new { ApplicationUserId = applicationUserId }, commandType: CommandType.StoredProcedure);
            }

            return blogs;
        }

        public async Task<IList<Blog>> GetAllFamousAsync()
        {
            IList<Blog> famousBlogs;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                famousBlogs = (IList<Blog>)await connection.QueryAsync<Blog>("Blog_GetAllFamous", new { }, commandType: CommandType.StoredProcedure);
            }

            return famousBlogs;

        }

        public async Task<Blog> GetAsync(int blogId)
        {
            Blog blog;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                blog = await connection.QueryFirstOrDefaultAsync<Blog>("Blog_Get", new { BlogId = blogId }, commandType: CommandType.StoredProcedure);
            }

            return blog;
        }

        public async Task<Blog> UpsertAsync(BlogCreate blogCreate, int applicationUserId)
        {
            var table = new DataTable();
            table.Columns.Add("BlogId", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Content", typeof(string));
            table.Columns.Add("PhotoId", typeof(int));

            table.Rows.Add(blogCreate.BlogId, blogCreate.Title, blogCreate.Content, blogCreate.PhotoId);

            int? newblogId;

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                // ExecuteScalarAsync<int?> ??
                newblogId = await connection.ExecuteAsync
                (
                    "Blog_Upsert", 
                    new { Blog = table.AsTableValuedParameter("dbo.BlogType"), ApplicationUserId = applicationUserId }, 
                    commandType: CommandType.StoredProcedure
                );
            }

            newblogId = newblogId ?? blogCreate.BlogId;
            Blog blog = await GetAsync(newblogId.Value);

            return blog;
        }
    }
}
