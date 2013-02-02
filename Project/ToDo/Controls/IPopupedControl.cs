using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDo.Controls
{
    public class PopupEventArgs : EventArgs
    {
 
    }

    public delegate void PopupEventHandler(object sender, PopupEventArgs e);

    interface IPopupedControl
    {
        event PopupEventHandler Closed;
        event PopupEventHandler Opened;
    }
}
