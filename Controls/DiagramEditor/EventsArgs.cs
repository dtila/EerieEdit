using System;
using System.Collections.Generic;
using System.Text;

namespace EerieEdit
{
    public class CancelEventsArgs : EventArgs
    {
        public bool Cancel { get; set; }

        public CancelEventsArgs(bool cancel)
        {
            Cancel = cancel;
        }
    }
}
