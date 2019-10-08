using System;
using System.Text;

namespace Anden_2.Classes
{
    public static class EpcConvertHexAsc
    {
        public static string From_hex(string s) 
        {
            StringBuilder sb = new StringBuilder();
            string[] a = s.Split(new char[] { ' ' });
            foreach (var h in a)
            {
                sb.Append((char)int.Parse(h, System.Globalization.NumberStyles.HexNumber));
            }
            return sb.ToString();
        }

        public static string HexToAscii(string hexString)
        {
            hexString = hexString.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexString.Length - 2; i += 2)
            {
                string nullyfe = "00";
                if (hexString.Substring(i, 2) != nullyfe)
                {
                    var isValid = Convert.ToString(Convert.ToChar(Int32.Parse(hexString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber)));
                    if ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ@".Contains(isValid))
                    {
                        
                        sb.Append(isValid);
                    }

                }
            }
            return sb.ToString();
        }

        static bool Contains(this string KeyString, char c)
        {
            var result = KeyString.IndexOf(c) >= 0;
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ascii"></param>
        /// <returns>Return convert ascii to hexa</returns>
        public static string AsciiToHex(string ascii)
        {
            StringBuilder sb = new StringBuilder();
            string hexOutput = string.Empty;
            char[] values = ascii.ToCharArray();
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                hexOutput = String.Format("{0:X}", value);
                sb.Append(hexOutput);
            }

            return sb.ToString();
        }
    }
}

//isValid = isValid.Replace("@", string.Empty);
