// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.OData.Client;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.ODataAdapter.Tests;

public class ODataAsyncQueryExecutorTests
{
    private static readonly Uri _serviceRoot = new("http://localhost/odata/");
    private readonly ODataAsyncQueryExecutor _executor = new();

    [Fact]
    public void IsSupported_WithDataServiceQueryProvider_ReturnsTrue()
    {
        var context = new DataServiceContext(_serviceRoot);
        IQueryable<TestEntity> query = context.CreateQuery<TestEntity>("Entities");

        Assert.True(_executor.IsSupported(query));
    }

    [Fact]
    public void IsSupported_WithRegularQueryable_ReturnsFalse()
    {
        IQueryable<TestEntity> query = new List<TestEntity>().AsQueryable();

        Assert.False(_executor.IsSupported(query));
    }

    [Fact]
    public async Task CountAsync_WhenPreCancelledToken_ThrowsOperationCanceledException()
    {
        var context = new DataServiceContext(_serviceRoot);
        IQueryable<TestEntity> query = context.CreateQuery<TestEntity>("Entities");

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => _executor.CountAsync(query, cts.Token));
    }

    [Fact]
    public async Task ToArrayAsync_WhenPreCancelledToken_ThrowsOperationCanceledException()
    {
        var context = new DataServiceContext(_serviceRoot);
        IQueryable<TestEntity> query = context.CreateQuery<TestEntity>("Entities");

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => _executor.ToArrayAsync(query, cts.Token));
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    [Key("Id")]
    private sealed class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
