using System;
using System.Collections.Generic;
using System.IO;

namespace Calendar
{
    public class CalendarEntries : List<ICalendarEntry>
     {
        public bool Load(string calendarEntriesFile)
        {
            // TODO.  Add your code to load the data from the file specified in
            //        calendarEntriesFile here.  You can edit the following two 
            //        lines if you wish.

            // Load the calendar enteries into the CalendarEntries collection from the specified file
            //returns a value of true if the load was successful, otherwise a value of false is returnd.

            bool status = true; // This displays the status of whetrher the file was opened or not - defaults to true.
            StreamReader calendarEntries = null;

            try
            {
                // Opens the calendar entry file specified in the parameter (C:\Users\<username>\AppData\Roaming\Calendar\CalendarApplication\1.0.0.0\appointments.txt)
                calendarEntries = new StreamReader(calendarEntriesFile);
                // Add any entreies located in the file into the CalenderEntries list
               
                // List to temporarily store the lines read by the streamreader
                List<string> lines = new List<string>();
                using (calendarEntries)
                {
                    string line;
                    // read the file, adding it to the list
                    while ((line = calendarEntries.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

                string firstNumber; // string value of the first number. used in the conversion process
                int singleOrRepeating; // Int value to determine whether the calendar entrey is a single event '0' or a repeating event '1'
                foreach (var calendarEntrey in lines)
                {
                    firstNumber = Convert.ToString(calendarEntrey).Substring(0, 1); // getting the first character of the saved data in the file that determins whether it is a single or recurring event
                    singleOrRepeating = int.Parse(firstNumber);
                    if (singleOrRepeating == 1)
                    {
                        // the calendar event is a single event
                        SingleCalendarEvent singleEvent = new SingleCalendarEvent(Convert.ToString(calendarEntrey).Remove(1, 0));
                        base.Add(singleEvent);
                    }
                    else if (singleOrRepeating == 2)
                    {
                        // the calendar event is a recurring event
                        RecurringCalendarEvent repeatingEvent = new RecurringCalendarEvent(Convert.ToString(calendarEntrey).Remove(1, 0));
                        base.Add(repeatingEvent);
                    }
                }
                status = true; // the file was successfully opened
            }
            catch
            {
                // If the file hasn't been opened, then the program sets the status as false
                status = false;
            }
            finally
            {
                // closes the calendar entries file
                if (calendarEntries != null)
                {
                    calendarEntries.Close();
                }
            }

            // returns the status of if the file has been opened or not.
            return status;
        }

        public bool Save(string calendarEntriesFile)
        {
            // TODO.  Add your code to save the collection to the file specified in
            //        calendarEntriesFile here.  You can edit the following two 
            //        lines if you wish.

            // Saves the contents of the collection into a specified file passed in as a parameter
            // replacing the existing file. it returns a value of true if the save was successful and false if it wasn't.

            bool status = true; // This is the current status of whether the file was succesfuly opend or not - defaults to true.
            StreamWriter calendarEntries = null;

            try
            {
                // Opens the calendar entry file specified in the parameter (C:\Users\jonat\AppData\Roaming\Calendar\CalendarApplication\1.0.0.0\appointments.txt)
                calendarEntries = new StreamWriter(calendarEntriesFile);
                // Add the collection into the file
                foreach (var calendarEntrey in base.ToArray())
                {
                    calendarEntries.WriteLine(calendarEntrey.SavedData);
                }
                status = true; // the file was successfully saved
            }

            catch
            {
                // If the file was unsuccesfully saved to then the status will be set to false.
                status = false;
            }

            finally
            {
                // closes the calendar entries file
                if (calendarEntries != null)
                {
                    calendarEntries.Close();
                }
            }

            // returns the final status of whether the file was sucessfully saved or not
            return status;
        }

        // Iterate through the collection, returning the calendar entries that
        // occur on the specified date

        public IEnumerable<ICalendarEntry> GetCalendarEntriesOnDate(DateTime date)
        {
            for (int i = 0; i < this.Count; i++ )
            {
                if (this[i].OccursOnDate(date))
                {
                    yield return this[i];                
                }
            }
        }
    }
}