using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingNote.DBSource
{
    class Logger
    {
        internal static void WriteLog(Exception ex)
        {
            //string msg

            //System.IO.File.AppendAllText("C:\\Logs\\Log.log,msg");
            throw ex;
        }
    }
}
