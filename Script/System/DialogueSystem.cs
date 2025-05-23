using FPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace FPS_Manager
{ 
    public class DialogueSystem : ISystem
    {
        private ISystem IC;
       
        public List<DialogueNode> _currentDialogue;    //对话内容
        public DialogueNode _currentNode;               //当前对话节点

        //  public DialoguePanel dialoguePanel;

        public DialogueNode _currentNodeOptions;
        public DialogueSystem()
        {
            IC = this;
            IC.RegisterEvent("ContinueDialogue");


            IC.RegisterEvent("UpdateUICallback");
            //上面是UI中的继续按钮，这个是生成的选项的。
            IC.RegisterEvent("OptionButtonOnClicked"); //这个可以跟ContinueDialogue合并，但是我为了区分，就分开了。


            IC.StartListening("OptionButtonOnClicked", TriggerEvent_OptionButtonOnClicked);
            IC.StartListening("UpdateUICallback", TriggerEvent_UpdateUICallback);
            IC.StartListening("ContinueDialogue", TriggerEvent_ContinueDialogue);
        }
        public void TriggerEvent_OptionButtonOnClicked(object data)
        {
            MoveToNode((int)data);
        }
        /// <summary>
        /// 就是UI中正文内容显示完的回调
        /// </summary>
        /// <param name="data"></param>
        public void TriggerEvent_UpdateUICallback(object data)
        {
            ProcessNode(_currentNode);
        }
        /// <summary>
        /// 这是继续按钮的回调。
        /// </summary>
        /// <param name="data"></param>
        public void TriggerEvent_ContinueDialogue(object data)
        {
            MoveToNode(_currentNode.defaultNextID);
        }

        public void StartDialogue(List<DialogueNode> list)
        {
            _currentDialogue = list;
            IC.OpenPanel<DialoguePanel>();
            MoveToNode(0);
        }

        public void MoveToNode(int nodeID)
        {
            // _currentNode?.TriggerExit();//处理当前节点退出回调
            Debug.Log("当前ID"+nodeID);
            _currentNode = _currentDialogue.FirstOrDefault(n => n.NodeID == nodeID);
            //如果ID=-1，就是结束对话
            if (_currentNode == null)
            { Debug.Log("找不到目标ID，退出对话"); EndDialogue(); return; }

            //条件检测这个节点,如果不为空，条件不满足
            if (_currentNode.Condition != null && !_currentNode.Condition.Invoke())
            {
                MoveToNode(_currentNode.failNextID);
                return;
            }

           // _currentNode.TriggerEnter();
            UpdateUI(_currentNode);
           // ProcessNode(_currentNode);

        }

        public void ProcessNode(DialogueNode node)
        {
            Debug.Log("当前语句的类型为:" + node.type);
            switch (node.type)
            {
                case DialogueNode.DialogueNodeType.Normal:
                    // 这里自动或者手动继续
                 //   MoveToNode(node.defaultNextID);
                    break;
                case DialogueNode.DialogueNodeType.Branch:
                    Debug.Log("生成按钮");
                    ShowOptions(node.NodeID);
                    break;
                case DialogueNode.DialogueNodeType.End:
                    //EndDialogue();
                    MoveToNode(-1);
                    break;
            }
        }

        public void UpdateUI(DialogueNode node)
        {
            //把正文内容传送给UI系统。且等待回调。
            IC.TriggerEvent("ShowDialogueText", node);
          
        }
        public void ShowOptions(int optionIDs)
        {
            for (int i = optionIDs+1; i < _currentDialogue.Count; i++)
            {
                if (_currentDialogue[i].type == DialogueNode.DialogueNodeType.Option)
                {
                    Debug.Log("按钮生成");
                    /*if (_currentNode.Condition != null && !_currentNode.Condition.Invoke())
                    {
                        continue;
                    }*/
                    _currentDialogue[i].OnExit += () => { MoveToNode(_currentDialogue[i].defaultNextID); };
                    IC.TriggerEvent("GeneraOptions", _currentDialogue[i]);
                }
                else
                {
                    break;
                }

            }
        }
        public void EndDialogue()
        {
            _currentNode?.TriggerExit();
            _currentDialogue = null;
            _currentNode = null;
            IC.ClosePanel();
        }
    }

}

