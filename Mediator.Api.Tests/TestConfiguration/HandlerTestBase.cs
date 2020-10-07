using System;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Mediator.Api.Tests.TestConfiguration
{
    public class HandlerTestBase<TC, TH> where TC : DbContext
    {
        protected readonly Mock<TC> Context;
        protected readonly TH Handler;

        protected HandlerTestBase()
        {
            Context = MockDbContextProvider.CreateContext<TC>();
            Handler = (TH)Activator.CreateInstance(typeof(TH), Context.Object);
        }
    }
}