using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Mediator.Api.Database;
using Mediator.Api.Features.Users;
using Mediator.Api.Models;
using Mediator.Api.Tests.TestConfiguration;
using Mediator.Api.Tests.TestSets;
using Xunit;

namespace Mediator.Api.Tests
{
    public class GetUsersTests : HandlerTestBase<MediatorContext, GetUsers.Handler>
    {
        private readonly List<User> _users;

        public GetUsersTests()
        {
            _users = new UsersSet().Users;
            var dbSet = Aef.FakeDbSet(_users, x => x.Id == 1);
            A.CallTo(() => Context.Users).Returns(dbSet);
        }

        [Fact]
        public async Task GetUsers_UsersReturned()
        {
            // Arrange
            var command = new GetUsers.Command();

            // Act
            var result = await Handler.Handle(command);

            // Assert
            result.Value.Should().NotBeEmpty();
            result.Value.Should().HaveCount(5);
            result.Value.Should().BeEquivalentTo(_users);
        }
    }
}
