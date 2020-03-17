
namespace DynamicDataLibrary.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using DynamicDataLibrary.Option;

    public class Document
    {
        public Document()
        {
            this.ItemFilter = new ItemFilter();
            this.FilterMode = new FilterMode();
            this.SortParameters = new SortParameterData();
            this.ChildrenService  = new ChildrenService(FilterMode, ItemFilter, SortParameters);
            this.CleanUpTarget.Add(this.ChildrenService);
        }

        private CompositeDisposable CleanUpTarget { get; } = new CompositeDisposable();

        public int MainId { get; private set; }

        public event EventHandler<object> ChildAdded;

        public FilterMode FilterMode { get; private set; }


        public SortParameterData SortParameters { get; private set; }

        public ItemFilter ItemFilter { get; private set; }

        public ChildrenService ChildrenService { get; private set; }

        public IEnumerable<ViewModelBase> Children => this.ChildrenService.ChildNodes;

        private int id = 0;

        public int GetNextId()
        {
            return id++;
        }
        
        private void InitTest(int categorys, int entitys)
        {
            this.MainId = this.GetNextId();

            using (this.ChildrenService.BulkAdd())
            {
                var data = MyDataInfo.CreateData(categorys, entitys);
                foreach (var categoryName in data)
                {
                    int categoryId = this.GetNextId();
                    var categoryViewModel = new CategoryViewModel(categoryId, this.MainId, "category" + categoryId.ToString(), null, this);
                    this.AddChild(categoryViewModel);

                    foreach (var name in categoryName.Value)
                    {
                        int id = this.GetNextId();
                        var personViewModel = new EntityViewModey(id, categoryId, "entity" + id.ToString(), categoryViewModel, this);
                        this.AddChild(personViewModel);
                    }
                }
            }
        }

        public void InitializeCategoryViewModels(int categorys, int entitys)
        {
            this.InitTest(categorys, entitys); 
            
            int lastId = this.GetNextId();
            Debug.WriteLine("last Id : " + lastId.ToString());
        }
        
        public void AddChild(int key, ViewModelBase viewModelBase)
        {
            this.ChildrenService.Add(key, viewModelBase);

            this.ChildAdded?.Invoke(this, key);
        }

        public void AddChild(ViewModelBase viewModelBase)
        {
            this.ChildrenService.Add(viewModelBase);

            this.ChildAdded?.Invoke(this, viewModelBase.Id);
        }
    }
}
