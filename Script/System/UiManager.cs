using System.Collections.Generic;
using UnityEngine;
using FPS;

namespace FPS_Manager
{
    public class UiManager : MonoBehaviour
    {

        //缓存面板字典
        public Dictionary<string, GameObject> loadedPanels = new Dictionary<string, GameObject>();
        public Stack<BaseUIPanel> panelStack = new Stack<BaseUIPanel>();
        //路径
        public Dictionary<string, string> panelPath = new Dictionary<string, string>();

        public Transform normalCanvas;
        public Transform popupCanvas;



        private void Awake()
        {
            InitPanelPath();
            normalCanvas = GameTools.GetSingleComponentInChild<Transform>(gameObject, "NormalCanvas");
            popupCanvas = GameTools.GetSingleComponentInChild<Transform>(gameObject, "PopupCanvas");

        }
        /* public UiManager(Transform normal,Transform popup)
         {

             InitPanelPath();
             normalCanvas = normal;
             popupCanvas = popup;
             tPanel = GameTools.GetSingleComponentInChild<Image>(GameManager.Instance.gameObject, "TPanel");
             tPanel.color = new Color(tPanel.color.r, tPanel.color.g, tPanel.color.b, 0f);
         }*/
        private void InitPanelPath()
        {
            /*    panelPath.Add("SelectScenePanel", "UIPanel/SelectScenePanel");
                panelPath.Add("GamePanel", "UIPanel/GamePanel");
                panelPath.Add("TransitionPanel", "UIPanel/TransitionPanel");
                panelPath.Add("BringertPanel", "UIPanel/BringertPanel");
                panelPath.Add("MessagePanel", "UIPanel/MessagePanel");
                panelPath.Add("LevelUpPanel", "UIPanel/LevelUpPanel");
                panelPath.Add("MenuPanel", "UIPanel/MenuPanel");*/
            panelPath.Add("GamePanel", "Panel/GamePanel");
            panelPath.Add("MainMenuPanel", "Panel/MainMenuPanel");
            panelPath.Add("LoadPanel", "Panel/LoadPanel");
            panelPath.Add("MessagePanel", "Panel/MessagePanel");
            panelPath.Add("GameFinishPanel", "Panel/GameFinishPanel");
            panelPath.Add("SettingPanel", "Panel/SettingPanel"); 
            panelPath.Add("DeadPanel", "Panel/DeadPanel");
            panelPath.Add("GuidePanel", "Panel/GuidePanel");
            panelPath.Add("TransitionPanel", "Panel/TransitionPanel");
            panelPath.Add("TaskPanel", "Panel/TaskPanel");
            panelPath.Add("DialoguePanel", "Panel/DialoguePanel");
        }

        //动画过度，关闭面板时有关闭动画顺便黑屏，就是黑屏时，上一个面板的关闭动画跟着，显示时，当前面板打开动画跟着。

        //1.打开新面板后，用CanvasGroup取消交互，alpha为0.
        //2.然后执行当前面板的关闭或者隐藏操作
        //3.执行新面板的Open。
        //4.弹窗不管！


        /*TODO:两种弹窗
         * 我觉得UI面板分类中的弹窗Popup应该有两种类型，一种弹窗是跟普通UI一样，
         * 弹出来是需要按确定或者点击空白处关闭，这种上一个面板应该不用隐藏或者关闭，
         * 但是需要onshow一下改动。还有一种弹窗就是比如杀敌掉经验，然后弹出来告诉你，这种就自己自动显示几秒自己关闭，
         * 也没有什么互动的价值，所以上一个面板就不需要隐藏或者关闭。或者说，第二种弹窗就不必进入栈里，直接生成就好了。
         */
        public void OpenPanel(string panelName, bool isPopup = false)
        {
            //0.来个过渡动画

            //1.检测这个面板有没有注册
            if (!panelPath.ContainsKey(panelName))
            {
                Debug.LogError($"未注册的面板:{panelName}");
                return;
            }

            GameObject temp;
            BaseUIPanel tempBase;
            //2.实例化想要打开的面板
            //2.1分已经加载，和没有加载
            if (!loadedPanels.ContainsKey(panelName))//没有则去加载
            {
                GameObject prefab = Resources.Load<GameObject>(panelPath[panelName]);
                if (prefab == null)
                {
                    Debug.LogError($"无法加载预制体:{panelPath[panelName]}");
                }
                loadedPanels.Add(panelName, prefab);
            }
            //2.2实例化面板
            temp = GameObject.Instantiate(loadedPanels[panelName], isPopup ? popupCanvas : normalCanvas);//实例化,并决定位置
            //3.给面板加上组件
            AddCom(temp, panelName);
            tempBase = temp.GetComponent<BaseUIPanel>();
            //4.根据面板的类型判断
            /*
             * 普通面板：保持在 UI 栈中，提供刷新状态的方法。
             * 需要交互的弹窗：压入 UI 栈，确保正确的层级管理和焦点控制。
             * 不需要交互的提示弹窗：直接生成并定时销毁，不压入 UI 栈。
             */
            if (tempBase == null) { Debug.LogError($"实例化对象{panelName}不是BaseUIPanel的子类"); return; }
            //4.1 如果是普通的面板
            if (tempBase.Type == PanelType.NormalPanel)
            {
                if (panelStack.Count > 0)
                {
                    BaseUIPanel currentPanel = panelStack.Peek();//当前面板
                    //5.1根据面板等级隐藏或则和关闭
                    if (tempBase.Level > currentPanel.Level)//隐藏
                    {
                        currentPanel.Hide();
                    }
                    else if (tempBase.Level == currentPanel.Level)//出栈并摧毁
                    {
                        panelStack.Pop();
                        currentPanel.Close();
                    }
                    else
                    {
                        Debug.LogWarning("正在从高级面板打开低级面板");
                        return;
                    }
                }
                //6.1新面板压入并打开
                panelStack.Push(tempBase);//新的面板压入
                tempBase.Open();
            }
            //4.1 如果是需要交互的面板
            else if (tempBase.Type == PanelType.InteractPopupPanel)
            {
                if (panelStack.Count > 0)
                {
                    BaseUIPanel currentPanel = panelStack.Peek();//当前面板
                    //5.1 不用隐藏上一个面板，但是需要把当前面板的交互关闭
                    tempBase.canvasGroup.interactable = false;
                }
                //6.1新面板压入并打开
                panelStack.Push(tempBase);//新的面板压入
                tempBase.Open();
            }
            //4.3 如果是弹窗
            else
            {
                tempBase.Open();
            }



        }


