using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.src
{
    class Ticket
    {

        readonly int sensor_id;
        readonly DateTime start;
        readonly DateTime end;
        readonly string plate;

        public int Sensor_id
        {
            get
            {
                return sensor_id;
            }
        }

        public DateTime Start
        {
            get
            {
                return start;
            }
        }

        public DateTime End
        {
            get
            {
                return end;
            }
        }

        public string Plate
        {
            get
            {
                return plate;
            }
        }

        public Ticket(int sensor_id,DateTime start, DateTime end, string plate)
        {
            this.sensor_id = sensor_id;
            this.start = start;
            this.end = end;
            this.plate = plate;
        }
    }
}
