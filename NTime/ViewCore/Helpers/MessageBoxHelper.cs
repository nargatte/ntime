using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewCore.Helpers
{
    public static class MessageBoxHelper
    {
        public static MessageBoxResult DisplayYesNo(string questionToDisplay)
        {
            return MessageBox.Show(questionToDisplay, $"",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
