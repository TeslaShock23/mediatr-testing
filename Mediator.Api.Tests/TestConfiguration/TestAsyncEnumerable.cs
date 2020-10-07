using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mediator.Api.Tests.TestConfiguration
{
    // https://docs.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking?redirectedfrom=MSDN#testing-query-scenarios
    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());

        public T Current => _inner.Current;

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken) => Task.FromResult(_inner.MoveNext());
    }
    
    // internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    // {
    //     private readonly IQueryProvider _inner;
    //
    //     internal TestAsyncQueryProvider(IQueryProvider inner)
    //     {
    //         _inner = inner;
    //     }
    //
    //     public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);
    //
    //     public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);
    //
    //     public object Execute(Expression expression) => _inner.Execute(expression);
    //
    //     public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);
    //
    //     public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken) => Execute<TResult>(expression);
    //
    //     public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken) => Task.FromResult(Execute(expression));
    // }
    //
    // internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    // {
    //     public TestAsyncEnumerable(IEnumerable<T> enumerable)
    //         : base(enumerable)
    //     {
    //     }
    //
    //     public TestAsyncEnumerable(Expression expression)
    //         : base(expression)
    //     {
    //     }
    //
    //     public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken()) => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    //
    //     IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    // }
}