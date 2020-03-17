namespace DynamicDataLibrary
{
    using DynamicData;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;

    public abstract class ItemsServiceBase<TObject, TKey> : IDisposable
    {
        private readonly ISourceCache<TObject, TKey> itemsSource;

        public ItemsServiceBase(Func<TObject, TKey> keySelector)
        {
            this.itemsSource = new SourceCache<TObject, TKey>(keySelector);
            
            this.CleanUpTarget.Add(this.itemsSource);
        }

        public ISourceCache<TObject, TKey> ItemsSource => this.itemsSource;

        public abstract ReadOnlyObservableCollection<TObject> Items { get; }

        protected CompositeDisposable CleanUpTarget { get; } = new CompositeDisposable();

        public virtual void Add(TObject item)
        {   
            this.itemsSource.AddOrUpdate(item);
        }

        public void Add(IEnumerable<TObject> items) => this.itemsSource.Edit(collection =>
        {
            collection.AddOrUpdate(items);
        });

        public IObservable<IChangeSet<TObject, TKey>> Connect(Func<TObject, bool> predicate = default) => this.itemsSource.Connect(predicate);

        public bool Contains(TKey key) => this.TryGetItem(key, out _);

        public IConnectableObservable<IChangeSet<TObject, TKey>> Publish() => this.itemsSource.Connect().Publish();

        public void Remove(TKey key)
        {
            this.itemsSource.Remove(key);
        }

        public void Remove(TObject item)
        {
            this.itemsSource.Remove(item);
        }

        public void Remove(IEnumerable<TObject> items) => this.itemsSource.Edit(collection =>
            {
                collection.Remove(items);
            });

        public void Remove(IEnumerable<TKey> keys) => this.itemsSource.Edit(collection =>
        {
            collection.Remove(keys);
        });

        public virtual bool TryGetItem(TKey key, out TObject item)
        {
            var optional = this.itemsSource.Lookup(key);

            item = optional.HasValue ? optional.Value : default;

            return item != null;
        }

        public bool TryGetItem<T>(TKey key, out T item)
        {
            item = this.TryGetItem(key, out var value) && value is T t ? t : default;

            return item != null;
        }

        public virtual void Dispose()
        {
            this.CleanUpTarget.Dispose();
        }
    }
}