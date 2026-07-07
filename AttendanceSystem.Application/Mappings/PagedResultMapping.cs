namespace AttendanceSystem.Application;

public static class PagedResultMapping
{
    public static PagedResult<TDto> ToPagedDto<TEntity, TDto>(
        this PagedResult<TEntity> source, Func<TEntity, TDto> map)
        => new()
        {
            Items = [.. source.Items.Select(map)],
            TotalCount = source.TotalCount,
            Page = source.Page,
            PageSize = source.PageSize
        };
}