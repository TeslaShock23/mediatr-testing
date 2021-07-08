using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace Mediator.Api.Tests.TestConfiguration
{
    public class DbContextProvider
    {
        public static T CreateContext<T>() where T : DbContext => A.Fake<T>(x => x.WithArgumentsForConstructor(new[] { new DbContextOptions<T>() }));
    }
}
