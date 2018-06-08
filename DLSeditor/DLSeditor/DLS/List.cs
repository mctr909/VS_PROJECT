using System;
using System.Collections.Generic;

namespace DLS
{
	public class List<T>
	{
		private Dictionary<int, T> mList;

		protected List()
		{
			mList = new Dictionary<int, T>();
		}

		public void Add(T obj)
		{
			mList.Add(mList.Count, obj);
		}

		public void Del(int index)
		{
			if (!mList.ContainsKey(index))
				return;

			mList.Remove(index);

			var temp = new Dictionary<int, T>();
			foreach (var o in mList.Values) {
				temp.Add(temp.Count, o);
			}

			mList = temp;
		}

		public T this[int index]
		{
			get {
				if (!mList.ContainsKey(index)) {
					throw new Exception();
				}
				return mList[index];
			}
			set {
				if (!mList.ContainsKey(index)) {
					throw new Exception();
				}
				mList[index] = value;
			}
		}

		public Dictionary<int, T>.ValueCollection Values
		{
			get {
				return mList.Values;
			}
		}

		public int Count
		{
			get {
				return mList.Count;
			}
		}
	}
}