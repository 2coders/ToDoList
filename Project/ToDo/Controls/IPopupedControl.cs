using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDo.Controls
{
    public class PopupEventArgs : EventArgs
    {
        private bool done = false;
        public bool Done
        {
            get { return done; }
            set { done = value; }
        }
    }

    public delegate void PopupEventHandler(object sender, PopupEventArgs e);

    interface IPopupedControl
    {
        event PopupEventHandler Closed;
        event PopupEventHandler Opened;

        bool IsCanceled {get;set;}
    }
}
