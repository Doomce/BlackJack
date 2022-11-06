using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack.Cards
{
    internal class CardsActions
    {
        public List<List<BJCard>> dekai = new List<List<BJCard>>();

        public void init(int dekuKiekis)
        {
            for (int i = 0; i < dekuKiekis; i++)
            {
                dekai.Add(padarytiDeka());
            } 
        }

        private List<BJCard> padarytiDeka()
        {
            List<BJCard> dekas = new List<BJCard>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j <= 10; j++)
                {
                    var container = Activator.CreateInstance(null, @"BlackJack.Cards.C" + j);
                    dekas.Add((BJCard)container.Unwrap());
                }
                dekas.Add(new CA());
                dekas.Add(new CQ());
                dekas.Add(new CJ());
                dekas.Add(new CK());
            }

            return maisymas(dekas);
        }

        private List<BJCard> iterpimas(BJCard obj, int slot, List<BJCard> kortos)
        {
            List<BJCard> kortos2 = new List<BJCard>();
            for (int j = 0; j < 52; j++)
            {
                if (j == slot)
                {
                    kortos2.Add(obj);
                    continue;
                }
                if (j > slot)
                {
                    kortos2.Add(kortos[j - 1]);
                    continue;
                }
                kortos2.Add(kortos[j]);
            }
            return kortos2;
        }

        private List<BJCard> maisymas(List<BJCard> kortos)
        {
            for (int i = 0; i < 52; i++)
            {
                BJCard korta = kortos.ElementAt(i);
                kortos.RemoveAt(i);

                int perKiekPoz = new Random().Next(51);
                int newSlot = i + perKiekPoz < 52 ? i + perKiekPoz : Math.Abs(52 - i - perKiekPoz);

                kortos = iterpimas(korta, newSlot, kortos);
                //Arba galima naudoti insert:
                //kortos.Insert(newSlot, korta);
            }
            return kortos;
        }




        private static void print(List<BJCard> cards)
        {
            foreach (var card in cards)
            {
                int value;
                try
                {
                    value = card.value();
                }
                catch (Exception)
                {
                    value = card.value(1);
                }
                Console.WriteLine(card.name() + " " + value);
            }

            Console.WriteLine(cards.Count);

            Console.WriteLine("Kortu kiekiai:");
            for (int j = 2; j <= 10; j++)
            {
                Console.WriteLine(j + ": " + cards.Count((card) => card.name().Equals(j + "")));
            }
            Console.WriteLine("K: " + cards.Count((card) => card.name().Equals("K")));
            Console.WriteLine("Q: " + cards.Count((card) => card.name().Equals("Q")));
            Console.WriteLine("J: " + cards.Count((card) => card.name().Equals("J")));
            Console.WriteLine("A: " + cards.Count((card) => card.name().Equals("A")));
        }

    }
}
