using DynamicData.Binding;
using System;
using System.Linq;

namespace DynamicDataLibrary.Option
{
    public class ItemFilter : AbstractNotifyPropertyChanged
    {
        private FilterInfo filterInfo = new FilterInfo();

        private string searchText = string.Empty;
        private TreeMode treeMode = TreeMode.General;

        public string SearchText
        {
            get => this.searchText;
            set => SetAndRaise(ref searchText, value);
        }

        public TreeMode TreeMode
        {
            get => this.treeMode;
            set => SetAndRaise(ref this.treeMode, value);
        }

        public void UpdateModeFilter()
        {
            var preMode = this.treeMode;
            var tempMode = Enum.GetValues(typeof(TreeMode)).Cast<TreeMode>().Where(x => x != preMode).ToList()[0];
            this.treeMode = tempMode;
            this.TreeMode = preMode;
        }
    }
}
