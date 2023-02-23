
using System;
using System.Collections.Generic;

namespace SortHelper
{
	public enum SortType
	{
		ASCENDING,
		DESCENDING,

	}
	public static class Common
	{
		public static void Swap<T>(ref T nData1, ref T nData2)
		{
			T nTemp = nData1;
			nData1 = nData2;
			nData2 = nTemp;
		}
	}

	/// <summary>
	/// 정렬되지 않은 리스트를 오름차순, 내림차순으로 정렬할때 사용할 것
	/// </summary>
	public static class QuickSort
	{
		public static T[] Sort<T>(T[] dataArray, Comparison<T> comparison, SortType sortType)
		{
			return Sort(dataArray, 0, dataArray.Length - 1, comparison, sortType);
		}
		public static T[] Sort<T>(T[] dataArray, int left, int right, Comparison<T> comparison, SortType sortType)
		{
			IComparer<T> comparer = Comparer<T>.Create(comparison);
			InnerSort(ref dataArray, left, right, comparer, sortType);
			return dataArray;
		}

		public static void InnerSort<T>(ref T[] dataArray, int left, int right, IComparer<T> comparer, SortType sortType)
		{
			if (left >= right)
			{
				return;
			}
			int pivot = Quick(ref dataArray, left, right, comparer, sortType);

			InnerSort(ref dataArray, left, pivot - 1, comparer, sortType);

			InnerSort(ref dataArray, pivot + 1, right, comparer, sortType);
		}

		private static int Quick<T>(ref T[] dataArray, int left, int right, IComparer<T> comparea, SortType sortType)
		{
			int pivot = right;
			int low = left, high = pivot - 1;

			while (low <= high)
			{
				switch (sortType)
				{
					case SortType.ASCENDING:
						for (; low <= high && comparea.Compare(dataArray[low], dataArray[pivot]) != 1; low++) ;
						for (; high >= low && comparea.Compare(dataArray[high], dataArray[pivot]) != -1; high--) ;
						break;
					case SortType.DESCENDING:
						for (; low <= high && comparea.Compare(dataArray[low], dataArray[pivot]) != -1; low++) ;
						for (; high >= low && comparea.Compare(dataArray[high], dataArray[pivot]) != 1; high--) ;
						break;
				}

				if (low <= high)
				{
					Common.Swap(ref dataArray[low], ref dataArray[high]);
				}

			}

			Common.Swap(ref dataArray[low], ref dataArray[pivot]);
			return low;
		}
	}

	/// <summary>
	/// 배열이 이미 정렬 되어있을 경우에 사용
	/// </summary>

	public static class HeapSort
	{

		public static void Sort<T>(T[] dataArray, Comparison<T> comparison)
		{
			IComparer<T> comparer = Comparer<T>.Create(comparison);
			Sort(dataArray, comparer);
		}

		public static void Sort<T>(T[] dataArray, IComparer<T> comparea)
		{


			for (int i = (dataArray.Length - 1) / 2; i >= 0; --i)
			{
				CalcHeap(dataArray, i, dataArray.Length);
			}

			for (int i = (dataArray.Length - 1); i >= 0; --i)
			{
				Common.Swap(ref dataArray[i], ref dataArray[0]);
				CalcHeap(dataArray, 0, i);
			}

			//Nested Method
			void CalcHeap(T[] nArrData, int nRoot, int nMax)
			{
				while (nRoot < nMax)
				{
					int nChild = nRoot * 2 + 1;
					if (nChild + 1 < nMax && comparea.Compare(nArrData[nChild], nArrData[nChild + 1]) == -1)
					{
						++nChild;
					}
					if (nChild < nMax && comparea.Compare(nArrData[nRoot], nArrData[nChild]) == -1)
					{
						Common.Swap(ref nArrData[nRoot], ref nArrData[nChild]);
						nRoot = nChild;
					}
					else
					{
						break;
					}
				}
			}


		}
	}
}






