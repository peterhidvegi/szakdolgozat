using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.src
{
    class Motion
    {

        public DateTime date
        {
            get;
        }

        public int node
        {
            get;
        }

        public float percentage
        {
            get;
        }

        public Motion(DateTime date, int node,float percentage)
        {
            this.date = date;
            this.node = node;
            this.percentage = percentage;
        }

    }
}
