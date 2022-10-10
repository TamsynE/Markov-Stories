using System;
using System.Collections;
using System.Collections.Generic;
using MarkovEntry;

namespace ListSymbolTable
{
    public class ListSymbolTable<K, V> : IEnumerable<K> where K : IComparable<K>
    {
        internal class Node<K, V>
        {
            public K key;
            public V value;
            public Node<K, V> next;

            public Node(K key, V value = default)
            {
                this.key = key;
                this.value = value;
                this.next = null;
            }

            public override string ToString()
            {
                return $" {key}:{value}";
            }
        }

        private Node<K, V> head;
        private int count;

        public ListSymbolTable()
        {
            // create empty list
            head = null;
            this.count = 0;
        }

        public void Add(K key, V value) // adding to the head of the list
        {
            // append item to the end of the list

            Node<K, V> curr = WalkToIndex(key);
            if (curr == null)
            {
                Node<K, V> newNode = new Node<K, V>(key, value);
                newNode.next = head;
                head = newNode;
                this.count++;
            }

            else
            {
                Console.WriteLine($"A node with key '{key} already exists in the symbol table.");
            }
        }

        public int Count
        {
            // number of items in the list
            get { return this.count; }

        }

        public bool Contains(K key)
        {
            Node<K, V> curr = WalkToIndex(key);
            if(curr == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Remove(K key)
        {
            Node<K, V> prev = head;

            if (count > 0 && head.key.Equals(key))
            {
                Node<K, V> toRemove = head;
                head = head.next;
                toRemove.key = default;
                toRemove.value = default;
                toRemove.next = null;
                this.count--;
                return;
            }

            while (prev != null && prev.next != null)
            {
                if (prev.next.key.Equals(key))
                {
                    Node<K, V> toRemove = prev.next;
                    prev.next = toRemove.next;
                    toRemove.key = default;
                    toRemove.value = default;
                    toRemove.next = null;
                    this.count--;
                    return;
                }
            }
        }

        private Node<K, V> WalkToIndex(K key) // searches through the entire list for a node with the specified key
        {
            Node<K, V> curr = head;

            while (curr != null)
            {
                if (key.Equals(curr.key))
                {
                    return curr;
                }
                curr = curr.next;

            }

            return null;
        }

        public void Clear()
        {
            // create new Node<T>, set head and tail to new node, garbage collector collects what's left with no references
            head = null;
            this.count = 0;
        }

        public V this[K key]
        {
            get
            {
                Node<K, V> node = WalkToIndex(key);
                if (node == null)
                {
                    string msg = $"Key ${key} could not be found";
                    throw new KeyNotFoundException(msg);
                }
                return node.value;
            }

            set
            {
                Node<K, V> node = WalkToIndex(key);
                node.value = value;
            }
        }

        public void InOrder()
        {
            Node<K, V> curr = head;

            while (curr.next != null)
            {
                curr = curr.next;
            }
        }

        public static ListSymbolTable<string, MarkovState> LSTTraversal(int N, string[] lines)
        {
            ListSymbolTable<string, MarkovState> db = new ListSymbolTable<string, MarkovState>();

            foreach (string line in lines)
            {
                for (int i = 0; i < line.Length - N; i++)
                {
                    string state = line.Substring(i, N);
                    char next = line[i + N];

                    //construct markov model

                    if (!db.Contains(state))
                    {
                        db.Add(state, new MarkovState(state));
                    }
                    db[state].AddSuffix(next);
                }
            }
            return db;
        }

        // correct
        public override string ToString()
        {
            // count, first 5 items, type of list.

            string info = $"MyList ({typeof(K).Name}): {this.count} items";

            if (this.count > 0)
            {
                Node<K, V> node = head; // head node

                if (Count == 1) // if 1 item in list
                {
                    info = $"MyList ({typeof(K).Name}): {this.count} item ({node})";
                    return info;
                }

                // if more than one in the list

                info += $" ({node}"; // add head node to info string
                node = node.next;

                int endingCount = Math.Min(5, this.count); // returns smaller of the two
                for (int i = 1; i < endingCount; i++)
                {
                    info += $", {node}";
                    node = node.next;

                    if (i == endingCount - 1 && this.count < 6)
                    {
                        info += $")";
                    }
                }

                if (this.count > 5)
                {
                    info += $", ...)";
                }
            }
            return info;
        }

        public IEnumerator<K> GetEnumerator()
        {
            Node<K, V> node = head;
            while (node != null)
            {
                yield return node.key;
                node = node.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

