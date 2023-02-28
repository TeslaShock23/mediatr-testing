using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Mediator.Api.Tests.TestConfiguration;

internal class TestAsyncQueryProvider<TEntity> : TestQueryProvider<TEntity>, IAsyncEnumerable<TEntity>, IAsyncQueryProvider
{
    public TestAsyncQueryProvider(Expression expression) : base(expression)
    {
    }

    public TestAsyncQueryProvider(IEnumerable<TEntity> enumerable) : base(enumerable)
    {
    }

    public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default) => new TestAsyncEnumerator<TEntity>(this.AsEnumerable().GetEnumerator());

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        var expectedResultType = typeof(TResult).GetGenericArguments()[0];
        var executionResult = typeof(IQueryProvider)
            .GetMethods()
            .First(method => method.Name == nameof(IQueryProvider.Execute) && method.IsGenericMethod)
            .MakeGenericMethod(expectedResultType)
            .Invoke(this, new object[] { expression });

        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
            .MakeGenericMethod(expectedResultType)
            .Invoke(null, new[] { executionResult });
    }
}