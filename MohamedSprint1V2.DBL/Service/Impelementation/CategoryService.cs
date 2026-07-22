using MohamedSprint1V2.DLL.ModelVM.Category;

namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo categoryRepo;
        public CategoryService(ICategoryRepo categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }
        public Response<bool> addCategory(AddCategoryVM categoryVM)
        {
            try
            {
                if (categoryVM != null)
                {
                    var category = new Category(0, categoryVM.Name, categoryVM.Description);
                    var result = categoryRepo.AddCategory(category);
                    return new Response<bool>(result, null, true);
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
                var result = categoryRepo.DeleteCategory(categoryId);
                if (result)
                {
                    return new Response<bool>(result, null, true);
                }
                return new Response<bool>(result, "Category not found", false);


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
                var result = categoryRepo.getAll();
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
                var result = categoryRepo.GetCategoryById(categoryId);
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

                var result = categoryRepo.UpdateCategory(category);

                if (result)
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
