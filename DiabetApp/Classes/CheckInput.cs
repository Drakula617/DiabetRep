using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiabetApp.Classes
{
    public class CheckInput
    {
        string previous_str;
        public string Previous_str
        {
            get { return previous_str; }
            set { previous_str = value; }
        }
        string present_str;
        public string Present_str
        {
            get { return present_str; }
            set { present_str = value; }
        }

        float previous_number;
        public float Previous_number
        {
            get { return previous_number; }
            set { previous_number = value; }
        }
        string present_number;
        public string Present_number
        {
            get { return present_number; }
            set { present_number = value; }
        }
        public CheckInput()
        {
            
        }

        public float CheckNumber(string num)
        {
            num = num.Replace('.', ',');
            try
            {
                if (Convert.ToDouble(num) > 0)
                {
                    Previous_number = (float)Convert.ToDouble(num);
                    return (float)Convert.ToDouble(num);
                }
                else
                {
                    return (float)Previous_number;
                }
            }
            catch
            {
                return Previous_number;
            }
        }

        public string CheckLetter(string str)
        {
            if (Regex.IsMatch(str, "[^a-z]") || Regex.IsMatch(str, "[^A-Z]") || Regex.IsMatch(str, "[^А-Я]") || Regex.IsMatch(str, "[^А-Я]"))
            {
                return str;
            }
            else
            {
                return Previous_str;
            }
        }
    }
}
