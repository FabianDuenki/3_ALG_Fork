using System.ComponentModel.Design.Serialization;
using System.Security.AccessControl;

namespace AlgAdv.MB03_01.AVLTree {
    public enum TraverseModeEnum {
        PreOrder,
        PostOrder,
        InOrder,
        ReverseInOrder
    }

    public class BinarySearchTree<T> where T : IComparable<T> {

        private sealed class Node {

            // TNode muss IComparable implementieren
            public T Item { get; set; } = default!;

            public Node Left { get; set; } = null!;
            public Node Right { get; set; } = null!;

            public int BalanceFactor()
            {
                if (this == null) return 0;
                int factorLeft = Left != null ? Left.BalanceFactor() : 0;
                int factorRight = Right != null ? Right.BalanceFactor() : 0;

                return 1 + (Math.Max(factorLeft, factorRight));
            }
            public bool HasChild()
            {
                return this.Left != null || this.Right != null;
            }
            public Node Balance()
            {
                int balanceShift = Right.BalanceFactor() - Left.BalanceFactor();
                // is right shifted?
                if (balanceShift > 1)
                {
                    balanceShift = Right.Right.BalanceFactor() - Right.Left.BalanceFactor();
                    // is double right shifted?
                    if (balanceShift > 0)
                    {
                        return LeftLeftRotation();
                    }
                    // is right left shifted?
                    else if (balanceShift < 0)
                    {
                        return RightLeftRotation();
                    }
                }
                // is LeftShifted?
                else if (balanceShift < -1)
                {
                    balanceShift = Left.Right.BalanceFactor() - Left.Left.BalanceFactor();
                    // is left right shifted?
                    if (balanceShift > 0)
                    {
                        return LeftRightRotation();
                    }
                    // is double left shiftd?
                    else if (balanceShift < 0)
                    {
                        return RightRightRotation();
                    }

                }
            }
            private Node LeftLeftRotation()
            {
                var root = this.Right;
                this.Right = root.Left;
                root.Left = this;

                return root;
            }
            private Node RightLeftRotation()
            {
                this.Right = this.Right.RightRightRotation();
                return this.LeftLeftRotation();
            }
            private Node LeftRightRotation()
            {
                this.Left  = this.Left.LeftLeftRotation();
                return this.RightRightRotation();
            }
            private Node RightRightRotation()
            {
                var root = this.Left;
                this.Left = root.Right;
                root.Right = this;

                return root;
            }
        }

        private Node root = null!;

        public int Count { get; private set; }
        public TraverseModeEnum TraverseMode { get; set; }

        public BinarySearchTree() {
            TraverseMode = TraverseModeEnum.PreOrder;
        }

        /// <summary>
        /// Adds the provided item to the binary tree.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item) {
            // Case 1: The tree is empty - allocate the head
            if (root == null) {
                root = new Node() { Item = item };
            }
            // Case 2: The tree is not empty so find the right location to insert
            else {
                AddTo(root, item);
            }

            Count++;
        }

        public void AddRange(T[] items) {
            foreach (var item in items)
                Add(item);
        }

        // Recursive add algorithm
        private void AddTo(Node node, T item) {
            // Case 1: item is less than the current node value
            if (item.CompareTo(node.Item) < 0)
            {
                // if there is no left child make this the new left
                if (node.Left == null)
                {
                    node.Left = new Node() { Item = item };
                }
                else
                {
                    // else add it to the left node
                    AddTo(node.Left, item);
                }
            }
            // Case 2: item is equal to or greater than the current value
            else
            {
                // If there is no right, add it to the right
                if (node.Right == null)
                {
                    node.Right = new Node() { Item = item };
                }
                else
                {
                    // else add it to the right node
                    AddTo(node.Right, item);
                }
            }
            node = node.Balance();
        }

        /// <summary>
        /// Determines if the specified item exists in the binary tree.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>True if the tree contains the item, false otherwise</returns>
        public bool Contains(T item) {
            //var node = root;

            //while (node != null) {
            //    var c = node.Item.CompareTo(item);

            //    if (c == 0) {
            //        return true;
            //    }

            //    if (c > 0) {
            //        node = node.Left;
            //    } else {
            //        node = node.Right;
            //    }
            //}

            //return false;

            // defer to the node search helper function.
            return SearchWithParent(item, out _) != null;

        }

        public T? Search(T item) {
            var node = SearchWithParent(item, out _);
            if (node != null) {
                return node.Item;
            }
            return default;
        }

