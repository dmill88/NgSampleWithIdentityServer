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
using EM.Sample.DomainLogic.Filters;
using EM.Data.Helpers;

namespace EM.Sample.DomainLogic
{
    public class AuthorQueries : BaseCQ, IAuthorQueries
    {
        public AuthorQueries(ContentleverageContext context) : base(context)
        {
        }

        public IEnumerable<AuthorDto> GetAuthors()
        {
            List<AuthorDto> authors = Context.Author
                .OrderBy(i => i.LastName).ThenBy(i => i.FirstName)
                .ProjectToType<AuthorDto>().ToList();
            return authors;
        }

        public IEnumerable<AuthorDto> GetAuthors(out int totalRecords, AuthorFilter filter)
        {
            totalRecords = 0;
            var entities = Context.Author.Select(i => i);

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                entities = entities.Where(i => EF.Functions.Like(i.FirstName, $"{filter.FirstName}%"));
            }
            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                entities = entities.Where(i => EF.Functions.Like(i.LastName, $"{filter.LastName}%"));
            }
            if (!string.IsNullOrWhiteSpace(filter.Alias))
            {
                entities = entities.Where(i => EF.Functions.Like(i.Alias, $"{filter.Alias}%"));
            }

            totalRecords = entities.Count();
            if (totalRecords <= filter.Skip)
            {
                filter.Skip = 0;
            }
            entities = entities.ApplySortToEntities(filter.SortMembers);

            List<AuthorDto> authors = entities
                .Skip(filter.Skip)
                .Take(filter.Take)
                .ProjectToType<AuthorDto>().ToList();
            return authors;
        }

        public AuthorDto GetAuthor(int id)
        {
            Author authorEntity = Context.Author.Find(id);
            if (authorEntity == null)
            {
                throw new Exception($"Invalid author Id ({id}).");
            }
            AuthorDto author = authorEntity.Adapt<AuthorDto>();
            return author;
        }

        public AuthorDto GetAuthor(string alias)
        {
            Author authorEntity = Context.Author.FirstOrDefault(i => i.Alias == alias);
            AuthorDto author = null;
            if (authorEntity != null)
            {
                author = authorEntity.Adapt<AuthorDto>();
            }
            return author;
        }

    }
}
