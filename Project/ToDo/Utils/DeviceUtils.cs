using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ToDo.Utils
{
    public class HeightUtils
    {
        public static double ScreenHeight(FrameworkElement root)
        {
            if (root == null)
            {
                return 0;
            }
            return root.ActualHeight;
        }

        public static double GoldSectionHeight(FrameworkElement root)
        {
            return ScreenHeight(root) * 0.618;
        }

        public static double CompletedPanelHeight(FrameworkElement root)
        {
            return ScreenHeight(root) * (1 - 0.618);
        }
    }
}
