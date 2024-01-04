using YANLib;
using static System.Math;

namespace TrackMap.Common.SeedWork;

public sealed class PagedList<T>
{
    public PagedList() { }

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = Ceiling(count.ToDouble() / pageSize).ToInt()
        };

        Items = items;
    }

    public MetaData? MetaData { get; set; }

    public List<T>? Items { get; set; }
}
