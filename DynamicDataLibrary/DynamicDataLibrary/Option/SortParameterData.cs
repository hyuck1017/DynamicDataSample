using DynamicData.Binding;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicDataLibrary.ViewModel;

namespace DynamicDataLibrary.Option
{
    public class SortParameterData : AbstractNotifyPropertyChanged
    {
        private readonly IList<SortContainer> _sortItems = new ObservableCollection<SortContainer>
        {
            new SortContainer("Ascending", SortExpressionComparer<ViewModelBase>
                .Ascending(l => l.Id)),

            new SortContainer("Descending", SortExpressionComparer<ViewModelBase>
                .Descending(l => l.Id)),
        };

        private SortContainer _selectedItem;


        public SortParameterData()
        {
            SelectedItem = _sortItems[1];
        }

        public SortContainer SelectedItem
        {
            get => _selectedItem;
            set => SetAndRaise(ref _selectedItem, value);
        }

        public IList<SortContainer> SortItems => _sortItems;
    }
}
