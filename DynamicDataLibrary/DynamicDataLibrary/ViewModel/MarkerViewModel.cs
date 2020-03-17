namespace DynamicDataLibrary.ViewModel
{
    public class MarkerViewModel : ViewModelBase
    {
        public MarkerViewModel(int id, int parentId, string name, ViewModelBase parent, Document parentDocument) 
            : base(id, parentId, name, parent, parentDocument)
        {
        }
    }
}
