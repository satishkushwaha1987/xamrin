using System.Collections.Generic;

namespace CHSBackOffice.Support
{
    public class ItemBroker<T>
    {
        public static ItemBroker<T> Instance = new ItemBroker<T>();

        protected Stack<T> holder = new Stack<T>();

        public void Bind(T obj)
        {
            if (obj != null)
            {
                holder.Push(obj);
            }
        }

        public T Get()
        {
            return holder.Count > 0 ? holder.Peek() : default(T);
        }

        public T Pop()
        {
            return holder.Count > 0 ? holder.Pop() : default(T);
        }

        public void Clear()
        {
            holder = null;
        }
    }
}
