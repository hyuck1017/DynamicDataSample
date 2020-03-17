using DynamicData.Binding;

namespace DynamicDataLibrary
{
    public class SearchFilter : AbstractNotifyPropertyChanged
    {
        private string searchText = string.Empty;

        public string SearchText
        {
            get => this.searchText;
            set => SetAndRaise(ref this.searchText, value);
        }
    }
}
