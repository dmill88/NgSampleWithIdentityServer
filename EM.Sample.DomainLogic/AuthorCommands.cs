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
    public class AuthorCommands : BaseCQ, IAuthorCommands
    {
        public AuthorCommands(ContentleverageContext context) : base(context)
        {
        }

        public AuthorDto AddAuthor(string userId, string alias, string firstName, string lastName, string bio, bool active = true)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("userId");
            }
            if (string.IsNullOrWhiteSpace(alias))
            {
                throw new ArgumentNullException("alias");
            }

            Author entity = new Author() { UserId = userId, Alias = alias, FirstName = firstName, LastName = lastName, Bio = bio, Active = active };
            Context.Author.Add(entity);
            Context.SaveChanges();

            AuthorDto author = entity.Adapt<AuthorDto>();
            return author;
        }

        public AuthorDto UpdateAuthor(AuthorDto author)
        {
            Author authorEntity = Context.Author.Find(author.Id);

            if (authorEntity == null)
            {
                authorEntity = Context.Author.FirstOrDefault(i => i.UserId == author.UserId);
            }

            if (authorEntity == null)
            {
                throw new Exception($"Invalid author Id ({author.Id}) and UserId ({author.UserId}) .");
            }

            authorEntity.UserId = author.UserId;
            authorEntity.LastName = author.LastName;
            authorEntity.FirstName = author.FirstName;
            authorEntity.Alias = author.Alias;
            authorEntity.Bio = author.Bio;
            authorEntity.Active = author.Active;
            authorEntity.UpdatedAt = DateTime.UtcNow;

            Context.SaveChanges();
            author = authorEntity.Adapt<AuthorDto>();
            return author;
        }

        public void DeactivateAuthor(AuthorDto author)
        {
            Author authorEntity = Context.Author.Find(author.Id);

            if (authorEntity == null)
            {
                authorEntity = Context.Author.FirstOrDefault(i => i.UserId == author.UserId);
            }

            if (authorEntity == null)
            {
                throw new Exception($"Invalid author Id ({author.Id}) and UserId ({author.UserId}) .");
            }

            authorEntity.Active = false;

            Context.SaveChanges();
        }

        public void DeleteAuthor(int id)
        {
            Author authorEntity = Context.Author.Find(id);
            if (authorEntity != null)
            {
                authorEntity = Context.Author.FirstOrDefault(i => i.UserId == authorEntity.UserId);
                Context.Remove(authorEntity);
                Context.SaveChanges();
            }
        }

    }
}
