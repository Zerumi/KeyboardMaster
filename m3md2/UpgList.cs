// This code & software is licensed under the Creative Commons license. 
// You can use & improve this code by keeping this comments
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m3md2
{
    public class UpgList<T> : List<T>
    {
        public event EventHandler OnAdd;
        public event EventHandler OnRemove;

        public new void Add(T item) // "new" to avoid compiler-warnings, because we're hiding a method from base-class
        {
            OnAdd?.Invoke(this, null);
            base.Add(item);
        }

        public new void Remove(T item)
        {
            OnRemove?.Invoke(this, null);
            base.Remove(item);
        }
    }
}
