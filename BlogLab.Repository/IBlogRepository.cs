﻿using BlogLab.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLab.Repository
{
    public interface IBlogRepository
    {
        public Task<Blog> UpsertAsync(BlogCreate blogCreate, int applicationUserId);
        public Task<PagedResults<Blog>> GetAllAsync(BlogPaging blogPaging);
        public Task<Blog> GetAsync(int blogId);
        public Task<IList<Blog>> GetAllByUserIdAsync(int applicationUserId);
        public Task<IList<Blog>> GetAllFamousAsync();
        public Task<int> DeleteAsync(int blogId);
    }
}