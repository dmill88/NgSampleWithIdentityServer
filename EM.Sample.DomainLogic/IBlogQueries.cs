using EM.Sample.DomainLogic.Enums;
using EM.Sample.DomainLogic.Filters;
using EM.Sample.DomainLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EM.Sample.DomainLogic
{
    public interface IBlogQueries
    {
        Task<IEnumerable<BlogListItemDto>> FindBlogs(string name, BlogStatuses? blogStatus = null);
        BlogDto GetBlog(int id);
        BlogDto GetBlog(string name);
        BlogPostDto GetBlogPost(int id);
        Task<IEnumerable<PostListItemDto>> GetAllBlogPostsAsync(int blogId);
        IEnumerable<BlogPostDto> GetBlogPostsAsync(out int totalRecords, BlogPostsFilter filter);
        Task<IEnumerable<BlogListItemDto>> GetBlogs(BlogStatuses blogStatus = BlogStatuses.Published, int? primaryAuthorId = null);
        Task<IEnumerable<BlogListItemDto>> GetBlogs(IEnumerable<string> tags, BlogStatuses blogStatus = BlogStatuses.Published);
        Task<IEnumerable<BlogListItemDto>> GetBlogsByAuthor(int authorId, BlogStatuses blogStatus = BlogStatuses.Published);
        Task<IEnumerable<TagDto>> GetBlogTags(int id);
        Task<PostDto> GetPostAsync(int id);
        IEnumerable<PostStatusDto> GetPostStatuses(bool activeOnly = true);
        Task<IEnumerable<TagDto>> GetPostTags(int id);
        IEnumerable<TagDto> GetTags(bool activeOnly = true);
        Task<IEnumerable<TagDto>> GetUnusedBlogTags(int blogId, bool activeOnly = true);
        Task<IEnumerable<TagDto>> GetUnusedPostTags(int postId, bool activeOnly = true);
    }
}