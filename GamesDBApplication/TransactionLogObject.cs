using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDBAccess
{
    class TransactionLogObject : LogObject
    {
        string Trans;
        string Game;
        string System;
        string Format;

        public TransactionLogObject(string transaction, string game, string system, string format)
        {
            LogType = "T";

            Trans= transaction;
            Game = game;
            System = system;
            Format = format;

            CreateLog();
        }

        public override void CreateLog()
        {
            Log = string.Concat("Transaction Type: ", Trans, ", Game Name: ", Game, ", System: ", System, ", Format: ", Format);
        }
    }
}
