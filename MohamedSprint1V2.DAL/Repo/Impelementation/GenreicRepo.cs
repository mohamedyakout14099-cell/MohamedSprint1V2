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
            context.Set<T>().Add(entity);
        }

        public void Delete(int id)
        {
            context.Set<T>().Remove(context.Set<T>().Find(id));
        }

        public bool SoftDelete(int id)
        {
            var entity = context.Set<T>().Find(id);
            if (entity == null) return false;

            var prop = typeof(T).GetProperty("IsDeleted");
            if (prop == null) return false;

            prop.SetValue(entity, true);
            context.Set<T>().Update(entity);
            return true;
        }

        public bool Restore(int id)
        {
            var entity = context.Set<T>().IgnoreQueryFilters().FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
            if (entity == null) return false;

            var prop = typeof(T).GetProperty("IsDeleted");
            if (prop == null) return false;

            prop.SetValue(entity, false);
            context.Set<T>().Update(entity);
            return true;
        }

        public List<T> getAll(Expression<Func<T, bool>>? filter = null)
        {
            if (filter == null)
                return context.Set<T>().ToList();
            return context.Set<T>().Where(filter).ToList();
        }

        public List<T> GetAllIncludingDeleted()
        {
            return context.Set<T>().IgnoreQueryFilters().ToList();
        }

        public T GetById(int id)
        {
            return context.Set<T>().Find(id);
        }

        public T GetByIdIgnoreFilter(int id)
        {
            return context.Set<T>().IgnoreQueryFilters().FirstOrDefault(e => EF.Property<int>(e, "Id") == id);
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}
