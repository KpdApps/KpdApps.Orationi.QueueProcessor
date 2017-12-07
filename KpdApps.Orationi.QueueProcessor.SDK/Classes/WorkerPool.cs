using System;
using System.Collections.Concurrent;

namespace KpdApps.Orationi.QueueProcessor.SDK.Classes
{
    public class WorkerPool<T>
    {
        public int PoolSize => _objects.Count;

        private ConcurrentBag<T> _objects;
        private Func<T> _objectGenerator;

        public WorkerPool(Func<T> objectGenerator)
        {
            if (objectGenerator == null)
                throw new ArgumentNullException("objectGenerator");

            _objects = new ConcurrentBag<T>();
            _objectGenerator = objectGenerator;
        }

        public T GetObject()
        {
            T item;
            if (_objects.TryTake(out item))
                return item;

            return _objectGenerator();
        }

        public void PutObject(T item)
        {
            _objects.Add(item);
        }
    }
}
