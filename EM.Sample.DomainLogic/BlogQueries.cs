using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mapster;
using EM.Data.Helpers;
using EM.Sample.DataRepository.Context;
using EM.Sample.DataRepository.Models;
using EM.Sample.DomainLogic.Enums;
using EM.Sample.DomainLogic.Filters;
using EM.Sample.DomainLogic.Models;

namespace EM.Sample.DomainLogic
{
    public class BlogQueries : BaseCQ, IBlogQueries
    {
        public BlogQueries(ContentleverageContext context) :
            base(context)
        {
        }

        public BlogDto GetBlog(int id)
        {
            BlogDto blog = null;
            if (id > 0)
            {
                Blog blogEntity = null;
                blogEntity = Context.Blog
                    .Include(i => i.PrimaryAuthor)
                    .Include(i => i.BlogStatus)
                    .Include(i => i.BlogTag).ThenInclude(i => i.Tag) // Intellisense doesn't work correctly for ThenInclude, but it works
                    .SingleOrDefault(i => i.Id == id);
                //blog = blogEntity.Adapt<BlogDto>();
                //blog.PrimaryAuthor = blogEntity.PrimaryAuthor.Adapt<AuthorDto>();
                //blog.Tags.AddRange(blogEntity.BlogTag.Select(i => i.Tag.Name));
                blog = ConvertBlogEntityToDto(blogEntity);
            }
            else
            {
                blog = new BlogDto();
                blog.BlogStatusId = (int)BlogStatuses.Draft;
                blog.DisplayOrder = 1;
            }

            return blog;
        }

        public BlogDto GetBlog(string name)
        {
            var blogEntity = (from blogs in Context.Blog
                              join blogTags in Context.BlogTag on blogs.Id equals blogTags.BlogId into dbt
                              from blogTags in dbt.DefaultIfEmpty()
                              join t in Context.Tag on blogTags.TagId equals t.Id into dt
                              from t in dt.DefaultIfEmpty()
                              where blogs.Name == name
                              select blogs).FirstOrDefault();
            BlogDto blog = ConvertBlogEntityToDto(blogEntity);
            return blog;
        }

        private BlogDto ConvertBlogEntityToDto(Blog entity)
        {
            if (entity == null)
            {
                return null;
            }
            BlogDto blogDto = entity.Adapt<BlogDto>();
            blogDto.Tags.AddRange(entity.BlogTag.Select(i => i.Tag.Name));
            return blogDto;
        }

        public async Task<IEnumerable<BlogListItemDto>> GetBlogs(BlogStatuses blogStatus = BlogStatuses.Published, int? primaryAuthorId = null)
        {
            int blogStatusId = (int)blogStatus;

            List<BlogDto> blogs = await Context.Blog.Include(i => i.PrimaryAuthor)
                .Where(i => i.BlogStatusId == blogStatusId
                    && (primaryAuthorId == null || i.PrimaryAuthorId == primaryAuthorId.Value))
                .OrderBy(i => i.DisplayOrder)
                .ThenBy(i => i.DisplayName)
                .ProjectToType<BlogDto>().ToListAsync();

            List<BlogListItemDto> blogsList = await Context.Blog
                .Where(i => i.BlogStatusId == blogStatusId
                    && (primaryAuthorId == null || i.PrimaryAuthorId == primaryAuthorId.Value))
                .OrderBy(i => i.DisplayOrder)
                .ThenBy(i => i.DisplayName)
                .ProjectToType<BlogListItemDto>().ToListAsync();

            return blogsList;
        }

        public async Task<IEnumerable<BlogListItemDto>> GetBlogs(IEnumerable<string> tags, BlogStatuses blogStatus = BlogStatuses.Published)
        {
            int blogStatusId = (int)blogStatus;

            var q = from blogs in Context.Blog
                    join blogTags in Context.BlogTag on blogs.Id equals blogTags.BlogId into dbt
                    from blogTags in dbt.DefaultIfEmpty()
                    join t in Context.Tag on blogTags.TagId equals t.Id into dt
                    from t in dt.DefaultIfEmpty()
                    join a in Context.Author on blogs.PrimaryAuthorId equals a.Id
                    where tags.Contains(t.Name)
                        && (blogStatusId == 0 || blogs.BlogStatusId == blogStatusId)
                    orderby blogs.DisplayOrder
                    orderby blogs.DisplayName
                    select blogs;
            List<BlogListItemDto> list = await q.ProjectToType<BlogListItemDto>().ToListAsync();
            return list;
        }

