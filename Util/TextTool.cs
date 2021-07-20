using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KakaoManagerBeta.Util
{
    public static class TextTool
    {
        public static string ToBase64(this string text)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes);
        }

        public static string ConvertBase64ToText(this string base64Text)
        {
            var textBytes = Convert.FromBase64String(base64Text);
            return Encoding.UTF8.GetString(textBytes);
        }
    }
}
