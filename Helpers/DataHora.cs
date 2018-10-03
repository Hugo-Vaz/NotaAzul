using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaAzul.Helpers
{
    public class DataHora
    {
        /// <summary>
        /// Retorna a hora de acordo com o TimeZone de Brasilia (E. South America Standard Time)
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCurrentNow()
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "E. South America Standard Time");
        }
    }
}