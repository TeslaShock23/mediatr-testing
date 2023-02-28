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

namespace Mediator.Api.Tests;

public class GetUserTests : HandlerTestBase<MediatorContext, GetUser.Handler>
{
    private readonly List<User> _users;

    public GetUserTests()
    {
        // Get a set of fake users
        _users = new UsersSet().Users;

        // Create fake DbSet of users
        var dbSet = Aef.FakeDbSet(_users);

        // Mock out the call to the Users DbSet on the context
        A.CallTo(() => Context.Users).Returns(dbSet);
    }

    [Fact]
    public async Task GetUser_UserReturned()
    {
        // Arrange
        var command = new GetUser.Command();

        // Act
        var result = await Handler.Handle(command);

        // Assert
        result.Value.FirstName.Should().Be(_users[0].FirstName);
    }
}