using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{

    public class TimeTrackerPanel : Panel
    {

            private TimeTracker timeTracker;
    
            public TimeTrackerPanel(TimeTracker timeTracker)
            {
    
                this.timeTracker = timeTracker;
    
                InitializeComponent();
    
            }
    
            private void InitializeComponent()
            {
    
                // Initialize UI components for the Time Tracker panel
                // This could include buttons for PunchIn, PunchOut, and displaying status


    
            }

        // Methods to handle user interactions like PunchIn, PunchOut, etc.

    }

}
