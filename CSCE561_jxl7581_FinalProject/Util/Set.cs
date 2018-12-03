using System;
using System.Collections.Generic;
using System.Collections;

namespace CSCE561_jxl7581_FinalProject.Util
{
    public class Set<T> : List<T>
    {
        /// <summary>
        /// Creates a new set.
        /// </summary>
        public Set() : base()
        {
        }

        /// <summary>
        /// Creates a new set initialized with ICollection object
        /// </summary>
        /// <param name="collection">
        /// ICollection object to initialize the set object
        /// </param>
        public Set(ICollection<T> collection) : base(collection)
        {
        }

        /// <summary>
        /// Creates a new set initialized with a specific capacity.
        /// </summary>
        /// <param name="capacity">
        /// value to set the capacity of the set object
        /// </param>
        public Set(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Adds an element to the set.
        /// </summary>
        /// <param name="item">
        /// The object to be added.
        /// </param>
        /// <returns>
        /// True if the object was added, false otherwise.
        /// </returns>
        public new virtual bool Add(T item)
        {
            if (this.Contains(item))
            {
                return false;
            }
            else
            {
                base.Add(item);
                return true;
            }
        }

        /// <summary>
        /// Adds all the elements contained in the specified collection.
        /// </summary>
        /// <param name="collection">
        /// The collection used to extract the elements that will be added.
        /// </param>
        /// <returns>
        /// Returns true if all the elements were successfuly added. Otherwise returns false.
        /// </returns>
        public virtual bool AddAll(ICollection<T> collection)
        {
            bool result = false;
            if (collection != null)
            {
                foreach (T item in collection)
                {
                    result = this.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Verifies that all the elements of the specified collection are contained into the current collection. 
        /// </summary>
        /// <param name="collection">
        /// The collection used to extract the elements that will be verified.
        /// </param>
        /// <returns>
        /// True if the collection contains all the given elements.
        /// </returns>
        public virtual bool ContainsAll(ICollection<T> collection)
        {
            bool result = false;
            foreach (T item in collection)
            {
                if (!(result = this.Contains(item)))
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Verifies if the collection is empty.
        /// </summary>
        /// <returns>
        /// True if the collection is empty, false otherwise.
        /// </returns>
        public virtual bool IsEmpty()
        {
            return (this.Count == 0);
        }

        /// <summary>
        /// Removes an element from the set.
        /// </summary>
        /// <param name="elementToRemove">
        /// The element to be removed.
        /// </param>
        /// <returns>
        /// True if the element was removed.
        /// </returns>
        public new virtual bool Remove(T elementToRemove)
        {
            bool result = false;
            if (this.Contains(elementToRemove))
            {
                result = true;
            }
            base.Remove(elementToRemove);
            return result;
        }

        /// <summary>
        /// Removes all the elements contained in the specified collection.
        /// </summary>
        /// <param name="collection">
        /// The collection used to extract the elements that will be removed.
        /// </param>
        /// <returns>
        /// True if all the elements were successfuly removed, false otherwise.
        /// </returns>
        public virtual bool RemoveAll(ICollection<T> collection)
        {
            bool result = false;
            foreach (T item in collection)
            {
                if ((!result) && (this.Contains(item)))
                {
                    result = true;
                }
                this.Remove(item);
            }
            return result;
        }

        /// <summary>
        /// Removes all the elements that aren't contained in the specified collection.
        /// </summary>
        /// <param name="collection">
        /// The collection used to verify the elements that will be retained.
        /// </param>
        /// <returns>
        /// True if all the elements were successfully removed, false otherwise.
        /// </returns>
        public virtual bool RetainAll(ICollection<T> collection)
        {
            bool result = false;

            IEnumerator<T> enumerator = collection.GetEnumerator();
            Set<T> currentSet = (Set<T>)collection;
            while (enumerator.MoveNext())
                if (!currentSet.Contains(enumerator.Current))
                {
                    result = this.Remove(enumerator.Current);
                    enumerator = this.GetEnumerator();
                }
            return result;
        }

        /// <summary>
        /// Obtains an array containing all the elements in the collection.
        /// </summary>
        /// <param name="objects">
        /// The array into which the elements of the collection will be stored.
        /// </param>
        /// <returns>
        /// The array containing all the elements of the collection.
        /// </returns>
        public virtual T[] ToArray(T[] objects)
        {
            int index = 0;
            foreach (T item in this)
            {
                objects[index++] = item;
            }
            return objects;
        }
    }

    public class SortedSet<T> : Set<T>
    {
        /// <summary>
        /// Creates a new SortedSet.
        /// </summary>
        public SortedSet() : base()
        {
        }

        /// <summary>
        /// Create a new SortedSet with a specific collection.
        /// </summary>
        /// <param name="collection">
        /// The collection used to iniciatilize the SortedSetSupport
        /// </param>
        public SortedSet(ICollection<T> collection) : base(collection)
        {
        }

        /// <summary>
        /// Returns the first element from the set.
        /// </summary>
        /// <returns>
        /// Returns the first element from the set.
        /// </returns>
        public virtual T First()
        {
            IEnumerator<T> enumerator = this.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current;
        }

        /// <summary>
        /// Returns a view of elements until the specified element.
        /// </summary>
        /// <returns>
        /// Returns a sorted set of elements that are strictly less than the specified element.
        /// </returns>
        //public virtual SortedSet<T> HeadSet(T toElement)
        //{
        //    SortedSet<T> sortedSet = new SortedSet<T>();
        //    IEnumerator<T> enumerator = this.GetEnumerator();
        //    while((enumerator.MoveNext() && ((enumerator.Current.ToString().CompareTo(toElement.ToString())) < 0)))
        //    {
        //        sortedSet.Add(enumerator.Current);
        //    }
        //    return sortedSet;
        //}

        /// <summary>
        /// Returns the last element of the set.
        /// </summary>
        /// <returns>Returns the last element from the set.</returns>
        public virtual T Last()
        {
            IEnumerator<T> enumerator = this.GetEnumerator();
            T element = default(T);
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null)
                {
                    element = enumerator.Current;
                }
            }
            return element;
        }

        /// <summary>
        /// Returns a view of elements from the specified element.
        /// </summary>
        /// <returns>
        /// Returns a sorted set of elements that are greater or equal to the specified element.
        /// </returns>
        //public virtual SortedSet<T> TailSet(T fromElement)
        //{
        //    SortedSet<T> sortedSet = new SortedSet<T>();
        //    IEnumerator<T> enumerator = this.GetEnumerator();
        //    while((enumerator.MoveNext() && (!(enumerator.Current.ToString().CompareTo(fromElement.ToString())) < 0)))
        //    {
        //        sortedSet.Add(enumerator.Current);
        //    }
        //    return sortedSet;
        //}

        /// <summary>
        /// Returns a view of elements between the specified elements.
        /// </summary>
        /// <returns>
        /// Returns a sorted set of elements from the first specified element to the second specified element.
        /// </returns>
        //public virtual SortedSet<T> SubSet(T fromElement, T toElement)
        //{
        //    SortedSet<T> sortedSet = new SortedSet<T>();
        //    IEnumerator<T> enumerator = this.GetEnumerator();
        //    while((enumerator.MoveNext() && ((!(enumerator.Current.ToString().CompareTo(fromElement.ToString())) < 0))) && (!(enumerator.Current.ToString().CompareTo(toElement.ToString())) > 0))
        //    {
        //        sortedSet.Add(enumerator.Current);
        //    }
        //    return sortedSet;
        //}
    }

    public class TreeSet<T> : SortedSet<T>
    {
        /// <summary>
        /// Creates a new TreeSet.
        /// </summary>
        public TreeSet()
        {
        }

        /// <summary>
        /// Create a new TreeSet with a specific collection.
        /// </summary>
        /// <param name="collection">
        /// The collection used to initialize the TreeSet
        /// </param>
        public TreeSet(ICollection<T> collection) : base(collection)
        {
        }

        /// <summary>
        /// Creates a copy of the TreeSet.
        /// </summary>
        /// <returns>A copy of the TreeSet.</returns>
        public virtual object TreeSetClone()
        {
            TreeSet<T> internalClone = new TreeSet<T>();
            internalClone.AddAll(this);
            return internalClone;
        }

        /// <summary>
        /// Retrieves the number of elements contained in the set.
        /// </summary>
        /// <returns>
        /// An integer value that represent the number of element in the set.
        /// </returns>
        public virtual int Size()
        {
            return this.Count;
        }
    }

    public class HashSet<T> : Set<T>
    {
        /// <summary>
        /// Creates a new hash set collection.
        /// </summary>
        public HashSet()
        {
        }

        /// <summary>
        /// Creates a new hash set collection.
        /// </summary>
        /// <param name="collection">
        /// The collection to initialize the hash set with.
        /// </param>
        public HashSet(ICollection<T> collection)
        {
            this.AddRange(collection);
        }

        /// <summary>
        /// Creates a new hash set with the given capacity.
        /// </summary>
        /// <param name="capacity">
        /// The initial capacity of the hash set.
        /// </param>
        public HashSet(int capacity)
        {
            this.Capacity = capacity;
        }

        /// <summary>
        /// Creates a new hash set with the given capacity.
        /// </summary>
        /// <param name="capacity">
        /// The initial capacity of the hash set.
        /// </param>
        /// <param name="loadFactor">
        /// The load factor of the hash set.
        /// </param>
        public HashSet(int capacity, float loadFactor)
        {
            this.Capacity = capacity;
        }

        /// <summary>
        /// Creates a copy of the HashSet.
        /// </summary>
        /// <returns> A copy of the HashSet.</returns>
        public virtual object HashSetClone()
        {
            return MemberwiseClone();
        }

        public static Set<IDictionaryEnumerator> EntrySet(IDictionary hashtable)
        {
            IDictionaryEnumerator hashEnumerator = hashtable.GetEnumerator();
            Set<IDictionaryEnumerator> hashSet = new Set<IDictionaryEnumerator>();
            while (hashEnumerator.MoveNext())
            {
                Hashtable hash = new Hashtable();
                hash.Add(hashEnumerator.Key, hashEnumerator.Value);
                hashSet.Add(hash.GetEnumerator());
            }
            return hashSet;
        }
    }
}