        public async Task<IEnumerable<BlogListItemDto>> GetBlogsByAuthor(int authorId, BlogStatuses blogStatus = BlogStatuses.Published)
        {
            int blogStatusId = (int)blogStatus;

            var q = from blogs in Context.Blog
                    where blogs.PrimaryAuthorId == authorId
                        && (blogStatusId == 0 || blogs.BlogStatusId == blogStatusId)
                    orderby blogs.DisplayOrder
                    orderby blogs.DisplayName
                    select blogs;
            List<BlogListItemDto> list = await q.Distinct().ProjectToType<BlogListItemDto>().ToListAsync();
            return list;
        }

        async public Task<IEnumerable<BlogListItemDto>> FindBlogs(string name, BlogStatuses? blogStatus = null)
        {
            if (string.IsNullOrWhiteSpace(name)) name = string.Empty;
            int blogStatusId = blogStatus.HasValue ? (int)blogStatus.Value : 0;

            List<BlogListItemDto> blogsList = await Context.Blog.Where(b => (b.Name.Contains(name) || b.DisplayName.Contains(name))
                && (blogStatusId == 0 || b.BlogStatusId == blogStatusId))
                .OrderBy(i => i.DisplayOrder)
                .ThenBy(i => i.DisplayName)
                .Select(i => new BlogListItemDto() { Id = i.Id, Name = i.DisplayName }).ToListAsync();

            return blogsList;
        }

        async public Task<IEnumerable<TagDto>> GetBlogTags(int id)
        {
            List<TagDto> tags = await Context.BlogTag.Include(i => i.Tag)
                .Where(i => i.BlogId == id && (!i.Tag.Active.HasValue || i.Tag.Active.Value))
                .Select(i => i.Tag).OrderBy(i => i.Name).ProjectToType<TagDto>().ToListAsync();
            return tags;
        }

        async public Task<IEnumerable<TagDto>> GetUnusedBlogTags(int blogId, bool activeOnly = true)
        {
            var usedTagIds = Context.BlogTag.Where(i => i.BlogId == blogId).Select(i => i.TagId).ToList();

            List<TagDto> tags = await Context.Tag
                .Where(i => !usedTagIds.Contains(i.Id) && (!activeOnly || !i.Active.HasValue || i.Active.Value))
                .OrderBy(i => i.Name)
                .ProjectToType<TagDto>().ToListAsync();
            return tags;
        }

        async public Task<IEnumerable<TagDto>> GetPostTags(int id)
        {
            List<TagDto> tags = await Context.PostTag.Include(i => i.Tag)
                .Where(i => i.PostId == id && (!i.Tag.Active.HasValue || i.Tag.Active.Value))
                .Select(i => i.Tag)
                .OrderBy(i => i.Name)
                .ProjectToType<TagDto>()
                .ToListAsync();
            return tags;
        }

        async public Task<IEnumerable<TagDto>> GetUnusedPostTags(int postId, bool activeOnly = true)
        {
            List<int> usedTagIds = Context.PostTag.Where(i => i.PostId == postId).Select(i => i.TagId).ToList();

            List<TagDto> tags = await Context.Tag
                .Where(i => !usedTagIds.Contains(i.Id) && (!activeOnly || !i.Active.HasValue || i.Active.Value))
                .OrderBy(i => i.Name)
                .ProjectToType<TagDto>()
                .ToListAsync();
            return tags;
        }

        public IEnumerable<PostStatusDto> GetPostStatuses(bool activeOnly = true)
        {
            List<PostStatusDto> statues = Context.PostStatus
                .Where(i => !activeOnly || i.Active)
                .OrderBy(i => i.DisplayOrder)
                .ThenBy(i => i.DisplayName)
                .ProjectToType<PostStatusDto>()
                .ToList();
            return statues;
        }

