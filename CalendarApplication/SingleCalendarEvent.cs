using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This is a class that holds a object of a single calendar event. The entrey will
//occur just once, it is not a repeating event.

namespace Calendar
{
    public class SingleCalendarEvent : ICalendarEntry
    {
        // variables for the single calendar event
        DateTime _Start;
        int _Length;
        string _DisplayText;
        string _SavedData;
        
        public SingleCalendarEvent()
        {
            // blank constructor
            // this is for when the user is updating the calender event,
            //as the entry already exists.
        }

        public SingleCalendarEvent(string data)
        {
            // variables to hold the data in the calendar entry
            DateTime start;
            int length;
            string displayText;
            string savedData;

            /* seperating the data */
            char splitter = '\t'; // The data is seperated by tabs. This makes it so that the data is splitted as tabs in the main file
            string[] splittedData = data.Split(splitter); // array to hold the splitted data

            /* put the splitted data in the array into the variables */
            start = Convert.ToDateTime(splittedData[1]);
            length = int.Parse(splittedData[2]);
            displayText = splittedData[3];
            savedData = data;

            // set all the data in the class as the data in the local variables.
            _Start = start;
            _Length = length;
            _DisplayText = displayText;
            _SavedData = savedData;
        }

        public DateTime Start
        {
            get
            {
                return _Start;
            }
        }

        public int Length
        {
            get
            {
                return _Length;
            }
        }

        public string DisplayText
        {
            get
            {
                return _DisplayText;
            }
        }

        public string SavedData
        {
            get
            {
                return _SavedData;
            }
        }

        public bool OccursOnDate(DateTime date)
        {
            int compare;
            compare = DateTime.Compare(date.Date, _Start.Date);

            if (compare == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
