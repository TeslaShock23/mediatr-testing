using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediator.Api.Tests.TestConfiguration;

internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public T Current => _inner.Current;

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return new ValueTask();
    }

    public ValueTask<bool> MoveNextAsync() => new(_inner.MoveNext());
}