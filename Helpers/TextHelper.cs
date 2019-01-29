using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Helpers
{
    public static class TextHelper
    {
        public static string CleanString(string text)
        {
            if (String.IsNullOrEmpty(text)) return "";

            text = text.Contains("\n") ? text.Replace("\n", "") : text;
            text = text.Contains("\t") ? text.Replace("\t", "") : text;
            text = text.Contains("\r") ? text.Replace("\r", "") : text;
            text = text.Contains("&nbsp;") ? text.Replace("&nbsp;", "") : text;

            return text.Trim();
        }
    }
}