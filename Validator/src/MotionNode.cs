using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.src
{
    class MotionNode
    {
        public int node{ get; }

        public float percentage { get; }

        public MotionNode(int node, float percentage)
        {
            this.node = node;
            this.percentage = percentage;
        }
    }
}
