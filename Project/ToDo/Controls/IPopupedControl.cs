using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToDo.Controls
{
    interface IPopupedControl
    {
        event EventHandler Closed;
        event EventHandler Opened;
    }
}
