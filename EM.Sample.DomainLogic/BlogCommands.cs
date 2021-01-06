using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mapster;
using EM.Sample.DataRepository.Context;
using EM.Sample.DataRepository.Models;
using EM.Sample.DomainLogic.Enums;
using EM.Sample.DomainLogic.Models;

namespace EM.Sample.DomainLogic
{
    public class BlogCommands : BaseCQ, IBlogCommands
    {
        public BlogCommands(ContentleverageContext context) : base(context)
        {
        }

        async public Task AddBlogAsync(BlogDto blog)
        {
            Blog blogEntity = blog.Adapt<Blog>();
            blogEntity.GUID = Guid.NewGuid();

            var b = Context.Blog.Add(blogEntity);

            Dictionary<string, Tag> existingTags = Context.Tag.Where(i =>
                blog.Tags.Contains(i.Name)).ToDictionary(i => i.Name.ToLower());

            foreach (string tag in blog.Tags)
            {
                Tag t = existingTags.ContainsKey(tag.ToLower()) ? existingTags[tag.ToLower()] : null;
                if (t == null)
                {
                    t = new Tag() { Name = tag, Active = true, CreatedAt = DateTime.UtcNow };
                }
                Context.BlogTag.Add(new BlogTag() { Blog = b.Entity, Tag = t, CreatedAt = DateTime.UtcNow });
            }

            await Context.SaveChangesAsync();
            blog.Id = b.Entity.Id;
        }

        async public Task UpdateBlogAsync(BlogDto blog)
        {
            try
            {
                int blogId = blog.Id;
                Blog blogEntity = Context.Blog.Find(blog.Id);
                blogEntity.BlogStatusId = blog.BlogStatusId;
                blogEntity.Description = blog.Description;
                blogEntity.DisplayName = blog.DisplayName;
                blogEntity.DisplayOrder = blog.DisplayOrder;
                blogEntity.Name = blog.Name;
                blogEntity.PrimaryAuthorId = blog.PrimaryAuthorId;

                Dictionary<string, Tag> availableTags = Context.Tag.ToDictionary(i => i.Name.ToLower());
                Dictionary<string, Tag> existingBlogTags = Context.BlogTag.Where(i => i.BlogId == blog.Id).Select(i => i.Tag).ToDictionary(i => i.Name.ToLower());
                List<string> blogTagsLowercase = blog.Tags.Select(i => i.ToLower()).ToList();

                // Remove tag assigments not in the blog.Tags list
                List<string> tagsToUnassign = new List<string>();
                foreach (var t in existingBlogTags)
                {
                    if (!blogTagsLowercase.Contains(t.Key))
                    {
                        tagsToUnassign.Add(t.Key);
                    }
                }
                var blogTagsToDelete = Context.BlogTag.Where(i => i.BlogId == blog.Id && tagsToUnassign.Contains(i.Tag.Name)).ToList();
                Context.BlogTag.RemoveRange(blogTagsToDelete);

                foreach (string tag in blog.Tags)
                {
                    Tag t = availableTags.ContainsKey(tag.ToLower()) ? availableTags[tag.ToLower()] : null;
                    if (t == null)
                    {
                        t = new Tag() { Name = tag, Active = true, CreatedAt = DateTime.UtcNow };
                    }
                    if (!existingBlogTags.ContainsKey(tag.ToLower()))
                    {
                        Context.BlogTag.Add(new BlogTag() { Blog = blogEntity, Tag = t, CreatedAt = DateTime.UtcNow });
                    }
                }

                Context.Update(blogEntity);
                await Context.SaveChangesAsync();
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.WriteLine(exp.ToString());
                throw;
            }
        }

        async public Task UpdateBlogTagsAsync(int id, IList<string> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException("tags");
            }

            Blog blog = Context.Blog.Find(id);
            if (blog == null)
            {
                throw new Exception($"Invalid blog Id ${id}");
            }

