using MohamedSprint1V2.DLL.ModelVM.Category;

namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public Response<bool> addCategory(AddCategoryVM categoryVM)
        {
            try
            {
                if (categoryVM != null)
                {
                    var category = new Category(0, categoryVM.Name, categoryVM.Description);
                        unitOfWork.Category.Add(category);
                    var result = unitOfWork.Save();
                    if(result>0)
                    {
                        return new Response<bool>(true, null, true);
                    }
                    return new Response<bool>(false,   "Category was not added", false);
                }
                return new Response<bool>(false, "Category is null", false);

            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.ToString(), false);
            }
        }
        public Response<bool> deleteCategoryById(int categoryId)
        {
            try
            {
                unitOfWork.Category.Delete(categoryId);
                var result = unitOfWork.Save();
                if (result > 0)
                {
                    return new Response<bool>(true, null, true);
                }
                return new Response<bool>(false, "Category not found", false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }
        Response<List<GetallCategoryVM>> ICategoryService.getAllCategories()
        {
            try
            {
                var result = unitOfWork.Category.getAll();
                if (result != null)
                {
                    List<GetallCategoryVM> mapp = new List<GetallCategoryVM>();
                    foreach (var item in result)
                    {
                        mapp.Add(new GetallCategoryVM() { id = item.Id, name = item.Name, description = item.Description, CreatedTime = item.CreatedTime });
                    }
                    return new Response<List<GetallCategoryVM>>(mapp, null, true);
                }
                return new Response<List<GetallCategoryVM>>(null, "No categories found", false);

            }
            catch (Exception ex)
            {
                return new Response<List<GetallCategoryVM>>(null, ex.Message, false);
            }
        }
        Response<UpdaeteCategoryVM> ICategoryService.getCategoryById(int categoryId)
        {
            try
            {
                var result = unitOfWork.Category.GetById(categoryId);
                if (result != null)
                {
                    var mapp = new UpdaeteCategoryVM() { Id = result.Id, Name = result.Name, Description = result.Description };
                    return new Response<UpdaeteCategoryVM>(mapp, null, true);
                }
                return new Response<UpdaeteCategoryVM>(null, "Category not found", false);
            }
            catch (Exception ex)
            {
                return new Response<UpdaeteCategoryVM>(null, ex.Message, false);
            }
        }
        Response<bool> ICategoryService.updateCategory(UpdaeteCategoryVM categoryVM)
        {
            try
            {
                var category = new Category(
                    categoryVM.Id,
                    categoryVM.Name,
                    categoryVM.Description);

                unitOfWork.Category.Update(category);
                var result = unitOfWork.Save();

                if (result > 0)
                    return new Response<bool>(true, null, true);
                return new Response<bool>(false, "Failed to update category", false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }
    }

}
