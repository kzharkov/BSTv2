using System;
using System.Collections.Generic;

namespace AlgorithmsDataStructures2
{
    public class BSTNode<T>
    {
        public int NodeKey; // ключ узла
        public T NodeValue; // значение в узле
        public BSTNode<T> Parent; // родитель или null для корня
        public BSTNode<T> LeftChild; // левый потомок
        public BSTNode<T> RightChild; // правый потомок	

        public BSTNode(int key, T val, BSTNode<T> parent)
        {
            NodeKey = key;
            NodeValue = val;
            Parent = parent;
            LeftChild = null;
            RightChild = null;
        }
    }

    // промежуточный результат поиска
    public class BSTFind<T>
    {
        // null если в дереве вообще нету узлов
        public BSTNode<T> Node;

        // true если узел найден
        public bool NodeHasKey;

        // true, если родительскому узлу надо добавить новый левым
        public bool ToLeft;

        public BSTFind()
        {
            Node = null;
        }
    }

    public class BST<T>
    {
        BSTNode<T> Root; // корень дерева, или null

        public BST(BSTNode<T> node)
        {
            Root = node;
        }

        public BSTFind<T> FindNodeByKey(int key)
        {
            // ищем в дереве узел и сопутствующую информацию по ключу
            BSTFind<T> bstFind = new();

            BSTNode<T> node = Root;
            while (node != null)
            {
                bstFind.Node = node;
                if (node.NodeKey == key)
                {
                    bstFind.NodeHasKey = true;
                    break;
                }

                if (key < node.NodeKey)
                {
                    node = node.LeftChild;
                    bstFind.ToLeft = true;
                }
                else
                {
                    node = node.RightChild;
                    bstFind.ToLeft = false;
                }
            }

            return bstFind;
        }

        public bool AddKeyValue(int key, T val)
        {
            // добавляем ключ-значение в дерево
            var bstFind = FindNodeByKey(key);

            if (bstFind.NodeHasKey)
            {
                return false; // если ключ уже есть
            }

            if (bstFind.ToLeft)
            {
                bstFind.Node.LeftChild = new BSTNode<T>(key, val, bstFind.Node);
            }
            else
            {
                bstFind.Node.RightChild = new BSTNode<T>(key, val, bstFind.Node);
            }

            return true;
        }

        public BSTNode<T> FinMinMax(BSTNode<T> FromNode, bool FindMax)
        {
            // ищем максимальный/минимальный ключ в поддереве
            while (FromNode != null)
            {
                if (FindMax)
                {
                    if (FromNode.RightChild == null)
                    {
                        return FromNode;
                    }

                    FromNode = FromNode.RightChild;
                }
                else
                {
                    if (FromNode.LeftChild == null)
                    {
                        return FromNode;
                    }

                    FromNode = FromNode.LeftChild;
                }
            }

            return null;
        }

        public bool DeleteNodeByKey(int key)
        {
            // удаляем узел по ключу
            var bstFind = FindNodeByKey(key);
            if (!bstFind.NodeHasKey)
            {
                return false; // если узел не найден
            }

            var deletedNode = bstFind.Node;

            if (deletedNode.RightChild == null)
            {
                if (deletedNode.LeftChild != null)
                {
                    deletedNode.LeftChild.Parent = deletedNode.Parent;
                }

                if (deletedNode == Root)
                {
                    Root = deletedNode.LeftChild;
                    return true;
                }
                
                if (deletedNode.Parent.LeftChild == deletedNode)
                {
                    deletedNode.Parent.LeftChild = deletedNode.LeftChild;
                }
                else
                {
                    deletedNode.Parent.RightChild = deletedNode.LeftChild;
                }

                return true;
            }

            var node = FinMinMax(deletedNode.RightChild, false);

            if (deletedNode.LeftChild != null)
            {
                deletedNode.LeftChild.Parent = node;
                node.LeftChild = deletedNode.LeftChild;
            }

            if (node.Parent != deletedNode)
            {
                if (node.LeftChild == null && node.RightChild == null)
                {
                    node.Parent.LeftChild = null;
                }
                else
                {
                    if (node.RightChild != null)
                    {
                        node.RightChild.Parent = node.Parent;
                    }

                    node.Parent.LeftChild = node.RightChild;
                }
            }

            deletedNode.RightChild.Parent = node;

            if (deletedNode.RightChild != node)
            {
                node.RightChild = deletedNode.RightChild;
            }

            if (deletedNode == Root)
            {
                node.Parent = null;
                Root = node;
                return true;
            }

            if (deletedNode.Parent.LeftChild == deletedNode)
            {
                deletedNode.Parent.LeftChild = node;
            }
            else
            {
                deletedNode.Parent.RightChild = node;
            }

            node.Parent = deletedNode.Parent;

            return true;
        }

        public int Count()
        {
            if (Root == null)
            {
                return 0;
            }

            return CountHelper(Root, 0); // количество узлов в дереве
        }

        private int CountHelper(BSTNode<T> node, int count)
        {
            count++;
            if (node.LeftChild != null)
            {
                count = CountHelper(node.LeftChild, count);
            }

            if (node.RightChild != null)
            {
                count = CountHelper(node.RightChild, count);
            }

            return count;
        }
    }
}