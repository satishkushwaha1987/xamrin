using CHSBackOffice.CustomControls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CHSBackOffice.Support
{
    public class LinkedListCacheService<T> where T : CHBackOffice.ApiServices.ChsProxy.PatronTransaction
    {
        public static LinkedListCacheService<T> Instance { get; set; }

        #region Fields
        static LinkedList<T> itemsCache;
        static Func<string,Task<T>> GetPrevItemFromServer;
        static Func<string,Task<T>> GetNextItemFromServer;
        Thread _updateContainerProcessThread;
        #endregion

        #region Properties

        public  T Current
        {
            get;
            set;
        }

        public T Prev
        {
            get { return GetCurrentNode()?.Previous?.Value; }
        }

        public T Next
        {
            get { return GetCurrentNode()?.Next?.Value; }

        }

        public int Offset
        {
            get;
            set;
        }

        public int Index
        {
            get
            {
                return itemsCache.GetIndex(Current);
            }
        }

        public int Count
        {
            get
            {
                return itemsCache.Count;
            }
        }

        public bool CacheLoaded
        {
            get { return itemsCache.Count > 1; }
        }

        #endregion

        #region Methods

        public static LinkedListCacheService<T> Create(Func<string,Task<T>> getPrevItemFromServer, Func<string,Task<T>>getNextItemFromServer)
        {
            Instance = new LinkedListCacheService<T>();
            GetPrevItemFromServer = getPrevItemFromServer;
            GetNextItemFromServer = getNextItemFromServer;
            itemsCache = new LinkedList<T>();
            
            return Instance;
        }

        public void StartUpdateContainer(UpdateJob updateJob)
        {
            _updateContainerProcessThread = new Thread(new ParameterizedThreadStart(UpdateContainerProcess)) { IsBackground = true};
            _updateContainerProcessThread.Start(updateJob);
        }

        public void RequestUpdateContainer(T value, bool left = false)
        {
            try
            {
                if (value == null)
                    return;
                LinkedListNode<T> node;
                if (!itemsCache.TryGetValue(value, out node))
                {
                    itemsCache.Add(value);
                    Current = value;
                    StartUpdateContainer(UpdateJob.Both);
                }
                else
                {
                    if (left)
                    {
                        var prevItem = node.Previous;
                        if (prevItem == null)
                        {
                            StartUpdateContainer(UpdateJob.Prev);
                            Offset = itemsCache.GetOffcetLeft(Current, node.Value);
                            Current = node.Value;
                            
                        }                        
                    }
                    else
                    {
                        var nextItem = node.Next;
                        if (nextItem == null)
                        {
                            StartUpdateContainer(UpdateJob.Next);
                            Offset = itemsCache.GetOffcetRight(Current, node.Value);
                            Current = node.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        public void ForceNext()
        {
            try
            {
                foreach (var item in itemsCache.ForwardEnumerator())
                {
                    if (item.Value == Current)
                    {
                        if (item.Next.Value != null)
                        {
                            Offset = itemsCache.GetOffcetRight(Current, item.Next.Value);
                            Current = item.Next.Value;
                            
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            
        }

        public void ForcePrev()
        {
            try
            {
                foreach (var item in itemsCache.BackEnumerator())
                {
                    if (item.Value == Current)
                    {
                        if (item.Previous.Value != null)
                        {
                            Offset = itemsCache.GetOffcetLeft(Current, item.Previous.Value);
                            Current = item.Previous.Value;
                            
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
            
        }

        public bool HasPrevCache
        {
            get
            {
                return itemsCache.PrevCache(Current) != null;
            }
            
        }

        public bool HasNextCache
        {
            get
            {
                return itemsCache.NextCache(Current) != null;
            }
        }

        public int GetIndex(T item)
        {
            return itemsCache.GetIndex(item);
        }

        public bool Contains(T item)
        {
            return itemsCache.Contains(item);
        }

        public void ResetCache()
        {
            try
            {
                itemsCache.Clear();
                Current = null;
            }
            catch (Exception ex)
            {
                ExceptionProcessor.ProcessException(ex);
            }
        }

        void UpdateContainerProcess(object updateJob)
        {
            UpdateJob update = (UpdateJob)updateJob;
            bool flag = true;
            while (flag)
            {
                try
                {
                    if (update == UpdateJob.Both)
                        GetPrevNextData(Current.Id);
                    else if (update == UpdateJob.Next)
                        GetNextData(Current.Id);
                    else if (update == UpdateJob.Prev)
                        GetPrevData(Current.Id);
                    flag = false;
                }
                catch (Exception ex)
                {
                    ExceptionProcessor.ProcessException(ex);
                }
            }
        }

        void GetPrevNextData(string transactionId)
        {

            Task.Factory.StartNew(async () =>
            {
                var eventArg = new AddItemEventArgs();
                var prevValue = await GetPrevItemFromServer?.Invoke(transactionId);
                if (prevValue != null)
                {
                    eventArg.PrevItem = prevValue;
                    itemsCache.AddFirst(prevValue);
                }

                var nextValue = await GetNextItemFromServer?.Invoke(transactionId);
                if (nextValue != null)
                {
                    eventArg.NextItem = nextValue;
                    itemsCache.Add(nextValue);
                }

                eventArg.Job = UpdateJob.Both;
                OnItemAdded(eventArg);
            });
        }

        void GetPrevData(string transactionId)
        {
            Task.Factory.StartNew(async () =>
            {
                var prevValue = await GetPrevItemFromServer?.Invoke(transactionId);
                if (prevValue != null)
                {
                    itemsCache.AddFirst(prevValue);
                    OnItemAdded(new AddItemEventArgs()
                    {
                        Job = UpdateJob.Prev,
                        PrevItem = prevValue
                    });
                }
                    
            });
        }

        void GetNextData(string transactionId)
        {
            Task.Factory.StartNew(async () =>
            {
                var nextValue = await GetNextItemFromServer?.Invoke(transactionId);
                if (nextValue != null)
                {
                    itemsCache.Add(nextValue);
                    OnItemAdded(new AddItemEventArgs()
                    {
                        Job = UpdateJob.Next,
                        NextItem = nextValue
                    });
                }
            });
        }

        public LinkedListNode<T> GetCurrentNode()
        {
            LinkedListNode<T> value;
            itemsCache.TryGetValue(Current, out value);
            return value;
        }

        protected virtual void OnItemAdded(AddItemEventArgs e)
        {
            EventHandler<AddItemEventArgs> handler = ItemAdded;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<AddItemEventArgs> ItemAdded;
        #endregion

    }

    public enum UpdateJob
    {
        Both,
        Prev,
        Next
    }

    public class AddItemEventArgs : EventArgs
    {
        public UpdateJob Job { get; set; }
        public CHBackOffice.ApiServices.ChsProxy.PatronTransaction PrevItem { get; set; }
        public CHBackOffice.ApiServices.ChsProxy.PatronTransaction NextItem { get; set; }
    }
}
