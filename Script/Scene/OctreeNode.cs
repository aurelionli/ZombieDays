using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeNode
{
    public Bounds Bounds { get; private set; }    // 节点的立方体边界
    public int Capacity { get; private set; }     // 节点容量
    public List<GameObject> Objects { get; private set; } // 节点中的物体
    public OctreeNode[] Children { get; private set; }    // 8个子节点

    public OctreeNode(Bounds bounds, int capacity)
    {
        Bounds = bounds;
        Capacity = capacity;
        Objects = new List<GameObject>();
        Children = new OctreeNode[8];
    }

    // 细分节点
    public void Subdivide()
    {
        Vector3 size = Bounds.size / 2;
        Vector3 center = Bounds.center;

        Children[0] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, -size.y / 2, -size.z / 2), size), Capacity);
        Children[1] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, -size.y / 2, -size.z / 2), size), Capacity);
        Children[2] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, size.y / 2, -size.z / 2), size), Capacity);
        Children[3] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, size.y / 2, -size.z / 2), size), Capacity);
        Children[4] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, -size.y / 2, size.z / 2), size), Capacity);
        Children[5] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, -size.y / 2, size.z / 2), size), Capacity);
        Children[6] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, size.y / 2, size.z / 2), size), Capacity);
        Children[7] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, size.y / 2, size.z / 2), size), Capacity);
    }

    // 检查是否应该插入到子节点
    public bool ShouldInsertToChildren(Vector3 position)
    {
        return Children[0] != null &&
               Bounds.Contains(position) &&
               Objects.Count >= Capacity;
    }
}
/*public class OctreeNode
{
    public Bounds Bounds;       // 节点的立方体边界
    public int Capacity;        // 节点容量
    public List<GameObject> Objects = new List<GameObject>(); // 节点中的物体
    public OctreeNode[] Children; // 8个子节点

    public OctreeNode(Bounds bounds, int capacity)
    {
        Bounds = bounds;
        Capacity = capacity;
        Children = new OctreeNode[8];
    }

    // 插入物体
    public void Insert(GameObject obj)
    {
        // 如果物体不在当前节点范围内，直接返回
        if (!Bounds.Contains(obj.transform.position))
            return;

        // 如果当前节点未满，直接添加
        if (Objects.Count < Capacity)
        {
            Objects.Add(obj);
        }
        else
        {
            // 如果未细分，先细分
            if (Children[0] == null)
                Subdivide();

            // 将物体插入到子节点
            foreach (var child in Children)
            {
                child.Insert(obj);
            }
        }
    }

    // 细分节点
    private void Subdivide()
    {
        Vector3 size = Bounds.size / 2;
        Vector3 center = Bounds.center;

        // 创建8个子节点
        Children[0] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, -size.y / 2, -size.z / 2), size), Capacity);
        Children[1] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, -size.y / 2, -size.z / 2), size), Capacity);
        Children[2] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, size.y / 2, -size.z / 2), size), Capacity);
        Children[3] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, size.y / 2, -size.z / 2), size), Capacity);
        Children[4] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, -size.y / 2, size.z / 2), size), Capacity);
        Children[5] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, -size.y / 2, size.z / 2), size), Capacity);
        Children[6] = new OctreeNode(new Bounds(center + new Vector3(-size.x / 2, size.y / 2, size.z / 2), size), Capacity);
        Children[7] = new OctreeNode(new Bounds(center + new Vector3(size.x / 2, size.y / 2, size.z / 2), size), Capacity);
    }
    public IEnumerator QueryOverFrames(Bounds range, List<GameObject> result, int queriesPerFrame = 500)
    {
        // 使用栈来模拟递归
        Stack<OctreeNode> nodeStack = new Stack<OctreeNode>();//深度优先搜索（DFS）
        nodeStack.Push(this); // 将根节点压入栈

        int queriesThisFrame = 0; // 当前帧已执行的查询次数

        while (nodeStack.Count > 0) // 栈不为空时继续处理
        {
            OctreeNode currentNode = nodeStack.Pop(); // 取出当前节点

            // 如果当前节点不与查询范围相交，跳过
            if (!currentNode.Bounds.Intersects(range))//快速检测两个包围盒（AABB）是否相交
                continue;

            // 添加当前节点的物体
            foreach (var obj in currentNode.Objects)
            {
                if (range.Contains(obj.transform.position))
                    result.Add(obj);

                queriesThisFrame++;
                if (queriesThisFrame >= queriesPerFrame) // 每帧限制查询次数
                {
                    queriesThisFrame = 0;
                    yield return null; // 等待下一帧
                }
            }

            // 将子节点压入栈中
            if (currentNode.Children[0] != null)
            {
                foreach (var child in currentNode.Children)
                {
                    nodeStack.Push(child);
                }
            }
        }
    }
    // 查询范围内的物体
   /* public List<GameObject> Query(Bounds range)
    {
        List<GameObject> result = new List<GameObject>();

        // 如果查询范围与当前节点不重叠，返回空
        if (!Bounds.Intersects(range))
            return result;

        // 添加当前节点的物体
        foreach (var obj in Objects)
        {
            if (range.Contains(obj.transform.position))
                result.Add(obj);
        }

        // 递归查询子节点
        if (Children[0] != null)
        {
            foreach (var child in Children)
            {
                result.AddRange(child.Query(range));
            }
        }

        return result;
    }

    // 移除物体
    public bool Remove(GameObject obj)
    {
        // 如果物体不在当前节点范围内，直接返回
        if (!Bounds.Contains(obj.transform.position))
            return false;

        // 尝试从当前节点移除物体
        if (Objects.Remove(obj))
            return true;

        // 递归从子节点移除物体
        if (Children[0] != null)
        {
            foreach (var child in Children)
            {
                if (child.Remove(obj))
                    return true;
            }
        }

        return false;
    }
}*/