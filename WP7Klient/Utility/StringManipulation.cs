using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WP7Klient.Utility
{
    public class StringManipulation
    {

        public static string SanitizeStatus(string status)
        {
            string blockedChars = @"!()*'";

            foreach (char c in blockedChars)
            {
                if (status.IndexOf(c) != -1)
                {
                    status = status.Replace(c.ToString(), "%" + String.Format("{0:X}", Convert.ToInt32(c)));
                }
            }

            return status;
        }

        public static string TimeFormatting (String time)
        {

            return "/ " + DateTime.ParseExact(time, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture).ToString("HH:mm dd.MM.yyyy ", CultureInfo.InvariantCulture);
        }
    }
}
