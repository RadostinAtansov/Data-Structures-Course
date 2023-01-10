namespace AVLTree
{
    using System;

    public class AVL<T> where T : IComparable<T>
    {
        public class Node
        {
            public Node(T value)
            {
                this.Value = value;
                this.Height = 1;
            }

            public T Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public int Height { get; set; }
        }

        public Node Root { get; private set; }

        public bool Contains(T element)
        {
            return this.Contains(this.Root, element) != null;
        }

        private Node Contains(Node node, T element)
        {
            if (node == null)
            {
                return null;
            }

            var compare = element.CompareTo(node.Value);

            if (compare < 0)
            {
                return this.Contains(node.Left, element);
            }
            else if (compare > 0)
            {
                return this.Contains(node.Right, element);
            }

            return node;
        }

        public void Delete(T element)
        {
            this.Root = this.Delete(this.Root, element);
        }

        private Node Delete(Node node, T element)
        {
            if (node == null)
            {
                return null;
            }

            if (element.CompareTo(node.Value) < 0)
            {
                node.Left = this.Delete(node.Left, element);
            }
            else if (element.CompareTo(node.Value) > 0)
            {
                node.Right = this.Delete(node.Right, element);
            }
            else
            {

                if (node.Left == null && node.Right == null)
                {
                    return null;
                }
                else if (node.Left == null)
                {
                    node = node.Right;
                }
                else if (node.Right == null)
                {
                    node = node.Left;
                }
                else
                {
                    Node temp = this.FimdSmallestChild(node.Right);
                    node.Value = temp.Value;
                    node.Right = this.Delete(node.Right, temp.Value);
                }
            }
            node = this.Balance(node);
            node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

            return node;
        }

        private Node FimdSmallestChild(Node node)
        {
            if (node.Left == null)
            {
                return node;
            }

            return this.FimdSmallestChild(node.Left);
        }

        public void DeleteMin()
        {
            if (this.Root == null)
            {
                return;
            }
            var temp = this.FimdSmallestChild(this.Root);

            this.Root = this.Delete(this.Root, temp.Value);
        }

        public void Insert(T element)
        {
            this.Root = this.Insert(this.Root, element);
        }

        private Node Insert(Node node, T element)
        {
            if (node == null)
            {
                return new Node(element);
            }

            if (element.CompareTo(node.Value) < 0) 
            {
                node.Left = this.Insert(node.Left, element);
            }
            else
            {
                node.Right = this.Insert(node.Right, element);
            }

            node = Balance(node);
            node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

            return node;
        }

        private Node Balance(Node node)
        {
            var balanceFactor = Height(node.Left) - Height(node.Right); // [-1, 0, 1]

            if (balanceFactor > 1)
            {
                var childBalanceFactor = Height(node.Left.Left) - Height(node.Left.Right);
                if (childBalanceFactor < 0)
                {
                    node.Left = RotateLeft(node.Left);
                }
                node = RotateRight(node);
            }
            else if (balanceFactor < -1)
            {
                var childBalancedFactor = Height(node.Right.Left) - Height(node.Right.Right);
                if (childBalancedFactor > 0)
                {
                    node.Right = RotateRight(node.Right);
                }
                node = RotateLeft(node);
            }

            return node;
        }

        private Node RotateRight(Node node)
        {
            var left = node.Left;
            node.Left = left.Right;
            left.Right = node;

            node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

            return left;
        }

        private Node RotateLeft(Node node)
        {
            var right = node.Right;
            node.Right = right.Left;
            right.Left = node;

            node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;

            return right;
        }

        private int Height(Node node)
        {
            if (node == null)
            {
                return 0;
            }

            return node.Height;
        }

        public void EachInOrder(Action<T> action)
        {
            this.EachInOrder(this.Root, action);
        }

        private void EachInOrder(Node node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            this.EachInOrder(node.Left, action);
            action(node.Value);
            this.EachInOrder(node.Right, action);
        }
    }
}
