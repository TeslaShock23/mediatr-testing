using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Mediator.Api.Tests.TestConfiguration
{
    public static class MockDbContextExtensions
    {
        public static Mock<DbSet<T>> SetupDbSet<TC, T>(this Mock<TC> dbContext, Expression<Func<TC, DbSet<T>>> getDbSet) where T : class where TC : DbContext =>
            SetupDbSet(dbContext, getDbSet, new List<T>());

        public static Mock<DbSet<T>> SetupDbSet<TC, T>(this Mock<TC> dbContext, Expression<Func<TC, DbSet<T>>> getDbSet, T obj) where T : class where TC : DbContext =>
            SetupDbSet(dbContext, getDbSet, new List<T> {obj});

        public static Mock<DbSet<T>> SetupDbSet<TC, T>(this Mock<TC> dbContext, Expression<Func<TC, DbSet<T>>> getDbSet, IEnumerable<T> objs) where T : class where TC : DbContext
        {
            var dbSet = MockDbSetCreator.Create(objs);
            dbContext.Setup(getDbSet).Returns(dbSet.Object);
            return dbSet;
        }

        public static void SetupDbSetException<TC, T>(this Mock<TC> dbContext, Expression<Func<TC, DbSet<T>>> getDbSet, Exception ex) where T : class where TC : DbContext
        {
            dbContext.Setup(getDbSet).Throws(ex);
        }
    }
}