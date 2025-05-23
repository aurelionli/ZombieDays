

using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace FPS
{
    public class GetIGeThrowController : BaseInteractController
    {



        public override void BePickUp()
        {
            IC.SendCommand<GetGeThrowCommand>();
            IC.OpenPanel<MessagePanel>(true, "Player Get HandGrenade");
            //  IC.ReturnObject("GeThrow", gameObject);
        }

    }
}
