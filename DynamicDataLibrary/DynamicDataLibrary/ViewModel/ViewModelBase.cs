namespace DynamicDataLibrary.ViewModel
{
    using DynamicData;
    using DynamicData.Binding;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        private readonly ObservableCollectionExtended<ViewModelBase> childNodes = new ObservableCollectionExtended<ViewModelBase>();

        private readonly ISubject<int> childrenCountChanged = new Subject<int>();
        
        public ViewModelBase(int id, int parentId, string name, ViewModelBase parent, Document parentDocument) 
        {
            this.Id = id;
            this.ParentId = parentId;
            this.DisplayName = name;
            this.Parent = parent;
            this.ParentDocument = parentDocument;

            this.CleanUpTarget.Add(Disposable.Create(() =>
            {
                this.childrenCountChanged.OnCompleted();
            }));
        }

        public bool IsExpanded { get; set; } = true;
        
        public Document ParentDocument { get; private set; }

        public virtual ViewModelBase Parent { get; private set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public IEnumerable Children => this.ChildNodes;

        public IEnumerable<ViewModelBase> ChildNodes => this.childNodes;

        public string DisplayName { get; set; }

        public int Id { get; set; }

        public int ParentId { get; set; }

        private CompositeDisposable CleanUpTarget { get; } = new CompositeDisposable();
        
        public void InitializeChildNodes(Node<ViewModelBase, int> node)
        {
            var sort = this.ParentDocument.SortParameters.WhenValueChanged(t => t.SelectedItem)
                        .Select(prop => prop.Comparer);

            var modeFilter = this.ParentDocument.ItemFilter.WhenValueChanged(t => t.TreeMode)
                         .Select(ChildrenService.ModeFilter);

            var searchFilter = this.ParentDocument.ItemFilter.WhenValueChanged(t => t.SearchText)
                         .Select(ChildrenService.SearchFilter);

            var childrenLoader = new Lazy<IDisposable>(() => node.Children.Connect()
                 .Transform(item =>
                 {
                     item.Item.InitializeChildNodes(item);
                     return item.Item;
                 })
                .Filter(modeFilter)
                .Filter(searchFilter)
                .Sort(sort)
                .Bind(this.childNodes)
                .Subscribe());

            var shouldExpand = node.IsRoot ? Observable.Return(true)
                : this.Parent.WhenValueChanged(@this => @this.IsExpanded);

            var expander = shouldExpand
              .Where(isExpanded => isExpanded)
              .Take(1)
              .Subscribe(_ =>
              {
                  var x = childrenLoader.Value;
              });

            this.CleanUpTarget.Add(Disposable.Create(() => childrenLoader.Value?.Dispose()));
            this.CleanUpTarget.Add(expander);
        }

        public void Dispose()
        {
            this.childNodes.Clear();
            
            this.CleanUpTarget.Dispose();
        }
    }
}
