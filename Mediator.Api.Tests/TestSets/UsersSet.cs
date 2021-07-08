using System.Collections.Generic;
using Bogus;
using Mediator.Api.Models;

namespace Mediator.Api.Tests.TestSets
{
    public class UsersSet
    {
        private readonly Faker<User> _faker = new();

        public UsersSet(int count = 5)
        {
            _faker.StrictMode(true)
                .RuleFor(o => o.Id, f => f.IndexFaker + 1)
                .RuleFor(o => o.FirstName, f => f.Name.FirstName())
                .RuleFor(o => o.LastName, f => f.Name.LastName());

            Users = _faker.Generate(count);
        }

        public List<User> Users { get; }
    }
}
