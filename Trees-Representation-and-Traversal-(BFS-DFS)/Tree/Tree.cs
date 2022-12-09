namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Tree<T> : IAbstractTree<T>
    {
        private List<Tree<T>> children;

        public Tree(T key, params Tree<T>[] children)
        {
            this.Key = key;
            this.children = new List<Tree<T>>();
        }

        public T Key { get; private set; }

        public Tree<T> Parent { get; private set; }

        public IReadOnlyCollection<Tree<T>> Children => this.children.AsReadOnly();

        public void AddChild(Tree<T> child)
        {
            this.children.Add(child);
        }

        public void AddParent(Tree<T> parent)
        {
            this.Parent = parent;
        }

        public string AsString()
        {
            var sb = new StringBuilder();

            this.DfsAsString(sb, this, 0);

            return sb.ToString().Trim();
        }

        private void DfsAsString(StringBuilder sb, Tree<T> tree, int indent)
        {
            sb.Append(' ', indent)
                .AppendLine(tree.Key.ToString());

            foreach (var child in tree.children)
            {
                this.DfsAsString(sb, child, indent + 2);
            }
        }

        public IEnumerable<T> GetInternalKeys()
        {
            return this.BfsWithResultKey(tree => tree.children.Count > 0 && tree.Parent != null)
                .Select(tree => tree.Key);
        }

        private IEnumerable<Tree<T>> BfsWithResultKey(Predicate<Tree<T>> predicate)
        {
            var result = new List<Tree<T>>();
            var queue = new Queue<Tree<T>>();

            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var subTree = queue.Dequeue();

                if (predicate.Invoke(subTree))
                {
                    result.Add(subTree);
                }

                foreach (var child in subTree.children)
                {
                    queue.Enqueue(child);
                }
            }

            return result;
        }

        public IEnumerable<T> GetLeafKeys()
        {
            return this.BfsWithResultKey(tree => tree.children.Count == 0)
                .Select(tree => tree.Key);
        }

        public T GetDeepestKey()
        {
            return this.GetDeepestNode().Key;
        }

        private Tree<T> GetDeepestNode()
        {
            var leafs = this.BfsWithResultKey(tree => tree.children.Count == 0);

            Tree<T> deepestNode = null;
            var maxDepth = 0;

            foreach (var leaf in leafs)
            {
                var depth = this.GetDepth(leaf);

                if (depth > maxDepth)
                {
                    maxDepth = depth;
                    deepestNode = leaf;
                }
            }
            return deepestNode;
        }

        private int GetDepth(Tree<T> leaf)
        {
            int depth = 0;
            var tree = leaf;

            while (tree.Parent != null)
            {
                depth++;
                tree = tree.Parent;
            }
            return depth;
        }

        public IEnumerable<T> GetLongestPath()
        {
            Tree<T> current = this.GetDeepestNode();
            var path = new Stack<T>();

            while (current != null)
            {
                path.Push(current.Key);
                current = current.Parent;
            }

            return path;
        }
    }
}
