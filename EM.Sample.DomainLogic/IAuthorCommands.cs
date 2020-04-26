using EM.Sample.DomainLogic.Models;

namespace EM.Sample.DomainLogic
{
    public interface IAuthorCommands
    {
        AuthorDto AddAuthor(string userId, string firstName, string lastName, string alias, string bio, bool active = true);
        void DeactivateAuthor(AuthorDto author);
        AuthorDto UpdateAuthor(AuthorDto author);

        void DeleteAuthor(int id);
    }
}