using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class CheckNoHordeConditional : Conditional
    {
        public SharedBool angry;

        public override TaskStatus OnUpdate()
        {
            return angry.Value ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}