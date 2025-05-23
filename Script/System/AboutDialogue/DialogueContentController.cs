using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DialogueContentController
{
    public const string RESOURCE_PATH = "Dialogue/NPC_{0}";

    public static List<DialogueNode>  LoadDialogue(int npcID)
    {
        //1.加载文件内容
       // TextAsset textAsset = Resources.Load<TextAsset>(string.Format(RESOURCE_PATH, npcID));
        TextAsset textAsset = Resources.Load<TextAsset>("10011");
        //2.判断是否加载到了
        if (textAsset == null) { Debug.LogError("文件为空！"); return null; }
        //3.处理并且返回
        return ParseContent(textAsset.text);
    }

    public static List<DialogueNode> ParseContent(string content)
    {
        List<DialogueNode> nodes = new List<DialogueNode>();
        StringReader reader = new StringReader(content);

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            Debug.Log(line);
            if (string.IsNullOrWhiteSpace(line)) continue; //检测是否为空，null，仅包含空白字符
            string[] parts = line.Split('&');
            if (parts.Length != 7)
            {
                Debug.LogError($"格式错误：{line}");
                continue;
            }
            DialogueNode node = new DialogueNode
            {
                NodeID = int.Parse(parts[0]),
                type = (DialogueNode.DialogueNodeType)int.Parse(parts[1]),
                Speaker = parts[2],
                PortraitID = parts[3],
                Content = parts[4],
                defaultNextID = int.Parse(parts[5]),
                failNextID = int.Parse(parts[6])
            };
            Debug.Log(node.Content);
            nodes.Add(node);
        }
        return nodes;
    }
}
