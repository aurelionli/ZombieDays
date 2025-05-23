using FPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class FindPlayerGameObjectAction : Action
    {
        public SharedGameObject Player;

        public override void OnStart()
        {
            base.OnStart();
            Player.Value = GameObject.FindWithTag("Player");
           // Player.Value = GameObject.FindTag("Player");
        }
        public override TaskStatus OnUpdate()
        {
            return Player.Value==null ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}