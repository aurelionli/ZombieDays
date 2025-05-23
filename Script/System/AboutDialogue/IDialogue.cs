

using FPS_Manager;
using System.Collections.Generic;

public interface IDialogue
{
    public void StartDialogue(List<DialogueNode> list)
    {
        GameManager.Instance.dialogueSystem.StartDialogue(list);
    }
}
