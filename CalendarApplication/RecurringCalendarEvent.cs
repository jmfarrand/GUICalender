using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This is a class that holds objects of a calendar event that is repeating 
//daily, weekly, every two weeks, every month and every year.

namespace Calendar
{
    public class RecurringCalendarEvent : ICalendarEntry
    {
        // class variables for the repeating calendar event
        DateTime _Start;
        int _Length;
        string _DisplayText;
        string _SavedData;

        public RecurringCalendarEvent()
        {
            // blank constructor
            // this is for when the user is updating the calender event,
            //as the entry already exists.
        }

        public RecurringCalendarEvent(string data)
        {
            // variables to hold the data in the calendar entry
            DateTime start;
            int length;
            string displayText;
            string savedData;

            char splitter = '\t'; // The data is seperated as tabs. This makes it so that the data is splitted as tabss in the main file
            string[] splittedData = data.Split(splitter); // array to hold the splitted data

            start = Convert.ToDateTime(splittedData[1]);
            length = int.Parse(splittedData[2]);
            displayText = splittedData[3];
            savedData = data;

            // set all the data in the class as the data from the array.
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
            // this needs to have more work - but I'm not quite sure how to make this work
            // both events are treated like single events which work.
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
