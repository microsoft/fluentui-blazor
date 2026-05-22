// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapter.Tests;

public class EntityFrameworkAsyncQueryExecutorTests : IDisposable
{
    private readonly TestDbContext _dbContext;
    private readonly EntityFrameworkAsyncQueryExecutor _executor;
    private readonly IAsyncQueryExecutor _interface;

    public EntityFrameworkAsyncQueryExecutorTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        _dbContext = new TestDbContext(options);
        _dbContext.Database.OpenConnection();
        _dbContext.Database.EnsureCreated();

        _executor = new EntityFrameworkAsyncQueryExecutor();
        _interface = _executor;
    }

    public void Dispose()
    {
        ((IDisposable)_executor).Dispose();
        _dbContext.Dispose();
    }

    [Fact]
    public void IsSupported_WhenProviderIsIAsyncQueryProvider_ReturnsTrue()
    {
        IQueryable<TestEntity> query = _dbContext.Entities;

        Assert.True(query.Provider is IAsyncQueryProvider);
        Assert.True(_executor.IsSupported(query));
    }

    [Fact]
    public void IsSupported_WhenProviderIsNotIAsyncQueryProvider_ReturnsFalse()
    {
        IQueryable<TestEntity> query = new List<TestEntity>().AsQueryable();

        Assert.False(_executor.IsSupported(query));
    }

    [Fact]
    public async Task CountAsync_ReturnsCorrectCount()
    {
        _dbContext.Entities.AddRange(
            new TestEntity { Id = 1, Name = "A" },
            new TestEntity { Id = 2, Name = "B" },
            new TestEntity { Id = 3, Name = "C" });
        await _dbContext.SaveChangesAsync();

        var count = await _interface.CountAsync(_dbContext.Entities);

        Assert.Equal(3, count);
    }

    [Fact]
    public async Task ToArrayAsync_ReturnsAllItems()
    {
        _dbContext.Entities.AddRange(
            new TestEntity { Id = 1, Name = "A" },
            new TestEntity { Id = 2, Name = "B" });
        await _dbContext.SaveChangesAsync();

        var result = await _interface.ToArrayAsync(_dbContext.Entities);

        Assert.Equal(2, result.Length);
        Assert.Contains(result, e => e.Name == "A");
        Assert.Contains(result, e => e.Name == "B");
    }

    [Fact]
    public async Task CountAsync_WhenPreCancelledToken_ThrowsOperationCanceledException()
    {
        _dbContext.Entities.Add(new TestEntity { Id = 1, Name = "A" });
        await _dbContext.SaveChangesAsync();

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => _interface.CountAsync(_dbContext.Entities, cts.Token));
    }

    [Fact]
    public async Task ToArrayAsync_WhenPreCancelledToken_ThrowsOperationCanceledException()
    {
        _dbContext.Entities.Add(new TestEntity { Id = 1, Name = "A" });
        await _dbContext.SaveChangesAsync();

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => _interface.ToArrayAsync(_dbContext.Entities, cts.Token));
    }

    [Fact]
    public async Task ToArrayAsync_WhenDisposed_ReturnsDefault()
    {
        _dbContext.Entities.Add(new TestEntity { Id = 1, Name = "A" });
        await _dbContext.SaveChangesAsync();

        using var executor = new EntityFrameworkAsyncQueryExecutor();
        IAsyncQueryExecutor iface = executor;
        ((IDisposable)executor).Dispose();

        // ObjectDisposedException from the disposed SemaphoreSlim should be swallowed
        var result = await iface.ToArrayAsync(_dbContext.Entities);

        Assert.Equal(default, result);
    }

    [Fact]
    public async Task ToArrayAsync_WhenIgnoreExceptionMatches_ReturnsDefault()
    {
        var thrownException = new InvalidOperationException("simulated query failure");
        using var executor = new EntityFrameworkAsyncQueryExecutor(ex => ReferenceEquals(ex, thrownException));
        IAsyncQueryExecutor iface = executor;

        IQueryable<TestEntity> query = new ThrowingQueryable<TestEntity>(thrownException);

        var result = await iface.ToArrayAsync(query);

        Assert.Equal(default, result);
    }

    [Fact]
    public async Task ToArrayAsync_WhenIgnoreExceptionDoesNotMatch_ThrowsOriginalException()
    {
        var thrownException = new InvalidOperationException("simulated query failure");
        using var executor = new EntityFrameworkAsyncQueryExecutor(ex => false);
        IAsyncQueryExecutor iface = executor;

        IQueryable<TestEntity> query = new ThrowingQueryable<TestEntity>(thrownException);

        var caughtEx = await Assert.ThrowsAnyAsync<Exception>(() => iface.ToArrayAsync(query));
        Assert.Same(thrownException, caughtEx);
    }

    [Fact]
    public async Task CountAsync_WhenDisposed_ReturnsDefault()
    {
        _dbContext.Entities.Add(new TestEntity { Id = 1, Name = "A" });
        await _dbContext.SaveChangesAsync();

        using var executor = new EntityFrameworkAsyncQueryExecutor();
        IAsyncQueryExecutor iface = executor;
        ((IDisposable)executor).Dispose();

        // ObjectDisposedException from the disposed SemaphoreSlim should be swallowed
        var result = await iface.CountAsync(_dbContext.Entities);

        Assert.Equal(default, result);
    }

    [Fact]
    public async Task CountAsync_WhenIgnoreExceptionMatches_ReturnsDefault()
    {
        // Use a fake IAsyncQueryProvider that throws a controlled exception to test the
        // ignoreException path without depending on a specific database error type.
        var thrownException = new InvalidOperationException("simulated query failure");
        using var executor = new EntityFrameworkAsyncQueryExecutor(ex => ReferenceEquals(ex, thrownException));
        IAsyncQueryExecutor iface = executor;

        IQueryable<TestEntity> query = new ThrowingQueryable<TestEntity>(thrownException);

        var result = await iface.CountAsync(query);

        Assert.Equal(default, result);
    }

    [Fact]
    public async Task CountAsync_WhenIgnoreExceptionDoesNotMatch_ThrowsOriginalException()
    {
        var thrownException = new InvalidOperationException("simulated query failure");
        using var executor = new EntityFrameworkAsyncQueryExecutor(ex => false);
        IAsyncQueryExecutor iface = executor;

        IQueryable<TestEntity> query = new ThrowingQueryable<TestEntity>(thrownException);

        var caughtEx = await Assert.ThrowsAnyAsync<Exception>(() => iface.CountAsync(query));
        Assert.Same(thrownException, caughtEx);
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        public DbSet<TestEntity> Entities { get; set; } = default!;
    }

    private sealed class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// A fake <see cref="IQueryable{T}"/> backed by an <see cref="IAsyncQueryProvider"/> that
    /// throws a controlled exception whenever a query is executed asynchronously.
    /// </summary>
    /// <summary>
    /// A fake <see cref="IQueryable{T}"/> backed by an <see cref="IAsyncQueryProvider"/> that
    /// throws a controlled exception whenever a query is executed asynchronously.
    /// Also implements <see cref="IAsyncEnumerable{T}"/> so that EF Core's ToArrayAsync
    /// (which uses AsAsyncEnumerable) reaches the controlled exception path.
    /// </summary>
    private sealed class ThrowingQueryable<T>(Exception exception) : IQueryable<T>, IAsyncEnumerable<T>
    {
        public Type ElementType => typeof(T);
        public Expression Expression => Expression.Constant(this);
        public IQueryProvider Provider => new ThrowingAsyncQueryProvider(exception);

        public IEnumerator<T> GetEnumerator() => Enumerable.Empty<T>().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => new ExceptionAsyncEnumerator<T>(exception);
    }

    private sealed class ThrowingAsyncQueryProvider(Exception exception) : IAsyncQueryProvider
    {
        public IQueryable CreateQuery(Expression expression) => throw new NotSupportedException();
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => throw new NotSupportedException();
        public object? Execute(Expression expression) => throw exception;
        public TResult Execute<TResult>(Expression expression) => throw exception;

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            // EF Core calls ExecuteAsync<Task<int>> for CountAsync and
            // ExecuteAsync<IAsyncEnumerable<T>> for ToArrayAsync.
            var returnType = typeof(TResult);
            if (returnType.IsGenericType)
            {
                var genericDef = returnType.GetGenericTypeDefinition();
                var innerType = returnType.GetGenericArguments()[0];

                if (genericDef == typeof(Task<>))
                {
                    // Task.FromException(Exception) and Task.FromException<T>(Exception) are both named
                    // "FromException", so we must select the generic overload explicitly.
                    var fromEx = typeof(Task)
                        .GetMethods()
                        .Single(m => m.Name == nameof(Task.FromException) && m.IsGenericMethodDefinition)
                        .MakeGenericMethod(innerType);
                    return (TResult)fromEx.Invoke(null, [exception])!;
                }

                if (genericDef == typeof(IAsyncEnumerable<>))
                {
                    var enumType = typeof(ExceptionAsyncEnumerable<>).MakeGenericType(innerType);
                    return (TResult)Activator.CreateInstance(enumType, exception)!;
                }
            }

            throw exception;
        }
    }

    private sealed class ExceptionAsyncEnumerable<T>(Exception exception) : IAsyncEnumerable<T>
    {
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => new ExceptionAsyncEnumerator<T>(exception);
    }

    private sealed class ExceptionAsyncEnumerator<T>(Exception exception) : IAsyncEnumerator<T>
    {
        public T Current => default!;
        public ValueTask<bool> MoveNextAsync() => ValueTask.FromException<bool>(exception);
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}

