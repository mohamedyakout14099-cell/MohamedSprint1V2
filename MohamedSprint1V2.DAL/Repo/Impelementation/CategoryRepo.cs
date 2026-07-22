namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class CategoryRepo : ICategoryRepo
    {

        private MohamedSprint1V2DbContext Db;
        public CategoryRepo(MohamedSprint1V2DbContext Db)
        {
            this.Db = Db;
        }
        public bool AddCategory(Category category)
        {
            try
            {
                if (category != null)
                {
                    Db.Categories.Add(category);
                    Db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }
        public List<Category> getAll(Expression<Func<Category, bool>>? filter = null)
        {
            try
            {
                if (filter == null)
                {
                    return Db.Categories.ToList();
                }
                else
                {
                    return Db.Categories.Where(filter).ToList();
                }

            }
            catch
            {
                return Db.Categories.ToList();
            }
        }

        public bool DeleteCategory(int id)
        {
            var category = Db.Categories.Find(id);
            if (category != null)
            {
                Db.Categories.Remove(category);
                Db.SaveChanges();
                return true;
            }
            return false;
        }

        public Category GetCategoryById(int id)
        {
            return Db.Categories.Find(id);
        }
        public bool UpdateCategory(Category category)
        {
            try
            {
                var old = Db.Categories.Find(category.Id);
                if (old != null)
                {
                    old.update(category.Name, category.Description);
                    Db.SaveChanges();
                    return true;
                }
                return false;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
