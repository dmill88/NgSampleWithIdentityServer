using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using EM.Sample.DomainLogic;
using EM.Sample.DomainLogic.Models;
using EM.Sample.DomainLogic.Enums;
using EM.Sample.DomainLogic.Filters;
using EM.Data.Helpers;
using System.Net.Mime;

namespace EM.Sample.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "BlogEditor")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogQueries _blogQueries = null;
        private readonly IBlogCommands _blogCommands = null;
        private readonly ILogger<BlogPostsController> _logger;

        public BlogController(IBlogQueries blogQueries, IBlogCommands blogCommands, ILogger<BlogPostsController> logger)
        {
            _blogQueries = blogQueries;
            _blogCommands = blogCommands;
            _logger = logger;
        }

        [HttpPost("[action]")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBlog([FromBody] BlogDto blog)
        {
            //int n = 4;
            //if (n == 14)
            //{
            //    throw new Exception("Test exception", new Exception("Inner exception for testing"));
            //}
            if (blog == null)
            {
                return BadRequest("Blog object is null.");
            }
            if (blog.Id != 0)
            {
                return BadRequest("Blog Id is not zero.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                await _blogCommands.AddBlogAsync(blog);
            }
            return CreatedAtAction(nameof(AddBlog), blog);
        }

        [HttpPut("[action]")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBlog([FromBody] BlogDto blog)
        {
            if (blog == null)
            {
                return BadRequest("Blog object is null.");
            }
            if (blog.Id == 0)
            {
                return BadRequest("Blog Id is zero.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                await _blogCommands.UpdateBlogAsync(blog);
            }
            return Ok(blog);
        }

        [HttpDelete("[action]/{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBlog([FromRoute] int id)
        {
            if (id == 0)
            {
                return BadRequest("Blog Id is zero.");
            }
            await _blogCommands.DeleteBlogAsync(id);
            return Ok();
        }

    }
}