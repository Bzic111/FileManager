namespace FileManager.Interfaces
{
    public interface IPersonsRepository<T> : IRepository<T> where T : Person, IEntity
    {
        T? GetByName(string LstName, string Name, string Patronymic, DateTime Birthday);
    }
}
