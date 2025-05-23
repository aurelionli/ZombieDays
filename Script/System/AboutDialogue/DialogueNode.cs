using System;
using System.Net.NetworkInformation;

public class DialogueNode
{
    public enum DialogueNodeType { Normal, Branch, Option, End }
    public int NodeID { get; set; }
    public string Speaker { get; set; }
    public string PortraitID { get; set; }
    public string Content { get; set; }
    public int defaultNextID { get; set; }
    public int failNextID { get; set; } //这个是进入节点失败的ID，就是没有达成条件进不了这个对话。
    public DialogueNodeType type { get; set; }

    // 回调系统
    public event  Action OnEnter;
    public event Action OnExit;
    public Func<bool> Condition { get; set; }

    public void TriggerEnter() => OnEnter?.Invoke();
    public void TriggerExit() => OnExit?.Invoke();
}