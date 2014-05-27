using System;
using System.Collections;
using System.Collections.Generic;
using Jabber.Net.Server.Utils;

namespace Jabber.Net.Server.Collections
{
    class ReaderWriterLockDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly ReaderWriterLock locker = new ReaderWriterLock();
        private readonly IDictionary<TKey, TValue> dic;


        public ReaderWriterLockDictionary()
        {
            dic = new Dictionary<TKey, TValue>();
        }

        public ReaderWriterLockDictionary(int capacity)
        {
            dic = new Dictionary<TKey, TValue>(capacity);
        }


        public TValue this[TKey key]
        {
            get
            {
                using (ReadLock())
                {
                    return dic[key];
                }
            }
            set
            {
                using (WriteLock())
                {
                    dic[key] = value;
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                using (ReadLock())
                {
                    return new List<TKey>(dic.Keys);
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                using (ReadLock())
                {
                    return new List<TValue>(dic.Values);
                }
            }
        }

        public int Count
        {
            get
            {
                using (ReadLock())
                {
                    return dic.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                using (ReadLock())
                {
                    return dic.IsReadOnly;
                }
            }
        }


        public bool ContainsKey(TKey key)
        {
            using (ReadLock())
            {
                return dic.ContainsKey(key);
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            using (ReadLock())
            {
                return dic.Contains(item);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            using (ReadLock())
            {
                return dic.TryGetValue(key, out value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            using (WriteLock())
            {
                dic.Add(key, value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            using (WriteLock())
            {
                dic.Add(item);
            }
        }

        public bool Remove(TKey key)
        {
            using (WriteLock())
            {
                return dic.Remove(key);
            }
        }

        public bool Remove(TKey key, out TValue value)
        {
            using (WriteLock())
            {
                if (dic.TryGetValue(key, out value))
                {
                    return dic.Remove(key);
                }
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            using (WriteLock())
            {
                return dic.Remove(item);
            }
        }

        public void Clear()
        {
            using (WriteLock())
            {
                dic.Clear();
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using (WriteLock())
            {
                dic.CopyTo(array, arrayIndex);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            using (ReadLock())
            {
                return new Dictionary<TKey, TValue>(dic).GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private IDisposable ReadLock()
        {
            return locker.ReadLock();
        }

        private IDisposable WriteLock()
        {
            return locker.WriteLock();
        }
    }
}
