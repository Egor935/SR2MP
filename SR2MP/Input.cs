using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP
{
    public class Input
    {
        [DllImport("user32.dll")]
        static extern int GetAsyncKeyState(int vKey);

        public static bool GetKeyDown(int key)
        {
            if (0 != (GetAsyncKeyState(key) & 0x8000))
            {
                if (!BackQuote)
                {
                    BackQuote = true;
                    return true;
                }
            }
            else
            {
                BackQuote = false;
            }
            return false;
        }

        static bool BackQuote;
    }
}
