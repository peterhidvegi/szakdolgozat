using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator
{
    class Senzor
    {

        List<int> eventId;

        List<int> occupancy;

        List<DateTime> dateTime;

        List<string> validation;

        public List<int> Occupancy
        {
            get
            {
                return occupancy;
            }

            set
            {
                occupancy = value;
            }
        }

        public List<DateTime> DateTime
        {
            get
            {
                return dateTime;
            }

            set
            {
                dateTime = value;
            }
        }

        public List<string> Validation
        {
            get
            {
                return validation;
            }

            set
            {
                validation = value;
            }
        }

        public List<int> EventId
        {
            get
            {
                return eventId;
            }

            set
            {
                eventId = value;
            }
        }

        public Senzor()
        {
            eventId = new List<int>();
            occupancy = new List<int>();
            dateTime = new List<DateTime>();
            validation = new List<string>();
        }
    }
}