            Dictionary<string, Tag> existingTags = await Context.Tag.Where(i => tags.Contains(i.Name)).ToDictionaryAsync(i => i.Name.ToLower());
            Dictionary<string, Tag> existingBlogTags = await Context.BlogTag.Where(i => i.BlogId == id).Select(i => i.Tag).ToDictionaryAsync(i => i.Name.ToLower());
            foreach (string tagName in tags)
            {
                Tag t = existingTags.ContainsKey(tagName.ToLower()) ? existingTags[tagName.ToLower()] : null;
                if (t == null)
                {
                    t = new Tag() { Name = tagName, Active = true, CreatedAt = DateTime.UtcNow };
                }

                if (!existingBlogTags.ContainsKey(tagName.ToLower()))
                {
                    Context.BlogTag.Add(new BlogTag() { Blog = blog, Tag = t, CreatedAt = DateTime.UtcNow });
                }
            }

            List<BlogTag> blogTagsToDelete = Context.BlogTag.Where(i => i.BlogId == id && !tags.Contains(i.Tag.Name)).ToList();
            Context.BlogTag.RemoveRange(blogTagsToDelete);

            await Context.SaveChangesAsync();
        }

        async public Task UpdateBlogStatusAsync(int blogId, BlogStatuses blogStatus)
        {
            var blog = Context.Blog.Find(blogId);
            blog.BlogStatusId = (int)blogStatus;
            Context.Update(blog);
            await Context.SaveChangesAsync();
        }

        async public Task DeleteBlogAsync(int id)
        {
            var b = Context.Blog.Find(id);
            if (b == null)
                throw new Exception($"Blog {id} does not exists.");

            var blogTags = Context.BlogTag.Where(i => i.BlogId == id).ToList();
            if (blogTags.Count > 0)
            {
                Context.BlogTag.RemoveRange(blogTags);
            }

            var postIds = Context.BlogPost.Where(i => i.BlogId == id).Select(i => i.PostId).ToList();
            foreach (int postId in postIds)
            {
                await DeletePostAsync(postId, false);
            }

            Context.Blog.Remove(b);
            await Context.SaveChangesAsync();
        }

        async public Task<BlogPostDto> AddBlogPostAsync(BlogPostDto post)
        {
            if (post == null)
            {
                throw new ArgumentNullException("blogPost");
            }

            //Post newPost = post.Adapt<Post>();
            //newPost.CreatedAt = DateTime.UtcNow;
            //newPost.UpdatedAt = DateTime.UtcNow;
            //newPost.PostStatusId = (int)post.PostStatus;
            Post newPost = post.Adapt<Post>();
            newPost.CreatedAt = DateTime.UtcNow;
            newPost.UpdatedAt = DateTime.UtcNow;
            newPost.PostStatus = null;
            //Post znewPost = new Post()
            //{
            //    Title = post.Title,
            //    PostContent = post.PostContent,
            //    Excerpt = post.Excerpt,
            //    PostStatusId = (int)post.PostStatusId,
            //    CommentStatusId = (int)post.CommentStatusId,
            //    CommentCount = 0,
            //    CreatedAt = DateTime.UtcNow,
            //    UpdatedAt = DateTime.UtcNow, 
            //};
            Context.Post.Add(newPost);

            BlogPost newBlogPost = new BlogPost()
            {
                BlogId = post.BlogId,
                Post = newPost
            };
            Context.BlogPost.Add(newBlogPost);

            Dictionary<string, Tag> existingTags = Context.Tag.Where(i =>
                post.Tags.Contains(i.Name)).ToDictionary(i => i.Name.ToLower());

            foreach (string tag in post.Tags)
            {
                Tag t = existingTags.ContainsKey(tag.ToLower()) ? existingTags[tag.ToLower()] : null;
                if (t == null)
                {
                    t = new Tag() { Name = tag, Active = true, CreatedAt = DateTime.UtcNow };
                }
                Context.PostTag.Add(new PostTag() { Post = newPost, Tag = t, CreatedAt = DateTime.UtcNow });
            }

            PostAuthor primaryPostAuthor = new PostAuthor()
            {
                AuthorId = post.PrimaryAuthorId,
                Post = newPost,
                IsPrimary = true,
                ListOrder = 1,
            };
            Context.PostAuthor.Add(primaryPostAuthor);

            await Context.SaveChangesAsync();

            post = newPost.Adapt<BlogPostDto>();
            post.BlogId = newBlogPost.BlogId;
            return post;
        }

