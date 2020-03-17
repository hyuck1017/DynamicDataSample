namespace DynamicDataLibrary
{
    using DynamicData;
    using DynamicData.Binding;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using DynamicDataLibrary.Option;
    using DynamicDataLibrary.ViewModel;

    public class ChildrenService : ItemsService<ViewModelBase, int> 
    {
        private static List<int> ids = null;

        // 카테고리뷰모델
        private readonly ObservableCollectionExtended<ViewModelBase> childNodes = new ObservableCollectionExtended<ViewModelBase>();

        private BulkAdder bulkAdder;

        public ChildrenService(FilterMode filterMode, ItemFilter itemFilter, SortParameterData sortParameters)
           : base(item => item.Id)
        {
            var sort = sortParameters.WhenValueChanged(t => t.SelectedItem)
                        .Select(prop => prop.Comparer);

            var modeFilter = itemFilter.WhenValueChanged(t => t.TreeMode)
                         .Select(ModeFilter);

            var searchFilter = itemFilter.WhenValueChanged(t => t.SearchText)
                         .Select(SearchFilter);

            var childNodes = this.Connect()
              .TransformToTree(item => item.ParentId)              
              .Transform(item =>
              {
                  item.Item.InitializeChildNodes(item);
                  return item.Item;
              })
              .DisposeMany()
              .Filter(modeFilter)
              .Filter(searchFilter)
              .Sort(sort)
              .Bind(this.childNodes)
              .Subscribe();

            this.CleanUpTarget.Add(childNodes);
        }
        
        public IEnumerable<ViewModelBase> ChildNodes => this.childNodes;
        
        private bool CanUseBulkAdder => this.bulkAdder != default && !this.bulkAdder.Disposing && !this.bulkAdder.Disposed;

        public static Func<ViewModelBase, bool> ModeFilter(TreeMode treeMode)
        {
            return t => true;
        }

        public static Func<ViewModelBase, bool> SearchFilter(string searchText)
        { 
            return t => true;
        }

        public void Add(int key, ViewModelBase viewModelBase)
        {
            this.Add(viewModelBase);
        }

        public override void Add(ViewModelBase item)
        {
            if (this.CanUseBulkAdder)
            {
                this.bulkAdder.Add(item);
            }
            else
            {
                base.Add(item);                
            }
        }

        public IDisposable BulkAdd()
        {
            if (!this.CanUseBulkAdder)
            {
                this.bulkAdder = new BulkAdder(this);
            }

            return this.bulkAdder;
        }

        public void Clear()
        {
            this.ItemsSource.Clear();
        }

        public void TestRemove()
        {
            this.Remove(ChildrenService.ids);
        }

        public override bool TryGetItem(int key, out ViewModelBase item)
        {
            if (this.CanUseBulkAdder)
            {
                if (this.bulkAdder.TryGetItem(key, out item))
                {
                    return true;
                }
            }

            return base.TryGetItem(key, out item);
        }

        private class BulkAdder : IDisposable
        {
            private Dictionary<int, ViewModelBase> dictionary = new Dictionary<int, ViewModelBase>();
            private ChildrenService childrenService;
            private object locker = new object();

            public BulkAdder(ChildrenService childrenService)
            {
                this.childrenService = childrenService;
            }

            public bool Disposed { get; private set; } = false;

            public bool Disposing { get; private set; } = false;

            public void Add(ViewModelBase item)
            {
                lock (this.locker)
                {
                    if (this.dictionary.ContainsKey(item.Id))
                    {
                        throw new InvalidOperationException();
                    }

                    this.dictionary.Add(item.Id, item);
                }
            }

            public void Dispose()
            {
                this.Disposing = true;

                ChildrenService.ids = this.dictionary.Keys.ToList();
                this.childrenService.Add(this.dictionary.Values);
                this.childrenService = default;

                this.dictionary.Clear();
                this.dictionary = default;

                this.Disposed = true;
            }

            public bool TryGetItem(int key, out ViewModelBase objectViewModel) => this.dictionary.TryGetValue(key, out objectViewModel);
        }
    }
}