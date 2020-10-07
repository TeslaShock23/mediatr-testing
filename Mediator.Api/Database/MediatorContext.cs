using Mediator.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Mediator.Api.Database
{
    public class MediatorContext : DbContext
    {
        public MediatorContext(DbContextOptions<MediatorContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }
}