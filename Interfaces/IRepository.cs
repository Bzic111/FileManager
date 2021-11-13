using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileManager.Base;

namespace FileManager.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        int GetCount() => GetAll().Count();

        IEnumerable<T> GetAll();

        T? GetById(int id);

        void Add(T entity);

        void Update(int id, T person);

        bool Delete(T entity);
    }
}
