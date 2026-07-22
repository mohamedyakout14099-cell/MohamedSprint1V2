using MohamedSprint1V2.DLL.ModelVM.Category;
using MohamedSprint1V2.DLL.ModelVM.Product;

namespace MohamedSprint1V2.DLL.Service.Abstraction
{
    public interface ICategoryService
    {
        Response<bool> addCategory(AddCategoryVM categoryVM);
        Response<bool> updateCategory(UpdaeteCategoryVM categoryVM);
        Response<bool> deleteCategoryById(int categoryId);
        Response<List<GetallCategoryVM>> getAllCategories();
        Response<UpdaeteCategoryVM> getCategoryById(int categoryId);

    }
}
