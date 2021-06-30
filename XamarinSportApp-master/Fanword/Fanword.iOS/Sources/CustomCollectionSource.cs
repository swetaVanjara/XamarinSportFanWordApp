using System;
using System.Collections.Generic;
using Foundation;
using Mobile.Extensions.iOS.Sources;
using UIKit;

namespace Fanword.iOS
{
	public class CustomCollectionSource<T> : UICollectionViewSource
	{
		public CustomCollectionSource ()
		{
		}

		public List<string> Sections = new List<string> ();
		public List<T> items;
		public List<T> allItems;
		Func<UICollectionView, NSIndexPath, T, UICollectionViewCell> getCell;
		public EventHandler<UIScrollView> OnScroll;
		public EventHandler<ScrollEventArgs> OnScrollAnimationEnded;
		public EventHandler<UIScrollView> OnDecelerationEnded;
		public delegate void ItemClickEvent (object sender, T e);
		public event ItemClickEvent ItemClick;
		public UIColor NoContentColor;
		public string NoContentText;
		public bool NoContentEnabled;
		public Func<UITableView, NSIndexPath, UITableViewRowAction []> EditActions;
		public CustomCollectionSource (List<T> objects, Func<UICollectionView, NSIndexPath, T, UICollectionViewCell> getCell)
		{
			items = objects;
			allItems = objects;
			this.getCell = getCell;
			NoContentColor = UIColor.FromRGB (180, 180, 180);
			NoContentText = "No content here";
			NoContentEnabled = true;
		}

		public override void Scrolled (UIScrollView scrollView)
		{
			if (OnScroll != null)
			{
				OnScroll (this, scrollView);
			}
		}

		public override nint GetItemsCount (UICollectionView collectionView, nint section)
		{
			return items.Count;
		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			if (getCell == null)
			{
				throw new NullReferenceException ("GetCell cannot be null. You must pass in a valid Func when initializing the source");
			}

			var item = items [indexPath.Row];
			return getCell (collectionView, indexPath, item);

		}

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			collectionView.DeselectItem (indexPath, true);
			if (items.Count == 0)
			{
				return;
			}
			var item = items [indexPath.Row];
			if (ItemClick != null)
			{
				ItemClick (this, item);
			}
		}


		/*
		public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath, Notification item)
		{
			var cell = collectionView.DequeueReusableCell("YourKey", indexPath);
			return cell;
		}		*/
	}
}
