namespace ApplicationCore.Models.SearchContexts;

public class PaginationContext
{
    public int Page { get; set; }

    public PaginationContext(int page)
    {
        Page = page;
    }

    public PaginationContext()
    {
    }
}