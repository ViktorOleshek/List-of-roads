using System;
using System.Collections.Generic;

namespace Kursova
{
  public class MergeSort<T> where T : IComparable<T>
  {
    public MergeSort()
    {

    }

    public void Sort(T [] arr)
    {
      MergeSortInternal(arr, 0, arr.Length - 1);
    }
    private void MergeSortInternal(T [] arr, int left, int right)
    {
      if (left < right)
      {
        int mid = (left + right) / 2;

        MergeSortInternal(arr, left, mid);
        MergeSortInternal(arr, mid + 1, right);

        Merge(arr, left, mid, right);
      }
    }

    private void Merge(T [] arr, int left, int mid, int right)
    {
      int sizeLeftArray = mid - left + 1;
      int sizeRightArray = right - mid;

      T [] leftArray = new T [sizeLeftArray];
      T [] rightArray = new T [sizeRightArray];

      Array.Copy(arr, left, leftArray, 0, sizeLeftArray);
      Array.Copy(arr, mid + 1, rightArray, 0, sizeRightArray);

      int i = 0, j = 0;
      int k = left;

      while (i < sizeLeftArray && j < sizeRightArray)
      {
        arr [k++] = (leftArray [i].CompareTo(rightArray [j]) >= 0) ? leftArray [i++] : rightArray [j++];
      }

      while (i < sizeLeftArray)
      {
        arr [k++] = leftArray [i++];
      }

      while (j < sizeRightArray)
      {
        arr [k++] = rightArray [j++];
      }
    }
  }
}
