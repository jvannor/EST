using System;
using System.Collections.Generic;

namespace Mobile.Utilities
{
    public class Tree<T> : HashSet<Tree<T>>
    {
        public T Value { get; set; }

        public Tree<T> AddMultiple(params Tree<T>[] list)
        {
            for (int i=0; i<list.Length; i++)
            {
                Add(list[i]);
            }
            return this;
        }
    }
}
