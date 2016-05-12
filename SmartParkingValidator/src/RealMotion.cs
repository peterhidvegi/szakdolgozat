using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.src
{
    class RealMotion
    {
        public DateTime date
        { get; }
    
        public List<MotionNode> motionNode { get; }

        public float avgPercentage
        { get; }

        public RealMotion(DateTime date, List<MotionNode> motionNode, float avgPercentage)
        {
            this.date = date;
            this.motionNode = motionNode;
            this.avgPercentage = avgPercentage;
        }

    }
}
