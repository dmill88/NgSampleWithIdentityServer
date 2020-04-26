using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EM.Sample.DataRepository.Context;
using EM.Sample.DomainLogic.Enums;
using EM.Sample.DomainLogic.Models;
using EM.Sample.DomainLogic.Filters;

namespace EM.Sample.DomainLogic.Tests
{
    [TestClass]
    public class BlogCRUDTests
    {
        private BlogCommands _blogCmds = null;
        private BlogQueries _blogRqs = null;

        [TestInitialize]
        public void Init()
        {
            ContentleverageContext dbRepository = new ContentleverageContext();
            _blogCmds = new BlogCommands(dbRepository);
            _blogRqs = new BlogQueries(dbRepository);
        }


        [TestMethod]
        public void BlogCRUD()
        {
            Task.Run(async () =>
            {
                //await _blogMng.DeleteBlogAsync(2010);

                string blogName01 = "testBlog01";
                int primaryAuthorId = 1;
                List<string> tags = new List<string>() { "Programming", "Art" };
                BlogDto blogDto = new BlogDto()
                {
                    Name = "myTestBlog",
                    DisplayName = "My Test Blog",
                    Description = "My test blog description",
                    DisplayOrder = 1,
                    BlogStatusId = (int)BlogStatuses.Draft,
                    PrimaryAuthorId = primaryAuthorId
                };
                blogDto.Tags.AddRange(tags);

                await _blogCmds.AddBlogAsync(blogDto);

                BlogDto blogToUpdate = _blogRqs.GetBlog(blogDto.Id);
                blogToUpdate.Name = blogName01;

                blogDto.Name = blogName01;
                await _blogCmds.UpdateBlogAsync(blogDto);

                blogToUpdate = _blogRqs.GetBlog(blogToUpdate.Id);
                Assert.AreEqual(blogToUpdate.Name, blogName01);
                blogToUpdate = _blogRqs.GetBlog(blogName01);
                Assert.AreEqual(blogToUpdate.Name, blogName01);

                var blogs = await _blogRqs.GetBlogs(tags, BlogStatuses.Draft);
                Assert.IsTrue(blogs.Count() > 0);

                blogs = await _blogRqs.GetBlogsByAuthor(primaryAuthorId, BlogStatuses.Draft);
                Assert.IsTrue(blogs.Count() > 0);

                await _blogCmds.UpdateBlogStatusAsync(blogToUpdate.Id, BlogStatuses.Published);
                blogs = await _blogRqs.GetBlogsByAuthor(primaryAuthorId, BlogStatuses.Published);
                Assert.IsTrue(blogs.Count() > 0);

                blogs = await _blogRqs.GetBlogsByAuthor(primaryAuthorId);
                Assert.IsTrue(blogs.Count() > 0);

                blogs = await _blogRqs.FindBlogs("TestBlog");
                Assert.IsTrue(blogs.Count() > 0);
                blogs = await _blogRqs.FindBlogs("TestBlog", BlogStatuses.Published);
                Assert.IsTrue(blogs.Count() > 0);

                await _blogCmds.DeleteBlogAsync(blogToUpdate.Id);
                blogToUpdate = _blogRqs.GetBlog(blogToUpdate.Id);
                Assert.IsNull(blogToUpdate);
            }).GetAwaiter().GetResult();

        }

        [TestMethod]
        public void BlogPostCRUD()
        {
            Task.Run(async () =>
            {
                int primaryAuthorId = 1;
                string blogName01 = "testBlog01";

                var blogThatShouldNotBe = _blogRqs.GetBlog(blogName01);
                if (blogThatShouldNotBe != null)
                {
                    await _blogCmds.DeleteBlogAsync(blogThatShouldNotBe.Id);
                }

                List<string> tags = new List<string>() { "Programming", "Art" };
                BlogDto blogDto = new BlogDto() 
                { 
                    Name = blogName01, 
                    DisplayName = "My Test Blog", 
                    BlogStatusId = (int)BlogStatuses.Draft, 
                    PrimaryAuthorId = 1 
                };
                blogDto.Tags.AddRange(tags);
                await _blogCmds.AddBlogAsync(blogDto);

                List<string> postTags = new List<string>() { "PostTag01", "PostTag02", "PostTag03", "PostTag04", "PostTag05" };
                BlogPostDto blogPost = new BlogPostDto()
                {
                    BlogId = blogDto.Id,
                    CommentStatusId = (int)CommentStatuses.MemberOnly,
                    PrimaryAuthorId = primaryAuthorId,
                    PostStatusId = (int)PostStatuses.Draft,
                    Title = "My Test Blog01 Post Title",
                    PostContent = "Here's my first blog post content.",
                    Excerpt = "Test excerpt content.",
                };
                blogPost.Tags.AddRange(postTags);
                BlogPostDto testPost01 = await _blogCmds.AddBlogPostAsync(blogPost);
                Assert.IsTrue(testPost01.Id > 0);

                testPost01.PostContent = "Here's my first blog post with updated content.";
                testPost01.PostStatusId = (int)PostStatuses.Draft;
                testPost01.Excerpt = "Updated test excerpt";

                PostCommentDto firstComment = await _blogCmds.AddPostComment(testPost01.Id, primaryAuthorId, "A wanted to add a few test comments.");
                PostCommentDto secondsComment = await _blogCmds.AddPostComment(testPost01.Id, primaryAuthorId, "This is a second test comment.");
                PostCommentDto subComment = await _blogCmds.AddPostComment(testPost01.Id, primaryAuthorId, "This is a sub comment for the second test comment.", secondsComment.Id);

                BlogPostDto testPost02 = await _blogCmds.AddBlogPostAsync(new BlogPostDto()
                {
                    BlogId = blogDto.Id,
                    PrimaryAuthorId = primaryAuthorId,
                    PostStatusId = (int)PostStatuses.Draft,
                    Title = "My Test Blog01 Second Post Title",
                    PostContent = "Here's my second blog post content.",
                    Excerpt = "Second test excerpt content.",
                });
                testPost02.Tags.AddRange(postTags);
                Assert.IsTrue(testPost01.Id > 0);

                //await _blogMng.ApprovePostComment()

                var blogPostListItem = await _blogRqs.GetAllBlogPostsAsync(blogDto.Id);
                Assert.IsTrue(blogPostListItem.Count() > 1);
                int numberOfPosts = 0;
                BlogPostsFilter blogFilter = new BlogPostsFilter() { BlogId = blogDto.Id, Skip = 0, Take = 1 };
                blogFilter.SortMembers.Add(new Data.Helpers.SortDescriptor("UpdatedAt", System.ComponentModel.ListSortDirection.Ascending));
                var blogPosts = _blogRqs.GetBlogPostsAsync(out numberOfPosts, blogFilter);
                Assert.IsTrue(blogPosts.Count() == 1);

                var myTestPost = await _blogRqs.GetPostAsync(testPost01.Id);

                await _blogCmds.DeletePostAsync(testPost01.Id);
                await _blogCmds.DeletePostAsync(testPost02.Id);

                await _blogCmds.DeleteBlogAsync(blogDto.Id);
                BlogDto blog = _blogRqs.GetBlog(blogDto.Id);
                Assert.IsNull(blog);
            }).GetAwaiter().GetResult();

        }

        [TestCleanup()]
        public void Cleanup()
        {
            if (_blogCmds != null) _blogCmds.Dispose();
            if (_blogRqs != null) _blogRqs.Dispose();
        }

    }
}
