using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
    public interface IGenreicRepo<T> where T: class
    {
        void Add(T entity  );
        void Update(T  entity);
        void Delete(int id);
        T GetById(int id);
        List<T> getAll(Expression<Func<T, bool>>? filter = null);
    }
}
