namespace MohamedSprint1V2.DAL.Repo.Impelementation
{
    public class CategoryRepo : GenreicRepo<Category>,ICategoryRepo  
    {

        private MohamedSprint1V2DbContext context;
        public CategoryRepo(MohamedSprint1V2DbContext context):base(context) 
        {
            this.context = context;
        }
       
    }
}
