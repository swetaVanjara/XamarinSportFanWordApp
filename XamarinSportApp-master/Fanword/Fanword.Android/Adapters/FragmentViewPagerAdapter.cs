using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace Fanword.Android.Adapters
{
    public class FragmentViewPagerAdapter : global::Android.Support.V13.App.FragmentStatePagerAdapter
    {
        public List<Fragment> Items = new List<Fragment>();

        public FragmentViewPagerAdapter(FragmentManager fm, List<Fragment> fragments) : base(fm)
        {
            Items = fragments;
        }

        public override int Count
        {
            get
            {
                return Items.Count;
            }
        }

        public override Fragment GetItem(int position)
        {
            return Items[position];
        }
        
    }
    
}