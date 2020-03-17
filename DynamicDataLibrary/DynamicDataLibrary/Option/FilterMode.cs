using DynamicData.Binding;

namespace DynamicDataLibrary
{
    public class FilterMode : AbstractNotifyPropertyChanged
    {
        private TreeMode treeMode = TreeMode.General;

        public TreeMode TreeMode
        {
            get => this.treeMode;
            set => SetAndRaise(ref this.treeMode, value);
        }
    }
}
