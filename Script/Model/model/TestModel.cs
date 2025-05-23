using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPS_Manager
{
    public class TestModel : BaseModel
    {
        public override BaseModel InitModel()
        {
           return this;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
   
    }
}
