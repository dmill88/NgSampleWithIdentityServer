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
    public class AuthorCRUDTests
    {

        AuthorQueries _authorQueries = null;
        AuthorCommands _authorCmds = null;

        [TestInitialize]
        public void Init()
        {
            ContentleverageContext dbRepository = new ContentleverageContext();
            _authorQueries = new AuthorQueries(dbRepository);
            _authorCmds = new AuthorCommands(dbRepository);
        }

        [TestMethod]
        public void AuthorCRUD()
        {
            string userId = "a1b05600-3e90-4271-3de1-7a88ae4c5c88";
            AuthorDto authorDto = _authorCmds.AddAuthor(userId, "Joe", "Enon", "jenon", "A wild and crazy test guy.");
            authorDto.Alias = "theTestGuy";
            authorDto.Bio = "A really, really wild and crazy test guy.";
            _authorCmds.UpdateAuthor(authorDto);
            authorDto = _authorQueries.GetAuthor(authorDto.Alias);
            Assert.IsNotNull(authorDto);

            AuthorDto notExistsAuthor = _authorQueries.GetAuthor("sdfsfsdfsdfsdfsd");
            Assert.IsNull(notExistsAuthor);

            _authorCmds.DeactivateAuthor(authorDto);
            authorDto = _authorQueries.GetAuthor(authorDto.Alias);
            _authorCmds.DeleteAuthor(authorDto.Id);
            //Assert.IsTrue(authorDto.);
        }

    }
}
