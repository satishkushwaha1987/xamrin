using System.Collections;
using System.Collections.Generic;

namespace CHSBackOffice.Support
{
    public class LinkedListNode<T>
    {
        #region .CTOR

        public LinkedListNode(T value)
        {
            Value = value;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Node value
        /// </summary>
        public T Value { get; internal set; }

        /// <summary>
        /// Previous value
        /// </summary>
        public LinkedListNode<T> Previous { get; internal set; }

        /// <summary>
        /// Next value
        /// </summary>
        public LinkedListNode<T> Next { get; internal set; }
        #endregion
    }

    public class LinkedList<T> : ICollection<T>
    {
        #region Properties

        public LinkedListNode<T> Head { get; private set; }
        public LinkedListNode<T> Tail { get; private set; }
        public int Count { get; private set; }
        public bool IsReadOnly { get { return false; } }

        #endregion

        #region Methods

        public void Add(T item)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(item);
            if (Head == null)
                Head = node;
            else
            {
                Tail.Next = node;
                node.Previous = Tail;
            }
            Tail = node;
            Count++;
        }

        public void AddFirst(T item)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(item);
            LinkedListNode<T> temp = Head;
            node.Next = temp;
            Head = node;
            if (Count == 0)
                Tail = Head;
            else
                temp.Previous = node;
            Count++;
        }

        public bool Remove(T item)
        {
            LinkedListNode<T> current = Head;
            while (current != null)
            {
                if (current.Value.Equals(item))
                {
                    break;
                }
                current = current.Next;
            }
            if (current != null)
            {
                if (current.Next != null)
                {
                    current.Next.Previous = current.Previous;
                }
                else
                {
                    Tail = current.Previous;
                }
                if (current.Previous != null)
                {
                    current.Previous.Next = current.Next;
                }
                else
                {
                    Head = current.Next;
                }
                Count--;
                return true;
            }
            return false;
        }

        public void Clear()
        {
            Head = null;
            Tail = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            LinkedListNode<T> current = Head;
            while (current != null)
            {
                if (current.Value.Equals(item))
                    return true;
                current = current.Next;
            }
            return false;
        }

        public bool TryGetValue(T item, out LinkedListNode<T> value)
        {
            LinkedListNode<T> current = Head;
            while (current != null)
            {
                if (current.Value.Equals(item))
                {
                    value = current;
                    return true;
                }
                current = current.Next;
            }
            value = null;
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            LinkedListNode<T> current = Head;
            while (current != null)
            {
                array[arrayIndex++] = current.Value;
                current = current.Next;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            LinkedListNode<T> current = Head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        public IEnumerable<LinkedListNode<T>> BackEnumerator()
        {
            LinkedListNode<T> current = Tail;
            while (current != null)
            {
                yield return current;
                current = current.Previous;
            }
        }

        public IEnumerable<LinkedListNode<T>> ForwardEnumerator()
        {
            LinkedListNode<T> current = Head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        public LinkedListNode<T> PrevCache(T item)
        {
            LinkedListNode<T> node;
            TryGetValue(item, out node);
            return node?.Previous?.Previous;
        }

        public LinkedListNode<T> NextCache(T item)
        {
            LinkedListNode<T> node;
            TryGetValue(item, out node);
            return node?.Next?.Next;
        }

        public int GetOffcetLeft(T item,  T prev)
        {
            LinkedListNode<T> current;
            TryGetValue(item, out current);

            int offset = 0;
            while (current != null)
            {
                if (current.Value.Equals(prev))
                {
                    return offset;
                }
                current = current.Previous;
                offset++;
            }
            return -1;
        }

        public int GetOffcetRight(T item, T next)
        {
            LinkedListNode<T> current;
            TryGetValue(item, out current);

            int offset = 0;
            while (current != null)
            {
                if (current.Value.Equals(next))
                {
                    return offset;
                }
                current = current.Next;
                offset++;
            }
            return -1;
        }

        public int GetIndex(T item)
        {
            int index = 0;
            LinkedListNode<T> current = Tail;
            while (current != null)
            {
                if (current.Value.Equals(item))
                {
                    return index;
                }
                current = current.Previous;
                index++;
            }
            return -1;
        }
        #endregion
    }
}
