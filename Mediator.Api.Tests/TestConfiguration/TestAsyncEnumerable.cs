﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Mediator.Api.Tests.TestConfiguration
{
    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        public TestAsyncEnumerable(Expression expression) : base(expression)
        {
        }

        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);

        public IAsyncEnumerator<T> GetEnumerator() => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new()) => throw new NotImplementedException();
    }
}
