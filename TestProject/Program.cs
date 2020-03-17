using DynamicDataLibrary.ViewModel;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {            
            Document document = new Document();

            // Add data (total = 2 * 2000 * 2);
            document.InitializeCategoryViewModels(2, 2000);
            
            // Clear
            document.ChildrenService.Clear();

            // Remove
            document.ChildrenService.TestRemove();
        }
    }
}
