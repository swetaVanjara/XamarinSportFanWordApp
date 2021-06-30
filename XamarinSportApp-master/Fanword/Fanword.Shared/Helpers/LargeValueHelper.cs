using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Shared.Helpers
{
    public class LargeValueHelper
    {
        public static string GetString(int number)
        {
            if (number < 1000)
            {
                return number.ToString();
            }
            else if (number < 1000000)
            {
                return (number/1000) + "k";
            }
            else
            {
                return (number / 1000000) + "m";
            }
        }
    }
}
