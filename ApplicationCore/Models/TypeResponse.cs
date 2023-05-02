namespace Entities.Models;

public class TypeResponse<T>
{
    public List<T> Items { get; set; } = new List<T>();

    public int Total { get; set; }
}