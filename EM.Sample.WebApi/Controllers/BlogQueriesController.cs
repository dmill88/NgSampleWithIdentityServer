using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using EM.Sample.DomainLogic;
using EM.Sample.DomainLogic.Models;
using EM.Sample.DomainLogic.Enums;
using EM.Sample.DomainLogic.Filters;
using EM.Data.Helpers;
using System.Diagnostics;

namespace EM.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BlogQueriesController : ControllerBase
    {
        private readonly IBlogQueries _blogQueries = null;
        private readonly ILogger<BlogPostsController> _logger;

        public BlogQueriesController(IBlogQueries blogQueries, ILogger<BlogPostsController> logger)
        {
            _blogQueries = blogQueries;
            _logger = logger;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBlogs([FromQuery] BlogStatuses status = BlogStatuses.Published, [FromQuery] int? primaryAuthorId = null)
        {
            //var claims = User.Claims.Select(i => new { i.Type, i.Value }).ToList();
            //claims.ForEach(c => Debug.WriteLine($"{c.Type}: {c.Value}"));
            IEnumerable<BlogListItemDto> blogs = await _blogQueries.GetBlogs(status, primaryAuthorId);
            return Ok(blogs);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetBlog(int id)
        {
            BlogDto blog = _blogQueries.GetBlog(id);
            return Ok(blog);
        }


        [HttpGet("[action]")]
        public IActionResult GetTags()
        {
            IEnumerable<TagDto> tags = _blogQueries.GetTags();
            return Ok(tags);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUnusedBlogTags(int id)
        {
            IEnumerable<TagDto> tags = await _blogQueries.GetUnusedBlogTags(id);
            return Ok(tags);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetBlogTags(int id)
        {
            IEnumerable<TagDto> tags = await _blogQueries.GetBlogTags(id);
            return Ok(tags);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetUnusedPostTags(int id)
        {
            IEnumerable<TagDto> tags = await _blogQueries.GetUnusedPostTags(id);
            return Ok(tags);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetPostTags(int id)
        {
            IEnumerable<TagDto> tags = await _blogQueries.GetPostTags(id);
            return Ok(tags);
        }


        [HttpGet("[action]")]
        public IActionResult GetPostStatuses(bool activeOnly = true)
        {
            IEnumerable<PostStatusDto> statuses = _blogQueries.GetPostStatuses(activeOnly);
            return Ok(statuses);
        }

        [HttpGet("[action]/{blogId}")]
        public async Task<IActionResult> GetAllBlogPostsAsync([FromQuery] int blogId)
        {
            IEnumerable<PostListItemDto> posts = await _blogQueries.GetAllBlogPostsAsync(blogId);
            return Ok(posts);
        }

        [HttpPost("[action]")]
        public IActionResult GetBlogPosts(BlogPostsFilter filter)
        {
            IEnumerable<PostDto> posts = _blogQueries.GetBlogPostsAsync(out int numberOfRecords, filter);
            PagedDataResult result = new PagedDataResult(posts, numberOfRecords);
            return Ok(result);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetBlogPost(int id)
        {
            PostDto blogPost = await _blogQueries.GetBlogPost(id);
            return Ok(blogPost);
        }


    }
}