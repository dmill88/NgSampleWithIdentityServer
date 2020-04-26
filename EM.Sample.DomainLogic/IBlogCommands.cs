using EM.Sample.DataRepository.Models;
using EM.Sample.DomainLogic.Enums;
using EM.Sample.DomainLogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EM.Sample.DomainLogic
{
    public interface IBlogCommands
    {
        Task AddBlogAsync(BlogDto blog);
        Task<BlogPostDto> AddBlogPostAsync(BlogPostDto post);
        Task<PostCommentDto> AddPostComment(int postId, int authorId, string comment, int? parentId = null, int? approvedByAuthorId = null);
        Task ApprovePostComment(int id, int approvedByAuthorId);
        Task DeleteBlogAsync(int id);
        Task DeletePostAsync(int id);
        IList<Tag> GetTags(bool activeOnly = true);
        Task UpdateBlogAsync(BlogDto blog);
        Task UpdateBlogStatusAsync(int blogId, BlogStatuses blogStatus);
        Task UpdateBlogTagsAsync(int id, IList<string> tags);
        Task UpdatePostAsync(PostDto post);
        Task UpdatePostComment(int id, string comment, int? approvedByAuthorId = null);
        Task UpdatePostStatusAsync(int id, PostStatuses postStatus);
        Task UpdatePostTagsAsync(int id, IList<string> tags);
    }
}