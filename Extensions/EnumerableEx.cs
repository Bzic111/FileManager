namespace FileManager.Extensions
{
    public static class EnumerableEx
    {
        public static string ToSeparateString<T>(this IEnumerable<T> items, string Separator = ", ")
        {
            return string.Join(Separator, items);
        }

    }
}