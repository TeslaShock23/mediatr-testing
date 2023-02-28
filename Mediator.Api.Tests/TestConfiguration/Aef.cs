using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace Mediator.Api.Tests.TestConfiguration
{
    public static class Aef
    {
        public static DbSet<T> FakeDbSet<T>(IList<T> data, Expression<Func<T, bool>> find = null) where T : class
        {
            var fakeDbSet = A.Fake<DbSet<T>>(b =>
            {
                b.Implements(typeof(IQueryable<T>));
                b.Implements(typeof(IAsyncEnumerable<T>));
            });

            QueryableSetUp(fakeDbSet, data);
            CollectionSetUp(fakeDbSet, data, find);

            return fakeDbSet;
        }

        public static DbSet<T> FakeDbSet<T>(IEnumerable<T> data, Expression<Func<T, bool>> find = null) where T : class
        {
            var fakeDbSet = A.Fake<DbSet<T>>(b =>
            {
                b.Implements(typeof(IAsyncEnumerable<T>));
                b.Implements(typeof(IQueryable<T>));
            });

            var listData = data.ToList();
            QueryableSetUp(fakeDbSet, listData);
            CollectionSetUp(fakeDbSet, listData, find);

            return fakeDbSet;
        }

        public static DbSet<T> FakeDbSet<T>(int numberOfFakes) where T : class
        {
            var data = A.CollectionOfFake<T>(numberOfFakes);
            return FakeDbSet(data);
        }

        public static DbSet<T> FakeDbSet<T>() where T : class => FakeDbSet(new List<T>());

        private static void QueryableSetUp<T>(DbSet<T> fakeDbSet, ICollection<T> data) where T : class
        {
            A.CallTo(() => ((IQueryable<T>)fakeDbSet).Provider).ReturnsLazily(_ => new TestAsyncQueryProvider<T>(data));
            A.CallTo(() => ((IQueryable<T>)fakeDbSet).Expression).ReturnsLazily(_ => data.AsQueryable().Expression);
            A.CallTo(() => ((IQueryable<T>)fakeDbSet).ElementType).ReturnsLazily(_ => data.AsQueryable().ElementType);
            A.CallTo(() => ((IQueryable<T>)fakeDbSet).GetEnumerator()).ReturnsLazily(_ => data.AsQueryable().GetEnumerator());
            A.CallTo(() => ((IAsyncEnumerable<T>)fakeDbSet).GetAsyncEnumerator(A<CancellationToken>._)).Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));
        }

        private static void CollectionSetUp<T>(DbSet<T> fakeDbSet, ICollection<T> data, Expression<Func<T, bool>> find = null) where T : class
        {
            A.CallTo(() => fakeDbSet.Add(A<T>._)).Invokes((T item) => data.Add(item));
            A.CallTo(() => fakeDbSet.AddRange(A<IEnumerable<T>>._)).Invokes((IEnumerable<T> items) =>
            {
                foreach (var item in items)
                {
                    data.Add(item);
                }
            });

            A.CallTo(() => fakeDbSet.Remove(A<T>._)).Invokes((T item) => data.Remove(item));
            A.CallTo(() => fakeDbSet.RemoveRange(A<IEnumerable<T>>._)).Invokes((IEnumerable<T> items) =>
            {
                foreach (var item in items.ToList())
                {
                    data.Remove(item);
                }
            });

            var func = find?.Compile();
            A.CallTo(() => fakeDbSet.Find(A<object[]>._)).ReturnsLazily((object[] _) => func is null ? null : data.FirstOrDefault(func));
            A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._)).ReturnsLazily((object[] _) => ValueTask.FromResult(func is null ? null : data.FirstOrDefault(func)));
            A.CallTo(() => fakeDbSet.FindAsync(A<object[]>._, A<CancellationToken>._)).ReturnsLazily((object[] _, CancellationToken _) => ValueTask.FromResult(func is null ? null : data.FirstOrDefault(func)));

            A.CallTo(() => fakeDbSet.Attach(A<T>._)).Invokes((T entity) => data.Add(entity));
        }
    }
}
