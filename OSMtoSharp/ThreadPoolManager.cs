using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OSMtoSharp
{
    public delegate void CallbackDelegate(object ThreadContext);
    public class ThreadPoolManager
    {
        CallbackDelegate callback;
        private int counter;
        private object lockCounter;
        public bool Finished { get { return counter <= 0; } }

        public ThreadPoolManager(CallbackDelegate callback)
        {
            lockCounter = new object();
            counter = 0;
            this.callback = callback;
        }

        private void ThreadPoolCallback(object context)
        {
            callback?.Invoke(context);
            lock (lockCounter)
            {
                counter--;
            }
        }

        public void Invoke<T>(IEnumerable<T> collection)
        {
            int worker = Environment.ProcessorCount;

            List<List<T>> splitCollection = new List<List<T>>();

            for (int i = 0; i < worker; i++)
            {
                splitCollection.Add(new List<T>());
            }
            int currentIndex = 0;
            foreach (var item in collection)
            {
                splitCollection[currentIndex].Add(item);
                currentIndex++;
                if (currentIndex >= worker)
                {
                    currentIndex = 0;
                }
            }
            foreach (var item in splitCollection)
            {
                lock (lockCounter)
                {
                    counter++;
                }
                ThreadPool.QueueUserWorkItem(ThreadPoolCallback, item);
            }

            while (!Finished)
            {
                Thread.Sleep(200);
            }
        }

        public void Invoke(IEnumerable collection)
        {
            int worker = Environment.ProcessorCount;

            List<List<object>> splitCollection = new List<List<object>>();

            for (int i = 0; i < worker; i++)
            {
                splitCollection.Add(new List<object>());
            }
            int currentIndex = 0;
            foreach (var item in collection)
            {
                splitCollection[currentIndex].Add(item);
                currentIndex++;
                if (currentIndex >= worker)
                {
                    currentIndex = 0;
                }
            }
            foreach (var item in splitCollection)
            {
                lock (lockCounter)
                {
                    counter++;
                }
                ThreadPool.QueueUserWorkItem(ThreadPoolCallback, item);
            }

            while (!Finished)
            {
                Thread.Sleep(200);
            }
        }

    }
}