        async public Task UpdatePostAsync(PostDto post)
        {
            Post updatedPost = await Context.Post.FindAsync(post.Id);
            if (updatedPost == null)
            {
                throw new ArgumentException($"Invalid Post id ({post.Id}). Unable to find Post entry.");
            }

            updatedPost.Title = post.Title;
            updatedPost.PostContent = post.PostContent;
            updatedPost.Excerpt = post.Excerpt;
            updatedPost.CommentStatusId = (int)post.CommentStatusId;
            updatedPost.PostStatusId = (int)post.PostStatusId;
            updatedPost.UpdatedAt = DateTime.UtcNow;

            Dictionary<string, Tag> availableTags = Context.Tag.ToDictionary(i => i.Name.ToLower());
            Dictionary<string, Tag> existingPostTags = Context.PostTag.Where(i => i.PostId == post.Id).Select(i => i.Tag).ToDictionary(i => i.Name.ToLower());
            List<string> postTagsLowercase = post.Tags.Select(i => i.ToLower()).ToList();

            // Remove tag assigments not in the blogPost.Tags list
            List<string> tagsToUnassign = new List<string>();
            foreach (var t in existingPostTags)
            {
                if (!postTagsLowercase.Contains(t.Key))
                {
                    tagsToUnassign.Add(t.Key);
                }
            }
            var postTagsToDelete = Context.PostTag.Where(i => i.PostId == post.Id && tagsToUnassign.Contains(i.Tag.Name)).ToList();
            Context.PostTag.RemoveRange(postTagsToDelete);

            foreach (string tag in post.Tags)
            {
                Tag t = availableTags.ContainsKey(tag.ToLower()) ? availableTags[tag.ToLower()] : null;
                if (t == null)
                {
                    t = new Tag() { Name = tag, Active = true, CreatedAt = DateTime.UtcNow };
                }
                if (!existingPostTags.ContainsKey(tag.ToLower()))
                {
                    Context.PostTag.Add(new PostTag() { Post = updatedPost, Tag = t, CreatedAt = DateTime.UtcNow });
                }
            }

            await Context.SaveChangesAsync();
        }

        async public Task UpdatePostTagsAsync(int id, IList<string> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException("tags");
            }
            Post updatedPost = await Context.Post.FindAsync(id);
            if (updatedPost == null)
            {
                throw new ArgumentException($"Invalid Post id ({id}). Unable to find Post entry.");
            }

            Dictionary<string, Tag> existingTags = await Context.Tag.Where(i =>
                tags.Contains(i.Name)).ToDictionaryAsync(i => i.Name.ToLower());
            Dictionary<string, Tag> existingPostTags = await Context.PostTag.Where(i =>
                i.PostId == id).Select(i => i.Tag).ToDictionaryAsync(i => i.Name.ToLower());
            List<string> postTagsLowercase = tags.Select(i => i.ToLower()).ToList();

            List<string> tagsToUnassign = new List<string>();
            foreach (var t in existingPostTags)
            {
                if (!postTagsLowercase.Contains(t.Key))
                {
                    tagsToUnassign.Add(t.Key);
                }
            }
            List<PostTag> postTagsToDelete = Context.PostTag.Where(i => i.PostId == id && tagsToUnassign.Contains(i.Tag.Name)).ToList();
            Context.PostTag.RemoveRange(postTagsToDelete);

            foreach (string tag in tags)
            {
                Tag t = existingTags.ContainsKey(tag.ToLower()) ? existingTags[tag.ToLower()] : null;
                if (t == null)
                {
                    t = new Tag() { Name = tag, Active = true, CreatedAt = DateTime.UtcNow };
                }

                if (!existingPostTags.ContainsKey(tag.ToLower()))
                {
                    Context.PostTag.Add(new PostTag() { Post = updatedPost, Tag = t, CreatedAt = DateTime.UtcNow });
                }
            }

