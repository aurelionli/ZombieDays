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
    /*����ÿһ�仰���л����Ļص���������벥�ţ��˳���ͣ��
     * Ȼ��֪����һ�仰�ᴥ��������¼�
     * 
     */
    private void AddNormalCallBack()
    {
        foreach (var item in dialogueNodes)
        {
            if(item.type==DialogueNode.DialogueNodeType.Option)
            {
                item.OnEnter += () => { Debug.Log("�����¼��ص�"); IC.PlaySFX("Special Click 45"); };
                item.OnExit += () => { Debug.Log("�����¼��ص�"); };
            }
            else
            {
                item.OnEnter += () => { Debug.Log("�����¼��ص�"); IC.PlaySFX("Special Click 45"); };
                item.OnExit += () => { Debug.Log("�����¼��ص�"); };
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
        Debug.Log("�Ի���ʼ");
        IC.TriggerEvent("ControlInput", false);
        Cursor.lockState = CursorLockMode.None;
        ID.StartDialogue(dialogueNodes);
    }
}