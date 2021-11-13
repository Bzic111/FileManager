using System.Linq;

namespace FileManager.Extensions
{
    //public static class RefListEx
    //{
    //    [Conditional("IncludeInternal")]
    //    public static void PrintToConsole<T>(this RefList<T> list, string Separator = ",")
    //    {
    //        Console.WriteLine(list.ToSeparateString(Separator));
    //    }
    //}
    public static class RepositoryEx
    {
        public static int GetCountWhere<T>(this IRepository<T> repository, Func<T, bool> Selector)
            where T : IEntity
        {
            return repository.GetAll().Count(Selector);
        }
    }
}