            await Context.SaveChangesAsync();
        }

        async public Task UpdatePostStatusAsync(int id, PostStatuses postStatus)
        {
            Post updatedPost = await Context.Post.FindAsync(id);
            if (updatedPost == null)
            {
                throw new ArgumentException($"Invalid Post id ({id}). Unable to find Post entry.");
            }

            updatedPost.PostStatusId = (int)postStatus;
            updatedPost.UpdatedAt = DateTime.UtcNow;

            await Context.SaveChangesAsync();
        }

        async public Task DeletePostAsync(int id)
        {
            await DeletePostAsync(id, true);
        }

        async private Task DeletePostAsync(int id, bool saveChanges)
        {
            Post deletedPost = await Context.Post.FindAsync(id);
            if (deletedPost == null)
            {
                throw new ArgumentException($"Invalid Post id ({id}). Unable to find Post entry.");
            }

            // Remove post tags
            var postTags = Context.PostTag.Where(i => i.PostId == id).ToList();
            Context.PostTag.RemoveRange(postTags);

            // Remove from blogs
            var blogPosts = Context.BlogPost.Where(i => i.PostId == id).ToList();
            Context.BlogPost.RemoveRange(blogPosts);

            // Remove authors
            var postAuthors = Context.PostAuthor.Where(i => i.PostId == id).ToList();
            Context.PostAuthor.RemoveRange(postAuthors);

            // Remove post categories
            var postCategories = Context.PostCategory.Where(i => i.PostId == id).ToList();
            Context.PostCategory.RemoveRange(postCategories);

            // Remove post comments
            var postCommentIds = Context.PostComment.Where(i => i.PostId == id).Select(i => i.Id).ToList();
            foreach (int postCommentId in postCommentIds)
            {
                DeletePostComment(postCommentId);
            }

            Context.Post.Remove(deletedPost);
            if (saveChanges)
            {
                await Context.SaveChangesAsync();
            }
        }

        public async Task<PostCommentDto> AddPostComment(int postId, int authorId, string comment,
            int? parentId = null, int? approvedByAuthorId = null)
        {
            Author commentAuthor = Context.Author.Find(authorId);
            if (commentAuthor == null)
            {
                throw new ArgumentException("Invalid authorId");
            }

            PostComment newPostComment = new PostComment()
            {
                PostId = postId,
                AuthorId = authorId,
                Comment = comment,
                ApprovedByAuthorId = approvedByAuthorId,
                ParentId = parentId,
            };
            Context.PostComment.Add(newPostComment);
            await Context.SaveChangesAsync();

            PostCommentDto dto = newPostComment.Adapt<PostCommentDto>();
            dto.AuthorAlias = commentAuthor.Alias;

            return dto;
        }

        public async Task ApprovePostComment(int id, int approvedByAuthorId)
        {

            var postComment = Context.PostComment.Find(id);
            postComment.ApprovedByAuthorId = approvedByAuthorId;
            await Context.SaveChangesAsync();
        }

        public async Task UpdatePostComment(int id, string comment, int? approvedByAuthorId = null)
        {
            PostComment postComment = Context.PostComment.Find(id);
            if (postComment == null)
                throw new ArgumentException($"Invalid PostComment Id ({id}).", "id");

            postComment.Comment = comment;
            if (approvedByAuthorId.HasValue)
            {
                postComment.ApprovedByAuthorId = approvedByAuthorId;
            }

            await Context.SaveChangesAsync();
        }

        public IList<Tag> GetTags(bool activeOnly = true)
        {
            List<Tag> tags = Context.Tag.Where(i => !activeOnly || !i.Active.HasValue || i.Active.Value).OrderBy(i => i.Name).ToList();
            return tags;
        }

        private void DeletePostComment(int id, bool saveChanges = false)
        {
            var childrenIds = Context.PostComment.Where(i => i.ParentId == id).Select(i => i.Id).ToList();
            foreach (int childId in childrenIds)
            {
                DeletePostComment(childId, saveChanges);
            }

            PostComment pc = Context.PostComment.Find(id);
            if (pc == null)
                throw new ArgumentException($"Invalid PostComment Id ({id}).");

            Context.PostComment.Remove(pc);
            if (saveChanges)
            {
                Context.SaveChanges();
            }
        }

    }
}
