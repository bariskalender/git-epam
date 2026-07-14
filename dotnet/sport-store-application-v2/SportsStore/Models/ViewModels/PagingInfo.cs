namespace SportsStore.Models.ViewModels;

public class PagingInfo
{
    public int TotalItems { get; init; }

    public int ItemsPerPage { get; init; }

    public int CurrentPage { get; init; }

    public int TotalPages =>
        ItemsPerPage == 0
            ? 0
            : (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
}
