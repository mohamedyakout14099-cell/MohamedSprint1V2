using Microsoft.Extensions.Caching.Memory;
using MohamedSprint1V2.DLL.ModelVM.Category;

namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMemoryCache memoryCache;

        private const string CategoriesCacheKey = "Categories_All";
        private static string GetCategoryCacheKey(int id) => $"Category_{id}";
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        public CategoryService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            this.unitOfWork = unitOfWork;
            this.memoryCache = memoryCache;
        }

        public Response<bool> addCategory(AddCategoryVM categoryVM)
        {
            try
            {
                if (categoryVM != null)
                {
                    var category = new Category(categoryVM.Name, categoryVM.Description);
                    unitOfWork.Category.Add(category);
                    var result = unitOfWork.Save();
                    if (result > 0)
                    {
                        memoryCache.Remove(CategoriesCacheKey);
                        return new Response<bool>(true, null, true);
                    }
                    return new Response<bool>(false, "Category was not added", false);
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
                    memoryCache.Remove(CategoriesCacheKey);
                    memoryCache.Remove(GetCategoryCacheKey(categoryId));
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
                if (memoryCache.TryGetValue(CategoriesCacheKey, out Response<List<GetallCategoryVM>>? cachedCategories) && cachedCategories != null)
                {
                    return cachedCategories;
                }

                var result = unitOfWork.Category.getAll();
                if (result != null)
                {
                    List<GetallCategoryVM> mapp = new List<GetallCategoryVM>();
                    foreach (var item in result)
                    {
                        mapp.Add(new GetallCategoryVM() { id = item.Id, name = item.Name, description = item.Description, CreatedTime = item.CreatedTime });
                    }
                    var response = new Response<List<GetallCategoryVM>>(mapp, null, true);
                    memoryCache.Set(CategoriesCacheKey, response, CacheDuration);
                    return response;
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
                string cacheKey = GetCategoryCacheKey(categoryId);
                if (memoryCache.TryGetValue(cacheKey, out Response<UpdaeteCategoryVM>? cachedCategory) && cachedCategory != null)
                {
                    return cachedCategory;
                }

                var result = unitOfWork.Category.GetById(categoryId);
                if (result != null)
                {
                    var mapp = new UpdaeteCategoryVM() { Id = result.Id, Name = result.Name, Description = result.Description };
                    var response = new Response<UpdaeteCategoryVM>(mapp, null, true);
                    memoryCache.Set(cacheKey, response, CacheDuration);
                    return response;
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
                    //categoryVM.Id,
                    categoryVM.Name,
                    categoryVM.Description);

                unitOfWork.Category.Update(category);
                var result = unitOfWork.Save();

                if (result > 0)
                {
                    memoryCache.Remove(CategoriesCacheKey);
                    memoryCache.Remove(GetCategoryCacheKey(categoryVM.Id));
                    return new Response<bool>(true, null, true);
                }
                return new Response<bool>(false, "Failed to update category", false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, ex.Message, false);
            }
        }
    }
}
