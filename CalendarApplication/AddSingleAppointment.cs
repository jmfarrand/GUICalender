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
    public partial class AddSingleAppointment : Form
    {
        SingleCalendarEvent _SingleEvent = null;
        DateTime date;
        Color _defaultSubjectColour;
        Color _defaultLocationColour;

        public AddSingleAppointment()
        {
            InitializeComponent();

            // get the default back colour for the text boxes so they can be reset when errors have been corrected or validation has finished
            _defaultSubjectColour = textBoxSubject.BackColor;
            _defaultLocationColour = textBoxSubject.BackColor;
        }

        //This property is for a person object to be passed
        // into the form to be displayed.
        // It is only being passed in by a reference, not
        // a full copy

        public SingleCalendarEvent SingleEvent
        {
            get
            {
                return _SingleEvent;
            }
            set
            {
                _SingleEvent = value;
            }
        }

        // main ShowDialog method that allows the date displayed on the MainForm to be displayed as the selcted date
        //for the calender event
        public DialogResult ShowDialog(string dateFromMainForm)
        {
            // set the displayed date as the one that is passed in as the paramter
            date = Convert.ToDateTime(dateFromMainForm);
            labelDate.Text = dateFromMainForm;
            this.ShowDialog(); // show the dialog using the standard ShowDialog method
            return DialogResult; // return the DialogResult. This is used for the if statement that the modal dialog box is a part of
        }

        // ShowDialog method that allows the selected calender entrey to be edited.
        public DialogResult ShowDialog(ICalendarEntry single, string dateFromMainForm)
        {
            // set the displayed date as the one that is passed in as the paramter
            date = Convert.ToDateTime(dateFromMainForm);
            labelDate.Text = dateFromMainForm;

            // populate the form with the data in the passed in calendar event
            char splitter = '\t';
            string[] splittedData = single.SavedData.Split(splitter);
            comboBoxStartTime.SelectedItem = splittedData[1].Remove(0, 11);
            // if statement to determine whether the length is 30, 60 or 90.
            //This is needed to be able to correctly format the currently selected item as the one
            //the user chose when they first created the event
            if (int.Parse(splittedData[2]) == 30 || int.Parse(splittedData[2]) == 60 || int.Parse(splittedData[2]) == 90)
            {
                comboBoxLength.SelectedItem = splittedData[2] + "minutes"; // no space needed as there is already one in because it picks 3 characters including a blank space
            }
            else
            {
                comboBoxLength.SelectedItem = splittedData[2] + " minutes"; // space needed as all 3 characters are full so a space is needed to correctly display it in the combo box.
            }

            // populate the subject and location text boxes with what has been entered in the
            //last field in the array
            char subjectAndLocationSplitter = ',';
            string[] subjectAndLocationSplittedData = splittedData[3].Split(subjectAndLocationSplitter);
            textBoxSubject.Text = subjectAndLocationSplittedData[0];
            textBoxLocation.Text = subjectAndLocationSplittedData[1];
            this.ShowDialog(); // show the dialog box by calling the standard ShowDialog method
            return DialogResult; // return the DialogResult. Used by the if statement that contains ShowDialog
        }

        private void AddAppointment_Load(object sender, EventArgs e)
        {
            this.AcceptButton = buttonSave;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // validation code
            bool validationFailed = false;

            if (textBoxSubject.Text == "") // the user didn't enter anything in the subject text box
            {
                textBoxSubject.BackColor = Color.Red;
                validationFailed = true;
            }

            else if (textBoxLocation.Text == "") // the user didn't enter anything in the location text box
            {
                textBoxLocation.BackColor = Color.Red;
                validationFailed = true;
            }

            if (validationFailed) // the validation failed
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
                string displayText = textBoxSubject.Text + "," + textBoxLocation.Text;
                finalData = Convert.ToString(1) + '\t' + start + '\t' + length + '\t' + displayText;
                SingleCalendarEvent singleEvent = new SingleCalendarEvent(finalData);
                // setting the onject values to the values that are in the single calendar event class
                _SingleEvent = singleEvent;
                DialogResult = DialogResult.OK; // close  the dialog box when the save is completed
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // the dialog box will close without any changes to the data
            DialogResult = DialogResult.Cancel;
        }
    }
}
