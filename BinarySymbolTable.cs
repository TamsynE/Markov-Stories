using System;
using System.Collections.Generic;
using MarkovEntry;
using System.Collections;
using System.Linq;

namespace MyBinarySymbolTable
{
    public class MyTreeTable<K, V> : IEnumerable<K> where K : IComparable<K>
    {
        class Node<K, V>
        {
            public K key;
            public V value;
            public Node<K, V> R;
            public Node<K, V> L;
            public int count;

            public Node(K key, V value = default)
            {
                this.key = key;
                this.value = value;
                this.L = null;
                this.R = null;
                this.count = 1;

            }

            public override string ToString()
            {
                string Lchild = "NULL";
                if (L != null)
                {
                    Lchild = $"{L.key}";
                }

                string Rchild = "NULL";
                if (R != null)
                {
                    Rchild = $"{R.key}";
                }
                return $"{Lchild} <- {key}:{value} -> {Rchild}";
            }
        }
        public MyTreeTable()
        {
            root = null;
        }

        private Node<K, V> root;

        public int Count
        {
            get
            {
                if(root == null)
                {
                    return 0;

                }
                else
                {
                    return root.count;
                }
            }
        }

        public void Add(K key, V value)
        {
            //first node in our tree
            if(root == null)
            {
                Node<K, V> curr = new Node<K, V>(key, value);
                root = curr;
            }
            else
            {
                root = Add(key, value, root);
            }
        }

        public static MyTreeTable<string, MarkovState> BSTMakeTree(int N, string[] lines)
        {
            MyTreeTable<string, MarkovState> db2 = new MyTreeTable<string, MarkovState>();

            foreach (string line in lines)
            {
                for (int i = 0; i < line.Length - N; i++)
                {
                    string state = line.Substring(i, N);
                    char next = line[i + N];

                    //construct markov model

                    if (!db2.Contains(state))
                    {
                        db2.Add(state, new MarkovState(state));
                    }
                    db2[state].AddSuffix(next);
                }
            }
            return db2;
        }

        private Node<K, V> Add(K key, V value, Node<K, V> subroot)
        {
            if (subroot == null)
            {
                return new Node<K, V>(key, value);
            }
            int compare = key.CompareTo(subroot.key); // -1 for <, 0 for =, 1 for >
            
            if (compare == -1) // key was less than current subroot
            {
                subroot.L = Add(key, value, subroot.L);
            }
            else if (compare == +1) // key was greater than curent subroot
            {
                subroot.R = Add(key, value, subroot.R);
            }
            else
            {
                throw new ArgumentException($"A node with key '{key}' already exists in the symbol table");
            }

            subroot.count++;
            return subroot;
        }

        public void Remove(K key)
        {
            root = Remove(key, root);
        }

        private Node<K, V> Remove(K key, Node<K, V> subroot)
        {
            if (subroot == null)
            {
                return null;
            }

            int compare = key.CompareTo(subroot.key);

            if (compare == 0)
            {
                if (subroot.L == null && subroot.R == null) // no children
                {
                    return null;
                }
                if (subroot.L == null && subroot.R != null) // one child right
                {
                    return subroot.R;
                }
                if (subroot.R == null && subroot.L != null) // one child left
                {
                    return subroot.L;
                }

                Node<K, V> replacement = Min(subroot.R); // successor
                subroot.key = replacement.key;
                subroot.value = replacement.value;
                Remove(replacement.key, subroot.R);
            }

            else if (compare < 0) // = -1
            {
                subroot.L = Remove(key, subroot.L);
            }
            else if (compare > 0) // +1
            {
                subroot.L = Remove(key, subroot.R);
            }

            return subroot;
        }

        public V this[K key]
        {
            get
            {
                Node<K, V> node = WalkToNode(key, root);
                if(node ==null)
                {
                    string msg = $"Key ${key} could not be found in the symbol table";
                    throw new KeyNotFoundException(msg);
                }
                return node.value;
            }

            set
            {
                Node<K, V> node = WalkToNode(key, root);
                if(node == null)
                {
                    Add(key, value);
                }
                else
                {
                    node.value = value;
                }
            }
        }

        public K Max()
        {
            return Max(root).key;
        }

        private Node<K, V> Max(Node<K, V> subroot)
        {
            while (subroot.R != null)
            {
                subroot = subroot.R;
            }

            return subroot;
        }

        public K Min()
        {
            return Min(root).key;
        }

        private Node<K, V> Min(Node<K, V> subroot)
        {
            //if(subroot.R != null)
            //{
            //    return Max(subroot.R);
            //}
            //else
            //{
            //    return subroot.key;
            //}

            while (subroot.L != null)
            {
                subroot = subroot.L;
            }

            return subroot;
        }

        private Node<K, V> WalkToNode(K nodeKey, Node<K, V> subroot)
        {
            if (subroot == null)
            {
                return null;
            }

            int compare = nodeKey.CompareTo(subroot.key);
            if(compare == -1)
            {
                return WalkToNode(nodeKey, subroot.L);
            }
            else if(compare == +1)
            {
                return WalkToNode(nodeKey, subroot.R);
            }
            else
            {
                return subroot;
            }
        }

        public K Predecessor(K fromKey)
        {
            Node<K, V> curr = WalkToNode(fromKey, root);
            if(curr == null)
            {
                throw new ArgumentException($"Error: this key '{fromKey}' does not exist");
            }
            if (curr.L == null)
            {
                throw new InvalidOperationException($"Error: '{fromKey}' does not have a predecessor");
            }
            //if (curr.L != null && curr.L.R == null) // left child has no child
            //{
            //    return curr.L.key;
            //}
            else
            {
                return Max(curr.L).key; // contains commented out above
            }
        }

        public K Successor(K fromKey)
        {
            Node<K, V> curr = WalkToNode(fromKey, root);
            if (curr == null)
            {
                throw new ArgumentException($"Error: this key '{fromKey}' does not exist");
            }
            if (curr.R == null)
            {
                throw new InvalidOperationException($"Error: '{fromKey}' does not have a successor");
            }
            //if (curr.R != null && curr.R.L == null) // right child has no child
            //{
            //    return curr.R.key;
            //}
            else
            {
                return Max(curr.R).key; // contains commented out above
            }
        }

        public MyTreeTable<string, MarkovState> BSTTraversal()
        {
            MyTreeTable<string, MarkovState> db2 = new MyTreeTable<string, MarkovState>();
            BSTTraversal(root, db2);
            return db2;
        }

        private void BSTTraversal(Node<K, V> subroot, MyTreeTable<string, MarkovState> db2)
        {
            if(subroot != null)
            {
                Object nK = subroot.key;
                String k = nK.ToString();
                MarkovState v = new MarkovState(k);
                BSTTraversal(subroot.L, db2);
                db2.Add(k, v);
                BSTTraversal(subroot.R, db2);
            }
        }

        private IEnumerable<K> GetEnumerator(Node<K, V> subroot) // now recursive!!!
        {
            if(subroot != null)
            {
                foreach (K key in GetEnumerator(subroot.L))
                {
                    yield return key;
                }
                yield return subroot.key;

                foreach (K key in GetEnumerator(subroot.R))
                {
                    yield return key;
                }
            }
        }

        public IEnumerator<K> GetEnumerator()
        {
            foreach(K key in GetEnumerator(root))
            {
                yield return key;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
