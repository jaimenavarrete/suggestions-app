namespace SuggestionsApp.Models.QueryFilters;

public class SuggestionQueryFilter
{
    public int? CategoryId { get; set; }

    public int? StateId { get; set; }

    public string? SearchText { get; set; }

    public string? UserId { get; set; }

    public OrderBy OrderBy { get; set; } = OrderBy.DateDesc;
}

public enum OrderBy
{
    DateAsc = 1,
    DateDesc = 2,
    PopularityAsc = 3,
    PopularityDesc = 4
}