using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class GenreicRepo<T> : IGenreicRepo<T> where T : class
    {
        private readonly MohamedSprint1V2DbContext context;

        public GenreicRepo(MohamedSprint1V2DbContext context)
        {
            this.context = context;
        }
        public void Add(T entity)
        {
             context.Set<T>().Add(entity) ;
        }

        public void Delete(int id)
        {
             context.Set<T>().Remove(context.Set<T>().Find(id)) ;
        }

        public List<T> getAll(Expression<Func<T, bool>>? filter = null)
        {
            if (filter == null)
                return context.Set<T>().ToList();
            return context.Set<T>().Where(filter).ToList();
        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
             context.Set<T>().Update(entity) ; 
        }
    }
}
