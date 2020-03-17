namespace DynamicDataLibrary.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
        public CategoryViewModel(int id, int parentId, string name, ViewModelBase parent, Document parentDocument) 
            : base(id, parentId, name, parent, parentDocument)
        {
        }
    }
}
