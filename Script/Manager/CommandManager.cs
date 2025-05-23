using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEditor.UIElements;
using UnityEngine;
namespace FPS_Manager
{
    public class CommandManager : MonoBehaviour
    {
        //当前使用中的Command
        private readonly Dictionary<string, Command> commandsDir = new Dictionary<string, Command>();

        private void Awake() {
            StartCoroutine(CleanupExpiredCommandsRoutine());
        }

        private IEnumerator CleanupExpiredCommandsRoutine(){
            while (true){
                yield return new WaitForSeconds(60);
                CleanupExoiredCommands();
            }
        }
        public void SendCommand<T>(object data = null) where T : Command, new(){
            if (!commandsDir.ContainsKey(typeof(T).Name))
            {
                commandsDir.Add(typeof(T).Name, new T());
            }
            commandsDir[typeof(T).Name].Execute(data);
        }

        public void SendCommand<T, Q>(Q data = default ) where T : Command, new() where Q : ICommand
        {
        }
        private void CleanupExoiredCommands()
        {
            Debug.Log("开始释放命令");
            var expiredKeys = commandsDir.Where(pair => pair.Value.IsExpired).Select(pair => pair.Key).ToList();
            
            foreach (var key in expiredKeys)
            {
                if (commandsDir.TryGetValue(key, out var command))
                {
                    command.Dispose();
                    commandsDir.Remove(key);
                }
            }

            foreach (var item in commandsDir)
            {
                if (item.Value.IsExpired)
                {
                    item.Value.Dispose();
                    commandsDir.Remove(item.Key);
                }
            }
        }
        private void OnDestroy()
        {
            StopAllCoroutines();
        }

    }

}