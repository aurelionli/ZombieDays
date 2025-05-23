using FPS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : BaseInteractController,IDialogue
{
    private IDialogue ID;
    public int _npcID;
    List<DialogueNode> dialogueNodes;

    protected override void Awake()
    {
        base.Awake();
        ID = this;
        dialogueNodes = DialogueContentController.LoadDialogue(_npcID);
        AddNormalCallBack();
        AddSpecialCallBack();
    }
    /*首先每一句话都有基础的回调，比如进入播放，退出暂停。
     * 然后知道哪一句话会触发特殊的事件
     * 
     */
    private void AddNormalCallBack()
    {
        foreach (var item in dialogueNodes)
        {
            if(item.type==DialogueNode.DialogueNodeType.Option)
            {
                item.OnEnter += () => { Debug.Log("进入事件回调"); IC.PlaySFX("Special Click 45"); };
                item.OnExit += () => { Debug.Log("进入事件回调"); };
            }
            else
            {
                item.OnEnter += () => { Debug.Log("进入事件回调"); IC.PlaySFX("Special Click 45"); };
                item.OnExit += () => { Debug.Log("进入事件回调"); };
            }
    
            if (item.NodeID == 2)
            {
                item.OnEnter += () => { IC.OpenPanel<SettingPanel>(); };
            }

            //item.Condition += () => { return true; };

        }

    }
    private void AddSpecialCallBack()
    {

    }

    public override void BePickUp()
    {
        Debug.Log("对话开始");
        IC.TriggerEvent("ControlInput", false);
        Cursor.lockState = CursorLockMode.None;
        ID.StartDialogue(dialogueNodes);
    }
}