using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{
    public partial class AddRecurringAppointment : Form
    {
        RecurringCalendarEvent _RepeatingEvent = null;
        DateTime date;
        Color _defaultHowManyTimesColour;
        Color _defaultSubjectColour;
        Color _defaultLocationColour;

        public AddRecurringAppointment()
        {
            InitializeComponent();

            // get the default back colour for the text boxes so they can be reset when errors have been corrected or validation has finished
            _defaultHowManyTimesColour = textBoxHowManyTimes.BackColor;
            _defaultSubjectColour = textBoxSubject.BackColor;
            _defaultLocationColour = textBoxSubject.BackColor;
        }

        public RecurringCalendarEvent RepeatingEvent
        {
            get
            {
                return _RepeatingEvent;
            }
            set
            {
                _RepeatingEvent = value;
            }
        }

        public DialogResult ShowDialog(string dateFromMainForm)
        {
            date = Convert.ToDateTime(dateFromMainForm);
            labelDate.Text = dateFromMainForm;
            this.ShowDialog();
            return DialogResult; // return the DialogResult. This is used for the if statement that the modal dialog box is a part of
        }

        public DialogResult ShowDialog(ICalendarEntry recurring, string dateFromMainForm)
        {
            // set the displayed date as the one that is passed in as the paramter
            date = Convert.ToDateTime(dateFromMainForm);
            labelDate.Text = dateFromMainForm;

            // populate the form with the data in the passed in calendar event
            char splitter = '\t';
            string[] splittedData = recurring.SavedData.Split(splitter);
            comboBoxStartTime.SelectedItem = splittedData[1].Remove(0, 11);
            // if statement to determine whether the length is 30, 60 or 90.
            //This is needed to be able to cirrectly format the currently selected item as the one
            //the user chose when they first created the event
            if (int.Parse(splittedData[2]) == 30 || int.Parse(splittedData[2]) == 60 || int.Parse(splittedData[2]) == 90)
            {
                comboBoxLength.SelectedItem = splittedData[2] + "minutes"; // no space needed as there is already one in because it picks 3 characters including a blank space
            }
            else
            {
                comboBoxLength.SelectedItem = splittedData[2] + " minutes"; // space needed as all 3 characters are full so a space is needed to correctly display it in the combo box.
            }

            // populate the subject, location, frequency and how many times fields with what has been entered in the
            // last field in the array
            char subjectLocationFrequencyAndHowManyTimesSplitter = ',';
            string[] subjectLocationFrequencyAndHowManyTimesSplittedData = splittedData[3].Split(subjectLocationFrequencyAndHowManyTimesSplitter);
            textBoxSubject.Text = subjectLocationFrequencyAndHowManyTimesSplittedData[0];
            textBoxLocation.Text = subjectLocationFrequencyAndHowManyTimesSplittedData[1];







            // Convert the number that is saved in the file to the selected item in the combo box frequency
            string comboBoxFrequencySelectedItem = "";
            switch (int.Parse(subjectLocationFrequencyAndHowManyTimesSplittedData[2].ToString()))
            {
                case 0: // Daily
                    comboBoxFrequencySelectedItem = "Daily";
                    break;
                case 1: // Weekly
                    comboBoxFrequencySelectedItem = "Weekly";
                    break;
                case 2: // Fornightly
                    comboBoxFrequencySelectedItem = "Fornightly";
                    break;
                case 3: // Monthly
                    comboBoxFrequencySelectedItem = "Monthly";
                    break;
                case 4: // Yearly
                    comboBoxFrequencySelectedItem = "Yearly";
                    break;
                default:
                    break;
            }
            comboBoxFrequency.SelectedItem = comboBoxFrequencySelectedItem;
            textBoxHowManyTimes.Text = subjectLocationFrequencyAndHowManyTimesSplittedData[3];
            this.ShowDialog(); // show the dialog box by calling the standard ShowDialog method
            return DialogResult; // return the DialogResult. This is used by the if statement that is contains the ShowDialog method
        }

        private void AddRecurringAppointment_Load(object sender, EventArgs e)
        {
            this.AcceptButton = buttonSave;
        }

        private void buttonSave_Click_1(object sender, EventArgs e)
        {
            // validation code
            int howManyTimes;
            bool validationFailed = false;

            if (textBoxSubject.Text == "")
            {
                textBoxSubject.BackColor = Color.Red;
                validationFailed = true;
            }

            else if (textBoxLocation.Text == "")
            {
                textBoxLocation.BackColor = Color.Red;
                validationFailed = true;
            }

            else if (!int.TryParse(textBoxHowManyTimes.Text, out howManyTimes) || howManyTimes < 0 || howManyTimes > 999)
            {
                textBoxHowManyTimes.BackColor = Color.Red;
                validationFailed = true;
            }

            if (validationFailed) // the validation failes
            {
                // Show a message box that tells the user hasn't entered a valid value.
                MessageBox.Show("Please enter a value in the boxes highligheted in red", "Validation Failed");
            }

            else // the validation was a success
            {
                // Variables to hold the data entered.
                string finalData;
                string dateWithoutTime = date.Date.ToString().Substring(0, 10);
                string start = dateWithoutTime + ' ' + comboBoxStartTime.Text;
                string length = comboBoxLength.Text.Substring(0, 3);
                // switch statement to convert combo box frequency selected item to int
                //then convert that int to string and store in displayText (which will go to the file as well)
                // do other stuff to help with enum thingy
                int enumFrequencyNumber = 0;
                switch (comboBoxFrequency.SelectedItem.ToString())
                {
                    case "Daily":
                        enumFrequencyNumber = 0;
                        break;
                    case "Weekly":
                        enumFrequencyNumber = 1;
                        break;
                    case "Fornightly":
                        enumFrequencyNumber = 2;
                        break;
                    case "Monthly":
                        enumFrequencyNumber = 3;
                        break;
                    case "Yearly":
                        enumFrequencyNumber = 4;
                        break;
                    default:
                        break;
                }
                string displayText = textBoxSubject.Text + "," + textBoxLocation.Text +  "," + enumFrequencyNumber.ToString() + "," + textBoxHowManyTimes.Text;
                //RecurringFrequency RecurringFrequency = (RecurringFrequency)int.Parse(comboBoxFrequency.SelectedItem.ToString());
                finalData = Convert.ToString(2) + '\t' + start + '\t' + length + '\t' + displayText;
                RecurringCalendarEvent repeatingEvent = new RecurringCalendarEvent(finalData);
                // setting the onject values to the values that are in the single calendar event class
                _RepeatingEvent = repeatingEvent;
                DialogResult = DialogResult.OK; // closes the dialog box when the event is created
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
