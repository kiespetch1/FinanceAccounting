using ApplicationCore.Models;

namespace ApplicationCore.Utils;

public class TypeResponseComparer<T> : IEqualityComparer<TypeResponse<T>>
{
    public bool Equals(TypeResponse<T> x, TypeResponse<T> y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x == null || y == null)
            return false;

        if (x.Total != y.Total)
            return false;

        if (x.Items.Count != y.Items.Count)
            return false;
            
        for (int i = 0; i < x.Items.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(x.Items[i], y.Items[i]))
                return false;
        }

        return true;
    }

    public int GetHashCode(TypeResponse<T> obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        var hash = 17;
        hash = hash * 23 + obj.Total.GetHashCode();

        foreach (var item in obj.Items)
        {
            hash = hash * 23 + EqualityComparer<T>.Default.GetHashCode(item);
        }

        return hash;
    }
}