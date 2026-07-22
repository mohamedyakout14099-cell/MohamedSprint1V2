using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MohamedSprint1V2.DAL.Repo.Abstraction
{
   public interface ICategoryRepo
    {
        bool AddCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(int id);
        Category GetCategoryById(int id);
        List<Category> getAll(Expression<Func<Category, bool>>? filter = null);
    }
}
