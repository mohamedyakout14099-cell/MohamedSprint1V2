using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
    public interface IGenreicRepo<T> where T: class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        bool SoftDelete(int id);
        bool Restore(int id);
        T GetById(int id);
        T GetByIdIgnoreFilter(int id);
        List<T> getAll(Expression<Func<T, bool>>? filter = null);
        List<T> GetAllIncludingDeleted();
    }
}
