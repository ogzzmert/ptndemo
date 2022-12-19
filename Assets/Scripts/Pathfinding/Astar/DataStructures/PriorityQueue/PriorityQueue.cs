using System;
using System.Collections.Generic;
using DataStructures.FibonacciHeap;

namespace DataStructures.PriorityQueue
{
    public class PriorityQueue<TElement, TPriority> : IPriorityQueue<TElement, TPriority>
    
    where TPriority : IComparable<TPriority>
    {
        private readonly FibonacciHeap<TElement, TPriority> heap;
        List<TElement> e;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minPriority">Minimum value of the priority - to be used for comparing.</param>
        public PriorityQueue(TPriority minPriority)
        {
            heap = new FibonacciHeap<TElement, TPriority>(minPriority);
            e = new List<TElement>();
        }

        public void Insert(TElement item, TPriority priority)
        {
            e.Add(item);
            heap.Insert(new FibonacciHeapNode<TElement, TPriority>(item, priority));
        }

        public TElement Top()
        {
            return heap.Min().Data;
        }

        public TElement Pop()
        {
            TElement te = heap.RemoveMin().Data;
            e.Remove(te);
            return te;
        }
        public bool isEmpty() { return heap.IsEmpty(); }
        public List<TElement> elements() { return e; }
    }
}