        async public Task<IEnumerable<PostListItemDto>> GetAllBlogPostsAsync(int blogId)
        {
            var blogPosts = await Context.BlogPost.Include(i => i.Blog)
                .Where(i => i.BlogId == blogId)
                .Select(i => i.Post)
                .OrderByDescending(i => i.UpdatedAt)
                .ProjectToType<PostListItemDto>()
                //.Select(i => new PostListItemDto() { 
                //    Id = i.Id,
                //    CommentCount = i.CommentCount, 
                //    CommentStatusId = i.CommentStatusId, 
                //    Excerpt = i.Excerpt, 
                //    PostStatusId = i.PostStatusId, 
                //    Title = i.Title, 
                //    UpdatedAt = i.UpdatedAt
                //})
                .ToListAsync();
            return blogPosts;
        }

        public IEnumerable<BlogPostDto> GetBlogPostsAsync(out int totalRecords, BlogPostsFilter filter)
        {
            totalRecords = 0;
            if (filter.BlogId < 1)
                throw new ArgumentOutOfRangeException("filter.BlogId", "The filter.BlogId is invalid.");
            if (filter.Skip < 0)
                throw new ArgumentOutOfRangeException("filter.Skip", "The filter.Skip number must be 0 or greater.");
            if (filter.Take < 1)
                throw new ArgumentOutOfRangeException("filter.Take", "The filter.Take must be greater than zero.");

            var entities = Context.BlogPost
                .Where(i => i.BlogId == filter.BlogId)
                .Select(i => i.Post);

            if (!string.IsNullOrEmpty(filter.Title))
            {
                entities = entities.Where(i => EF.Functions.Like(i.Title, $"%{filter.Title}%"));
            }

            totalRecords = entities.Count();
            if (totalRecords <= filter.Skip)
            {
                filter.Skip = 0;
            }

            entities = entities.ApplySortToEntities(filter.SortMembers);

            List<BlogPostDto> blogPosts = entities.Skip(filter.Skip).Take(filter.Take)
                .ProjectToType<BlogPostDto>().ToList();

            blogPosts.ForEach(i => {
                i.BlogId = filter.BlogId;
            });

            return blogPosts;
        }

        public async Task<PostDto> GetPostAsync(int id)
        {
            var postEntity = await Context.Post
                .Include(i => i.PostAuthor)
                .Include(i => i.PostTag).ThenInclude(i => i.Tag)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (postEntity == null)
            {
                throw new ArgumentException("Invalid post id");
            }

            PostDto post = postEntity.Adapt<PostDto>();
            post.Tags.AddRange(postEntity.PostTag.Select(i => i.Tag.Name).Distinct().ToList());
            var pa = postEntity.PostAuthor.FirstOrDefault(i => i.PostId == id && i.IsPrimary);
            if (pa != null)
            {
                post.PrimaryAuthorId = pa.AuthorId;
            }

            return post;
        }

        public async Task<BlogPostDto> GetBlogPost(int id)
        {
            var entity = await Context.BlogPost
                .Include(i => i.Blog)
                .Include(i => i.Post).ThenInclude(i => i.PostTag).ThenInclude(i => i.Tag)
                .Include(i => i.Post).ThenInclude(i => i.PostAuthor)
                .Where(i => i.PostId == id).FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new ArgumentException("Invalid post id");
            }

            BlogPostDto blogPost = new BlogPostDto()
            {
                Id = entity.Post.Id,
                GUID = entity.Post.GUID,
                BlogDisplayName = entity.Blog.DisplayName,
                BlogId = entity.BlogId,
                CommentCount = entity.Post.CommentCount,
                CommentStatusId = entity.Post.CommentStatusId,
                Excerpt = entity.Post.Excerpt,
                PostContent = entity.Post.PostContent,
                PostStatusId = entity.Post.PostStatusId,
                Title = entity.Post.Title,
                UpdatedAt = entity.Post.UpdatedAt
            };
            blogPost.Tags.AddRange(entity.Post.PostTag.Select(i => i.Tag.Name).ToList());

            var pa = entity.Post.PostAuthor.FirstOrDefault(i => i.PostId == id && i.IsPrimary);
            if (pa != null)
            {
                blogPost.PrimaryAuthorId = pa.AuthorId;
            }
            return blogPost;
        }

        public IEnumerable<TagDto> GetTags(bool activeOnly = true)
        {
            List<TagDto> tags = Context.Tag
                .Where(i => !activeOnly || !i.Active.HasValue || i.Active.Value)
                .OrderBy(i => i.Name)
                .ProjectToType<TagDto>().ToList();
            return tags;
        }


    }
}
