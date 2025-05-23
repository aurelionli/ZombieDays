using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace FPS_Manager
{
    public abstract class BaseModel : Imodel,IDisposable
    {
        public  void Dispose()
        {
            Save();
        }

        public abstract BaseModel InitModel();

        public abstract void Save(); 

    }
}
