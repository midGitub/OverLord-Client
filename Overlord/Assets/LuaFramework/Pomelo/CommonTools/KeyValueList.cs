
using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonToolkit
{
    public class KeyValueList<K, V> : IList, ICollection, IEnumerable
    {
        protected List<K> keyList = new List<K>();
        protected List<V> valList = new List<V>();

        public V this[K key]
        {
            get
            {
                V v;
                if (this.TryGetValue(key, out v))
                    return v;
                throw new KeyNotFoundException();
            }
            set
            {
                int index = this.keyList.IndexOf(key);
                if (index == -1)
                {
                    this.keyList.Add(key);
                    this.valList.Add(value);
                }
                else
                    this.valList[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return this.valList.Count;
            }
        }

        public bool IsFixedSize
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        object IList.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public KeyValueList()
        {
        }

        public KeyValueList(ref List<K> keyListRef, ref List<V> valListRef)
        {
            this.keyList = keyListRef;
            this.valList = valListRef;
        }

        public KeyValueList(KeyValueList<K, V> otherKeyValueList)
        {
            this.AddRange(otherKeyValueList);
        }

        public bool TryGetValue(K key, out V value)
        {
            int index = this.keyList.IndexOf(key);
            if (index == -1)
            {
                value = default(V);
                return false;
            }
            value = this.valList[index];
            return true;
        }

        public int Add(object value)
        {
            throw new NotImplementedException("Use KeyValueList[key] = value or insert");
        }

        public void Clear()
        {
            this.keyList.Clear();
            this.valList.Clear();
        }

        public bool Contains(V value)
        {
            return this.valList.Contains(value);
        }

        public bool ContainsKey(K key)
        {
            return this.keyList.Contains(key);
        }

        public int IndexOf(K key)
        {
            return this.keyList.IndexOf(key);
        }

        public void Insert(int index, K key, V value)
        {
            if (this.keyList.Contains(key))
                throw new Exception("Cannot insert duplicate key.");
            this.keyList.Insert(index, key);
            this.valList.Insert(index, value);
        }

        public void Insert(int index, KeyValuePair<K, V> kvp)
        {
            this.Insert(index, kvp.Key, kvp.Value);
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException("Use Insert(K key, V value) or Insert(KeyValuePair<K, V>)");
        }

        public void Remove(K key)
        {
            int index = this.keyList.IndexOf(key);
            if (index == -1)
                throw new KeyNotFoundException();
            this.keyList.RemoveAt(index);
            this.valList.RemoveAt(index);
        }

        public void Remove(object value)
        {
            throw new NotImplementedException("Use Remove(K key)");
        }

        public void RemoveAt(int index)
        {
            this.keyList.RemoveAt(index);
            this.valList.RemoveAt(index);
        }

        public V GetAt(int index)
        {
            if (index >= this.valList.Count)
                throw new IndexOutOfRangeException();
            return this.valList[index];
        }

        public void SetAt(int index, V value)
        {
            if (index >= this.valList.Count)
                throw new IndexOutOfRangeException();
            this.valList[index] = value;
        }

        public void CopyTo(V[] array, int index)
        {
            this.valList.CopyTo(array, index);
        }

        public void CopyTo(KeyValueList<K, V> otherKeyValueList, int index)
        {
            foreach (KeyValuePair<K, V> keyValuePair in this)
                otherKeyValueList[keyValuePair.Key] = keyValuePair.Value;
        }

        public void AddRange(KeyValueList<K, V> otherKeyValueList)
        {
            otherKeyValueList.CopyTo(this, 0);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (K key in this.keyList)
                yield return new KeyValuePair<K, V>(key, this[key]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (K key in this.keyList)
                yield return (object)new KeyValuePair<K, V>(key, this[key]);
        }

        public override string ToString()
        {
            string[] strArray = new string[this.keyList.Count];
            string format = "{0}:{1}";
            int index = 0;
            foreach (KeyValuePair<K, V> keyValuePair in this)
            {
                strArray[index] = string.Format(format, (object)keyValuePair.Key, (object)keyValuePair.Value);
                ++index;
            }
            return string.Format("[{0}]", (object)string.Join(", ", strArray));
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
