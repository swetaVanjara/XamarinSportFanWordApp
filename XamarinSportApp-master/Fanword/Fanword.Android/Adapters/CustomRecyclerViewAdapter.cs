using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget; 

namespace Fanword.Android.Adapters
{
    public class CustomRecyclerViewAdapter<T> : RecyclerView.Adapter
    {
        public List<T> AllItems;
        public List<T> Items;
        public event EventHandler<ItemClickEventArgs<T>> ItemClick;

        private Action<RecyclerView.ViewHolder, T, int, int> BindViewHolder;
        private Func<ViewGroup, int, Action<int>, RecyclerView.ViewHolder> CreateViewHolder;
        private Func<T, int, int> ItemViewType;
        public CustomRecyclerViewAdapter(List<T> items, Action<RecyclerView.ViewHolder, T, int, int> bindViewHolder, Func<ViewGroup, int, Action<int>, RecyclerView.ViewHolder> createViewHolder, Func<T, int, int> itemViewType)
        {
            AllItems = items;
            Items = items;
            BindViewHolder = bindViewHolder;
            CreateViewHolder = createViewHolder;
            ItemViewType = itemViewType;
        }

        public override int GetItemViewType(int position)
        {
            return ItemViewType?.Invoke(Items[position], position) ?? 0;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (CreateViewHolder == null)
            {
                throw new NullReferenceException("Null cannot be passed in for the CreateViewHolder function.");
            }

            return CreateViewHolder(parent, viewType, OnClick);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            // header at i;
            if (BindViewHolder == null)
            {
                throw new NullReferenceException("Null cannot be passed in for the BindViewHolder action.");
            }

            BindViewHolder(holder, Items[position], position, GetItemViewType(position));
        }

        public override int ItemCount => Items.Count;

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, new ItemClickEventArgs<T>(Items[position], position));
        }
    }

    public class ItemClickEventArgs<T>
    {
        public T Item { get; set; }
        public int Position { get; set; }

        public ItemClickEventArgs(T item, int position)
        {
            Item = item;
            Position = position;
        }

    }
}