namespace DynamicDataLibrary
{
    using DynamicData;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Reactive.Linq;

    public class ItemsService<TObject, TKey> : ItemsServiceBase<TObject, TKey>
        where TObject : INotifyPropertyChanged
    {
        private ReadOnlyObservableCollection<TObject> items;

        public ItemsService(Func<TObject, TKey> keySelector)
            : base(keySelector)
        {
        }

        public override ReadOnlyObservableCollection<TObject> Items
        {
            get
            {
                if (this.items == default)
                {
                    var itemsBinder = this.Connect()
                        .ObserveOnDispatcher()
                        .Bind(out this.items)
                        .Subscribe();

                    this.CleanUpTarget.Add(itemsBinder);
                }

                return this.items;
            }
        }
    }
}