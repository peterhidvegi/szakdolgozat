using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    class Types
    {

        public enum SaveType
        {
            jpg, png
        }
        public enum ImgType
        {
            snap, motion
        }

        public enum Status
        {
            legal,illegal
        }

        public enum EventType
        {
            sensor,motion,ticketStart,ticketEnd
        }
    }
}
