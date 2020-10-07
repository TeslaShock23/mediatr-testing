using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Api.Features.Users;
using Mediator.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mediator.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers(CancellationToken cancellationToken) => 
            (await _mediator.Send(new GetUsers.Command(), cancellationToken)).Value;
    }
}