using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GBRead.Base
{
	public delegate void ChangedEventHandler(object sender, EventArgs e);
	public class NotifyList<T> : IList
	{
		private List<T> privList;

		public event ChangedEventHandler Changed;

		public NotifyList()
		{
			privList = new List<T>();
		}

		protected virtual void OnChanged(EventArgs e)
		{
			if (Changed != null)
				Changed(this, e);
		}

		public int IndexOf(T item)
		{
			return privList.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			privList.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		public void Add(T item)
		{
			privList.Add(item);
			OnChanged(EventArgs.Empty);
		}

		public void Clear()
		{
			privList.Clear();
			OnChanged(EventArgs.Empty);
		}

		public T Find(Predicate<T> match)
		{
			return privList.Find(match);
		}

		public void Sort()
		{
			privList.Sort();
			OnChanged(EventArgs.Empty);
		}

		public void Sort(Comparison<T> comparison)
		{
			privList.Sort(comparison);
			OnChanged(EventArgs.Empty);
		}

		public void Sort(IComparer<T> comparer)
		{
			privList.Sort(comparer);
			OnChanged(EventArgs.Empty);
		}

		public bool Contains(T item)
		{
			return privList.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			privList.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get 
			{
				return privList.Count;
			}
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(T item)
		{
			bool returned = privList.Remove(item);
			OnChanged(EventArgs.Empty);
			return returned;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return privList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return privList.GetEnumerator();
		}

		public int Add(object value)
		{
			if (value is T)
			{
				privList.Add((T)value);
				OnChanged(EventArgs.Empty);
				return 1;
			}
			else return - 1;
		}

		public bool Contains(object value)
		{
			if (value is T) return privList.Contains((T)value);
			else return false;
		}

		public int IndexOf(object value)
		{
			if (value is T)
			{
				return privList.IndexOf((T)value);
			}
			else
			{
				return -1;
			}
		}

		public void Insert(int index, object value)
		{
			if (value is T)
			{
				privList.Insert(index, (T)value);
			}
		}

		public bool IsFixedSize
		{
			get { return false; }
		}

		public void Remove(object value)
		{
			if (value is T) privList.Remove((T)value);
			OnChanged(EventArgs.Empty);
		}

		object IList.this[int index]
		{
			get
			{
				return privList[index];
			}
			set
			{
				if (value is T)
				{
					privList[index] = (T)value;
					OnChanged(EventArgs.Empty);
				}
			}
		}

		#region Unimplemented methods
		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		public bool IsSynchronized
		{
			get { throw new NotImplementedException(); }
		}

		public object SyncRoot
		{
			get { throw new NotImplementedException(); }
		}
		#endregion
	}
}
