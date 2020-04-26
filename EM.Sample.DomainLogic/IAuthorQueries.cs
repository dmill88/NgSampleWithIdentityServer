using EM.Sample.DomainLogic.Filters;
using EM.Sample.DomainLogic.Models;
using System.Collections.Generic;

namespace EM.Sample.DomainLogic
{
    public interface IAuthorQueries
    {
        AuthorDto GetAuthor(int id);
        AuthorDto GetAuthor(string alias);
        IEnumerable<AuthorDto> GetAuthors();
        IEnumerable<AuthorDto> GetAuthors(out int totalRecords, AuthorFilter filter);
    }
}