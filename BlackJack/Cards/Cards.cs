using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Cards
{
    internal class C2 : BJCard
    {
        public string name()
        {
            return "2";
        }

        public int value()
        {
            return 2;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class C3 : BJCard
    {
        public string name()
        {
            return "3";
        }

        public int value()
        {
            return 3;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class C4 : BJCard
    {
        public string name()
        {
            return "4";
        }

        public int value()
        {
            return 4;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class C5 : BJCard
    {
        public string name()
        {
            return "5";
        }

        public int value()
        {
            return 5;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class C6 : BJCard
    {
        public string name()
        {
            return "6";
        }

        public int value()
        {
            return 6;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class C7 : BJCard
    {
        public string name()
        {
            return "7";
        }

        public int value()
        {
            return 7;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class C8 : BJCard
    {
        public string name()
        {
            return "8";
        }

        public int value()
        {
            return 8;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class C9 : BJCard
    {
        public string name()
        {
            return "9";
        }

        public int value()
        {
            return 9;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class C10 : BJCard
    {
        public string name()
        {
            return "10";
        }

        public int value()
        {
            return 10;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class CJ : BJCard
    {
        public string name()
        {
            return "J";
        }

        public int value()
        {
            return 10;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class CQ : BJCard
    {
        public string name()
        {
            return "Q";
        }

        public int value()
        {
            return 10;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class CK : BJCard
    {
        public string name()
        {
            return "K";
        }

        public int value()
        {
            return 10;
        }

        public int value(int count)
        {
            throw new NotSupportedException();
        }
    }

    internal class CA : BJCard
    {
        public string name()
        {
            return "A";
        }

        public int value()
        {
            throw new NotSupportedException();
        }

        public int value(int count)
        {
            if (count+11 <= 21)
            {
                return 11;
            }
            else
            {
                return 1;
            }
        }
    }
}
