namespace Hierarchy
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;

    public class Hierarchy<T> : IHierarchy<T>
    {

        private class Node
        {
            public T Value { get; set; }
            public Node Parent { get; set; }
            public List<Node> Children { get; set; }

            public Node(T value)
            {
                this.Value = value;
                this.Children = new List<Node>();
            }
        }

        private Node root;
        private Dictionary<T, Node> nodesByValue;

        public Hierarchy(T value)
        {
            this.root = new Node(value);
            this.nodesByValue = new Dictionary<T, Node>();
            this.nodesByValue.Add(value, this.root);
        }

        public int Count => this.nodesByValue.Count;

        public void Add(T element, T child)
        {
            if (!nodesByValue.ContainsKey(element) || nodesByValue.ContainsKey(child))
            {
                throw new ArgumentException();
            }

            var node = new Node(child)
            {
                Parent = nodesByValue[element],
            };
            nodesByValue.Add(child, node);
            nodesByValue[element].Children.Add(node);
        }

        public bool Contains(T element)
        {
            return this.nodesByValue.ContainsKey(element);
        }

        public IEnumerable<T> GetChildren(T element)
        {
            if (!this.nodesByValue.ContainsKey(element))
            {
                throw new ArgumentException();
            }

            return nodesByValue[element].Children.Select(x => x.Value);
        }

        public IEnumerable<T> GetCommonElements(Hierarchy<T> other)
        {
            //var keys = new List<T>();

            //foreach (var key in this.nodesByValue.Keys)
            //{
            //    if (other.Contains(key))
            //    {
            //        keys.Add(key);
            //    }
            //}
            //return keys;


           return this.nodesByValue.Keys.Intersect(other.nodesByValue.Keys);
        }

        public T GetParent(T element)
        {
            if (!this.nodesByValue.ContainsKey(element))
            {
                throw new ArgumentException();
            }

            if (this.nodesByValue[element].Parent == null)
            {
                return default;
            }
            return this.nodesByValue[element].Parent.Value;
        }

        public void Remove(T element)
        {
            if (element.Equals(this.root.Value))
            {
                throw new InvalidOperationException();
            }

            if (!nodesByValue.ContainsKey(element))
            {
                throw new ArgumentException();
            }

            var node = this.nodesByValue[element];
            var parenNode = node.Parent;

            parenNode.Children.Remove(node);
            parenNode.Children.AddRange(node.Children);

            foreach (var child in node.Children)
            {
                this.nodesByValue[child.Value].Parent = parenNode;
            }

            this.nodesByValue.Remove(element);
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            var queue = new Queue<Node>();

            queue.Enqueue(this.root);

            while (queue.Count != 0)
            {
                var current = queue.Dequeue();

                yield return current.Value;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
    }
}