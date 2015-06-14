using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TraktorProj.Commons
{
    public class Imagelist<T>
    {
        T[] arr = null;
        public Imagelist()
        {
            arr = new T[1];
            // myInitialize(ref arr, arr.Length + 1);
        }
        public void Add(T item)
        {
            arr[arr.Length - 1] = item;
            myInitialize(ref arr, arr.Length + 1);
        }
        

        private void myInitialize(ref T[] arr, int newLength)
        {
            if (arr == null)
            {
                arr = new T[newLength];
            }
            else
            {
                T[] tempArr = new T[newLength];
                //Array.Copy(arr, tempArr, arr.Length);
                for (int i = 0; i < newLength; i++)
                    if (i < arr.Length)
                    {
                        tempArr[i] = arr[i];
                    }
                    else
                    {
                        tempArr[i] = default(T);
                    }
                arr = new T[newLength];
                //Array.Copy(tempArr, arr, arr.Length);
                for (int i = 0; i < newLength; i++)
                {
                    arr[i] = tempArr[i];
                }
            }
        }
    }
}
