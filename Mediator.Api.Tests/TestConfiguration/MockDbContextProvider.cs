using Microsoft.EntityFrameworkCore;
using Moq;

namespace Mediator.Api.Tests.TestConfiguration
{
    public class MockDbContextProvider
    {
        public static Mock<T> CreateContext<T>() where T : DbContext => new Mock<T>(new DbContextOptions<T>());
    }
}