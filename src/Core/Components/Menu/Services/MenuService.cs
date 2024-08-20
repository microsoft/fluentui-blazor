// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public class MenuService : IMenuService, IDisposable
{
    /// <summary>
    /// <see cref="IMenuService.OnMenuUpdated" />
    /// </summary>
    public Action OnMenuUpdated { get; set; } = () => { };

    private ReaderWriterLockSlim MenuLock { get; } = new ReaderWriterLockSlim();

    private IList<FluentMenu> MenuList { get; } = new List<FluentMenu>();

    /// <summary>
    /// <see cref="IMenuService.Menus" />
    /// </summary>
    public IEnumerable<FluentMenu> Menus
    {
        get
        {
            MenuLock.EnterReadLock();
            try
            {
                return MenuList;
            }
            finally
            {
                MenuLock.ExitReadLock();
            }
        }
    }

    /// <summary>
    /// <see cref="IMenuService.Add(FluentMenu)" />
    /// </summary>
    public void Add(FluentMenu menu)
    {
        MenuLock.EnterWriteLock();
        try
        {
            MenuList.Add(menu);
        }
        finally
        {
            MenuLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// <see cref="IMenuService.Remove(FluentMenu)" />
    /// </summary>
    public void Remove(FluentMenu menu)
    {
        MenuLock.EnterWriteLock();
        try
        {
            var item = MenuList.FirstOrDefault(i => i.Id == menu.Id);
            if (item != null)
            {
                MenuList.Remove(item);
            }
        }
        finally
        {
            MenuLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// <see cref="IMenuService.Clear" />
    /// </summary>
    public void Clear()
    {
        MenuLock.EnterWriteLock();
        try
        {
            MenuList.Clear();
        }
        finally
        {
            MenuLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// <see cref="IMenuService.Clear" />
    /// </summary>
    public void Dispose()
    {
        Clear();
    }

    /// <summary>
    /// <see cref="IMenuService.RefreshMenuAsync(string, bool)" />
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isOpen"></param>
    /// <returns></returns>
    public async Task RefreshMenuAsync(string id, bool isOpen)
    {
        var item = MenuList.FirstOrDefault(i => i.Id == id);

        if (item != null)
        {
            await item.SetOpenAsync(isOpen);
            OnMenuUpdated.Invoke();
        }
    }
}