        /// <summary>
        /// Finds and returns the first node containing the specified item.  If the item
        /// is not found, returns null.  Also returns the parent of the found node (or null)
        /// which is used in Remove.
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <param name="parent">The parent of the found node (or null)</param>
        /// <returns>The found node (or null)</returns>
        private Node? SearchWithParent(T item, out Node? parent) {
            // Now, try to find data in the tree
            var node = root;
            parent = null;

            // while we don't have a match
            while (node != null) {
                var c = item.CompareTo(node.Item);

                if (c == 0) {
                    // we have a match!
                    return node;
                }

                parent = node;
                // if the item is less than current, go left.
                if (c < 0) {
                    node = node.Left;
                } else {
                    // if the item is more than current, go right.
                    node = node.Right;
                }
            }

            return null;
        }


        /// <summary>
        /// Removes the first occurance of the specified value from the tree.
        /// </summary>
        /// <param name="item">The value to remove</param>
        /// <returns>True if the value was removed, false otherwise</returns>
        public bool Remove(T item) {
            var current = this.SearchWithParent(item, out var parent);

            if (current == null) {
                return false;
            }

            Count--;

            // not leaf?
            if (current.Right != null || current.Left != null) {
                // Case 1: If current has no right child, then current's left replaces current
                if (current.Right == null) {
                    if (parent == null) {
                        // removing node is root
                        this.root = current.Left;
                    } else {
                        // Determine, if the current node (which will be removed) is left or right of its parent
                        var result = current.Item.CompareTo(parent.Item);
                        if (result < 0) {
                            // if parent value is less than current value
                            // make the current left child a left child of parent
                            parent.Left = current.Left;
                        } else if (result > 0) {
                            // if parent value is greater than current value
                            // make the current left child a right child of parent
                            parent.Right = current.Left;
                        }
                    }
                }
                // Case 2: If current has no left child, then current's right replaces current
                else if (current.Left == null) {
                    if (parent == null) {
                        // removing node is root
                        this.root = current.Right;
                    } else {
                        // Determine, if the current node (which will be removed) is left or right of its parent
                        var result = current.Item.CompareTo(parent.Item);
                        if (result < 0) {
                            // if parent value is less than current value
                            // make the current right child a left child of parent
                            parent.Left = current.Right;
                        } else if (result > 0) {
                            // if parent value is greater than current value
                            // make the current right child a right child of parent
                            parent.Right = current.Right;
                        }
                    }
                }
                // Case 3: If current's right child has a left child, replace current with current's
                //         right child's left-most child
                else {
                    // find the right's left-most child
                    var leftmost = current.Right.Left;
                    var leftmostParent = current.Right;

                    if (leftmost == null) {
                        leftmost = current.Right;
                    } else {
                        while (leftmost.Left != null) {
                            leftmostParent = leftmost;
                            leftmost = leftmost.Left;
                        }
                        // the parent's left subtree becomes the leftmost's right subtree
                        leftmostParent.Left = leftmost.Right;

                        // assign leftmost's left and right to current's left and right children
                        leftmost.Left = current.Left;
                        leftmost.Right = current.Right;
                    }

                    if (parent == null) {
                        // removing node is root
                        root = leftmost;
                    } else {
                        // Determine, if the current node (which will be removed) is left or right of its parent
                        var result = current.Item.CompareTo(parent.Item);
                        if (result < 0) {
                            // if parent value is less than current value
                            // make leftmost the parent's left child
                            parent.Left = leftmost;
                        } else if (result > 0) {
                            // if parent value is greater than current value
                            // make leftmost the parent's right child
                            parent.Right = leftmost;
                        }
                    }
                }
            }

            return true;
        }

        public void Clear() {
            root = null!;
            Count = 0;
        }

        public override string ToString() {
            var s = "";
            var level = 0;

            Traverse(root, level, ref s);

            return s;
        }

        private void Traverse(Node node, int level, ref string s) {
            if (node == null) {
                return;
            }

            var reverse = TraverseMode == TraverseModeEnum.ReverseInOrder;

            if (TraverseMode == TraverseModeEnum.PreOrder) {
                s += "".PadLeft(level, ' ') + node.Item + "\n";
            }
            Traverse(reverse ? node.Right : node.Left, level + 2, ref s);

            if (TraverseMode == TraverseModeEnum.InOrder || TraverseMode == TraverseModeEnum.ReverseInOrder) {
                s += "".PadLeft(level, ' ') + node.Item + "\n";
            }
            Traverse(reverse ? node.Left : node.Right, level + 2, ref s);

            if (TraverseMode == TraverseModeEnum.PostOrder) {
                s += "".PadLeft(level, ' ') + node.Item + "\n";
            }
        }
    }
}
