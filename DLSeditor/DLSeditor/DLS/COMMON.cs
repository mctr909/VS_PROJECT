using System;
using System.Collections.Generic;
using System.Text;

namespace DLS
{
	public class LIST<T>
	{
		private Dictionary<int, T> mList;

		protected LIST()
		{
			mList = new Dictionary<int, T>();
		}

		public void Add(T obj)
		{
			mList.Add(mList.Count, obj);
		}

		public void Del(int index)
		{
			if (!mList.ContainsKey(index)) return;

			mList.Remove(index);

			var temp = new Dictionary<int, T>();
			foreach (var o in mList.Values)
			{
				temp.Add(temp.Count, o);
			}

			mList = temp;
		}

		public T this[int index]
		{
			get
			{
				if (!mList.ContainsKey(index)) throw new Exception();
				return mList[index];
			}
			set
			{
				if (!mList.ContainsKey(index)) throw new Exception();
				mList[index] = value;
			}
		}

		public Dictionary<int, T>.ValueCollection List
		{
			get { return mList.Values; }
		}
	}
}
