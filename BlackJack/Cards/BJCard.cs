using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Cards
{
    internal interface BJCard
    {
        string name();
        int value();
        int value(int count);
    }
}
