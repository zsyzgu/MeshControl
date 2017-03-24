using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BlockingQueue<T> {
    private readonly Queue<T> _queue = new Queue<T>();
    private bool _stopped = false;
    
    public bool Enqueue(T item)
    {
        if (_stopped)
        {
            return false;
        }
        lock (_queue)
        {
            if (_stopped)
            {
                return false;
            }
            if (_queue.Count <= 100)
            {
                _queue.Enqueue(item);
            } else
            {
                Debug.Log("queue is too long");
                _queue.Clear();
            }
            Monitor.Pulse(_queue);
        }
        return true;
    }

    public T Dequeue()
    {
        if (_stopped)
        {
            return default(T);
        }
        lock (_queue)
        {
            if (_stopped)
            {
                return default(T);
            }
            while (_queue.Count == 0)
            {
                Monitor.Wait(_queue);
                if (_stopped)
                {
                    return default(T);
                }
            }
            return _queue.Dequeue();
        }
    }

    public void Stop()
    {
        if (_stopped)
        {
            return;
        }
        lock (_queue)
        {
            if (_stopped)
            {
                return;
            }
            _stopped = true;
            Monitor.PulseAll(_queue);
        }
    }
}
