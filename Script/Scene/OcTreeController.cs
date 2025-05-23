using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;



    public class OcTreeController
{
        private OctreeNode root;

        public OcTreeController(Bounds bounds, int capacity)
        {
            root = new OctreeNode(bounds, capacity);
        }

    public void Subdivide(OctreeNode tree)
    {
        
        Vector3 size = tree.Bounds.size / 2;
        Vector3 center = tree.Bounds.center;

        tree.Children[0] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, -size.y / 2, -size.z / 2), size), tree.Capacity);
        tree.Children[1] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, -size.y / 2, -size.z / 2), size), tree.Capacity);
        tree.Children[2] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, size.y / 2, -size.z / 2), size), tree.Capacity);
        tree.Children[3] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, size.y / 2, -size.z / 2), size), tree.Capacity);
        tree.Children[4] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, -size.y / 2, size.z / 2), size), tree.Capacity);
        tree.Children[5] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, -size.y / 2, size.z / 2), size), tree.Capacity);
        tree.Children[6] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, size.y / 2, size.z / 2), size), tree.Capacity);
        tree.Children[7] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, size.y / 2, size.z / 2), size), tree.Capacity);
    }

    // 检查是否应该插入到子节点
   /* public bool ShouldInsertToChildren(OctreeNode tree,Vector3 position)
    {
        return tree.Children[0] != null &&
               tree.Bounds.Contains(position) &&
               tree.Objects.Count >= tree.Capacity;
    }*/
    // 插入物体
    public void Insert(GameObject obj)
        {
            InsertRecursive(root, obj);
        }

        private void InsertRecursive(OctreeNode node, GameObject obj)
        {
            if (!node.Bounds.Contains(obj.transform.position))
                return;

            if (node.Objects.Count < node.Capacity)
            {
                node.Objects.Add(obj);
            }
            else
            {
                if (node.Children[0] == null)
                    Subdivide(node);

                foreach (var child in node.Children) {
                    InsertRecursive(child, obj);
                }
            }
        }

        // 查询范围内的物体
        public List<GameObject> Query(Bounds range)
        {
            List<GameObject> result = new List<GameObject>();
            QueryRecursive(root, range, result);
            return result;
        }

        private void QueryRecursive(OctreeNode node, Bounds range, List<GameObject> result)
        {
            if (!node.Bounds.Intersects(range))
                return;

            foreach (var obj in node.Objects)
            {
                if (range.Contains(obj.transform.position))
                    result.Add(obj);
            }

            if (node.Children[0] != null)
            {
                foreach (var child in node.Children)
                {
                    QueryRecursive(child, range, result);
                }
            }
        }

        // 分帧查询
        public IEnumerator QueryOverFrames(Bounds range, List<GameObject> result, int queriesPerFrame = 500)
        {
            Stack<OctreeNode> nodeStack = new Stack<OctreeNode>();
            nodeStack.Push(root);
            int queriesThisFrame = 0;

            while (nodeStack.Count > 0)
            {
                OctreeNode currentNode = nodeStack.Pop();

                if (!currentNode.Bounds.Intersects(range))
                    continue;

                foreach (var obj in currentNode.Objects)
                {
                    if (range.Contains(obj.transform.position))
                        result.Add(obj);

                    queriesThisFrame++;
                    if (queriesThisFrame >= queriesPerFrame)
                    {
                        queriesThisFrame = 0;
                        yield return null;
                    }
                }

                if (currentNode.Children[0] != null)
                {
                    foreach (var child in currentNode.Children)
                    {
                        nodeStack.Push(child);
                    }
                }
            }
        }

        // 移除物体
        public bool Remove(GameObject obj)
        {
            return RemoveRecursive(root, obj);
        }

        private bool RemoveRecursive(OctreeNode node, GameObject obj)
        {
            if (!node.Bounds.Contains(obj.transform.position))
                return false;

            if (node.Objects.Remove(obj))
                return true;

            if (node.Children[0] != null)
            {
                foreach (var child in node.Children)
                {
                    if (RemoveRecursive(child, obj))
                        return true;
                }
            }

            return false;
        }
    }

