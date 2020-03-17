namespace DynamicDataLibrary.ViewModel
{
    public class ContourViewModel : ViewModelBase
    {
        public ContourViewModel(int id, int parentId, string name, ViewModelBase parent, Document parentDocument)
          : base(id, parentId, name, parent, parentDocument)
        {
            int markerId = parentDocument.GetNextId();
            var markerViewModel = new MarkerViewModel(markerId, id, "marker" + markerId.ToString(), this, parentDocument);
            parentDocument.AddChild(markerViewModel);
        }
    }
}