        public void OpenAutoPanel<T>()where T:BaseUIPanel
        {
            //1.通用第一步，检测这个面板有没有注册
            //2.看看有没有加载过，没则加载并加入缓存字典
            //3.创建实例并且添加组件


        }
        public void OpenPanel<T>(bool isPopup = false,string message = null) where T : BaseUIPanel
        {
            //0.来个过渡动画

            //1.检测这个面板有没有注册
            if (!panelPath.ContainsKey(typeof(T).Name))
            {
                Debug.LogError($"未注册的面板:{typeof(T).Name}");
                return;
            }

            GameObject temp;
            BaseUIPanel tempBase;
            //2.实例化想要打开的面板
            //2.1分已经加载，和没有加载
            if (!loadedPanels.ContainsKey(typeof(T).Name))//没有则去加载
            {
                GameObject prefab = Resources.Load<GameObject>(panelPath[typeof(T).Name]);
                if (prefab == null)
                {
                    Debug.LogError($"无法加载预制体:{panelPath[typeof(T).Name]}");
                }
                loadedPanels.Add(typeof(T).Name, prefab);
            }
            //2.2实例化面板

            temp = GameObject.Instantiate(loadedPanels[typeof(T).Name], isPopup ? popupCanvas : normalCanvas);//实例化,并决定位置
            if(typeof(T).Name== "LoadPanel") { temp.transform.SetParent(popupCanvas); }
            //3.给面板加上组件
            // AddCom(temp, typeof(T).Name);
            temp.AddComponent<T>();
            tempBase = temp.GetComponent<BaseUIPanel>();
            //4.根据面板的类型判断
            /*
             * 普通面板：保持在 UI 栈中，提供刷新状态的方法。
             * 需要交互的弹窗：压入 UI 栈，确保正确的层级管理和焦点控制。
             * 不需要交互的提示弹窗：直接生成并定时销毁，不压入 UI 栈。
             */
            tempBase.message = message;
            if (tempBase == null) { Debug.LogError($"实例化对象{temp.name}不是BasePanel的子类"); return; }
            //4.1 如果是普通的面板
            if (tempBase.Type == PanelType.NormalPanel)
            {
                //  OpenTPanel(tempBase);//这里有0.5秒的执行，0.25留给关闭面板，0.25留给新面板。
                if (panelStack.Count > 0)
                {
                    BaseUIPanel currentPanel = panelStack.Peek();//当前面板
                    //5.1根据面板等级隐藏或则和关闭
                    if (tempBase.Level > currentPanel.Level)//隐藏
                    {
                        currentPanel.Hide();
                    }
                    else if (tempBase.Level == currentPanel.Level)//出栈并摧毁
                    {
                        panelStack.Pop();
                        currentPanel.Close();
                    }
                    else
                    {
                        Debug.LogWarning("正在从高级面板打开低级面板");
                        return;
                    }
                }
                //6.1新面板压入并打开
                panelStack.Push(tempBase);//新的面板压入
                tempBase.Open();

            }
            //4.1 如果是需要交互的面板
            else if (tempBase.Type == PanelType.InteractPopupPanel)
            {
                
                if (panelStack.Count > 0)
                {
                    BaseUIPanel currentPanel = panelStack.Peek();//当前面板
                    //5.1 不用隐藏上一个面板，但是需要把当前面板的交互关闭
                    currentPanel.canvasGroup.interactable = false;
                }
                //6.1新面板压入并打开
                panelStack.Push(tempBase);//新的面板压入
                tempBase.Open();
            }
            //4.3 如果是弹窗
            else
            {
                tempBase.Open();
            }



        }

        private void AddCom(GameObject temp, string Name)
        {
           /* switch (Name)
            {
                case "GamePanel":
                    temp.AddComponent<GamePanel>();
                    break;
                case "SelectScenePanel":
                    temp.AddComponent<SelectScenePanel>();
                    break;

            }*/

        }

        //这里是返回上一级UI的关闭，同级别之间的关闭在OpenPanel那，
        //所以不用管交互弹窗关闭后的问题。
        public void ClosePanel()
        {
            //1.判断当前栈内还有UI与否
            if (panelStack.Count == 0) { return; }
            //2.获得栈顶UIpanel
            BaseUIPanel t = panelStack.Pop();
            t.Close();   //当前面板出栈并摧毁
            if (panelStack.Count > 0)
            {
                panelStack.Peek().Show();
            }
        }
        public void CloseAllPanel()
        {
            if (panelStack.Count == 0) { return; }
            for (int i = 0; i < panelStack.Count; i++)
            {
                BaseUIPanel t = panelStack.Pop();
                t.gameObject.SetActive(true);
                t.Close();
            }
        }
        public void RegisterPanelPath(string panelName, string path)
        {
            if (!panelPath.ContainsKey(panelName))
            {
                panelPath.Add(panelName, path);
            }
            else
            {
                Debug.LogWarning($"尝试重复注册面板:{panelName}");
            }
        }
    }
}