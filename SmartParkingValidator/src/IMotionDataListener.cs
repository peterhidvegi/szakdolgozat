using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.src
{
    interface IMotionDataListener
    {
        void GetEventData(string eventDate,string node,float result);
    }
}
