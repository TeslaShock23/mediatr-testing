using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Api.Database;
using Mediator.Api.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Mediator.Api.Features.Users
{
    public class GetUsers
    {
        public class Command : IRequest<Response>
        {
        }

        public class Response
        {
            public List<User> Value { get; set; }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly MediatorContext _context;

            public Handler(MediatorContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken = default) => new Response
            {
                Value = await _context.Users.AsNoTracking().ToListAsync(cancellationToken)
            };
        }
    }
}