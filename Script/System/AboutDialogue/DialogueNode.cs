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
    public int failNextID { get; set; } //����ǽ���ڵ�ʧ�ܵ�ID������û�д����������������Ի���
    public DialogueNodeType type { get; set; }

    // �ص�ϵͳ
    public event  Action OnEnter;
    public event Action OnExit;
    public Func<bool> Condition { get; set; }

    public void TriggerEnter() => OnEnter?.Invoke();
    public void TriggerExit() => OnExit?.Invoke();
}