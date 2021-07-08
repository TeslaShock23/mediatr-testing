using System;
using Microsoft.EntityFrameworkCore;

namespace Mediator.Api.Tests.TestConfiguration
{
    public class HandlerTestBase<TC, TH> where TC : DbContext
    {
        protected readonly TC Context;
        protected readonly TH Handler;

        protected HandlerTestBase()
        {
            Context = DbContextProvider.CreateContext<TC>();
            Handler = (TH)Activator.CreateInstance(typeof(TH), Context);
        }
    }
}
