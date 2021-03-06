﻿namespace DynamicDataLibrary.ViewModel
{
    public class EntityViewModey : ViewModelBase
    {
        public EntityViewModey(int id, int parentId, string name, ViewModelBase parent, Document parentDocument) 
            : base(id, parentId, name, parent, parentDocument)
        {
            int markerId = parentDocument.GetNextId();
            var markerViewModel = new MarkerViewModel(markerId, id, "marker"+ markerId.ToString(), this, parentDocument);
            parentDocument.AddChild(markerViewModel);
        }
    }
}
