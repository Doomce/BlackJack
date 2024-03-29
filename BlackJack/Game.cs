﻿using BlackJack.Cards;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace BlackJack
{
    
    enum ZaidTipas
    {
        DYLERIS, KONSOLE
    }

    struct Zaidejas
    {
        public List<BJCard> kortos { get; set; } = new List<BJCard>();
        public int taskai { get; set; } = 0;
        public bool paskutinisStatymas { get; set; } = false;

        public Zaidejas()
        {
        }
    }

    internal class Game
    {
        private const int BLACKJACK = 21;
        private CardsActions CA = new CardsActions();
        private Zaidejas dyleris = new Zaidejas();
        private Zaidejas konsole = new Zaidejas();

        public Game()
        {
            int sk = 1;
            while (true)
            {
                var key = Console.ReadKey().KeyChar;
                try
                {
                    sk = Int16.Parse(key.ToString());
                    if (sk < 1 || sk > 8)
                    {
                        throw new FormatException();
                    }
                    Console.WriteLine("");
                } catch (Exception ex) when (ex is FormatException || ex is ArgumentNullException || ex is OverflowException) {
                    Console.WriteLine("\nNe skaicius");
                    continue;
                }
                break;
            }

            CA.init(sk);

            startas();
        }

        void startas()
        {
            int kalade = new Random().Next(CA.dekai.Count);
            for (int i = 0; i<2; i++)
            {
                paimtiKorta(ZaidTipas.KONSOLE);
                paimtiKorta(ZaidTipas.DYLERIS);
            }

            konsole.taskai = taskuSkaiciavimas(ZaidTipas.KONSOLE);
            dyleris.taskai = taskuSkaiciavimas(ZaidTipas.DYLERIS);
            kortuIsvedimas(false);

            while (true)
            {
                Console.WriteLine("");
                if (konsole.taskai == dyleris.taskai && konsole.paskutinisStatymas && dyleris.paskutinisStatymas)
                {
                    Console.WriteLine("LYGU");
                    kortuIsvedimas(true);
                    break;
                }
                else if (konsole.taskai == BLACKJACK)
                {
                    Console.WriteLine("KONSOLE BLACKJACK - LAIMI");
                    kortuIsvedimas(true);
                    break;
                }
                else if (dyleris.taskai == BLACKJACK)
                {
                    Console.WriteLine("DYLERIS BLACKJACK - LAIMI");
                    kortuIsvedimas(true);
                    break;
                }
                else if (dyleris.taskai > BLACKJACK)
                {
                    Console.WriteLine("TU LAIMI");
                    kortuIsvedimas(true);
                    break;
                }
                else if (konsole.taskai > BLACKJACK) {
                    Console.WriteLine("DYLERIS LAIMI");
                    kortuIsvedimas(true);
                    break;
                }       
                else if (!konsole.paskutinisStatymas)
                {
                    Console.WriteLine("H - imti kortą;  S- Praleisti");
                    var ivestis = Console.ReadKey().KeyChar;
                    Console.WriteLine("");
                    if (ivestis.Equals('s'))
                    {
                        konsole.paskutinisStatymas = true;
                    }
                    else if (ivestis.Equals('h'))
                    {
                        paimtiKorta(ZaidTipas.KONSOLE);
                        konsole.taskai = taskuSkaiciavimas(ZaidTipas.KONSOLE);
                        kortuIsvedimas(false);
                    }
                    continue;
                }
                else if (dyleris.taskai > konsole.taskai)
                {
                    Console.WriteLine("DYLERIS LAIMI");
                    kortuIsvedimas(true);
                    break;
                }
                else if (dyleris.taskai < konsole.taskai && dyleris.paskutinisStatymas)
                {
                    Console.WriteLine("TU LAIMI");
                    kortuIsvedimas(true);
                    break;
                }
                else if (dyleris.taskai < BLACKJACK && konsole.paskutinisStatymas)
                {
                    paimtiKorta(ZaidTipas.DYLERIS);
                    dyleris.taskai = taskuSkaiciavimas(ZaidTipas.DYLERIS);
                    if (dyleris.taskai > 17) dyleris.paskutinisStatymas = true;
                    kortuIsvedimas(true);
                    continue;
                }
                break;
            }

        }

        void paimtiKorta(ZaidTipas zaidejas)
        {
            int kalade = new Random().Next(CA.dekai.Count);
            if (zaidejas == ZaidTipas.KONSOLE)
            {
                Debug.WriteLine("Dekas: " + (kalade + 1) + "; Zaidejas gauna: " + CA.dekai[kalade][0].name() + ";");
                konsole.kortos.Add((BJCard)CA.dekai[kalade][0]);
            } else
            {
                Debug.WriteLine("Dekas: " + (kalade + 1) + "; Dyleris gauna: " + CA.dekai[kalade][0].name() + ";");
                dyleris.kortos.Add((BJCard)CA.dekai[kalade][0]);
            }
            CA.dekai[kalade].RemoveAt(0);
        }

        private int taskuSkaiciavimas(ZaidTipas zaidejas)
        {
            int points = 0;
            Zaidejas zaid;
            if (zaidejas == ZaidTipas.KONSOLE) zaid = konsole;
            else zaid = dyleris;

            /*for (int i = 0; i < zaid.kortos.Count; i++)
            {
                if (!zaid.kortos[i].name().Equals("A")) points += kortosVerte(zaid.kortos[i], points, i, zaid.kortos.Count-1);
            }*/
            for (int i = 0; i < zaid.kortos.Count; i++)
            {
                points += kortosVerte(zaid.kortos[i], points, i, zaid.kortos.Count - 1);
            }
            return points;
        }


        private void kortuIsvedimas(bool paskutinisEjimas)
        {
            Isvedimas(ZaidTipas.KONSOLE, false);
            Isvedimas(ZaidTipas.DYLERIS, paskutinisEjimas);
        }

        private void Isvedimas(ZaidTipas zaidejas, bool paskutinisEjimas)
        {
            int points;
            if (zaidejas == ZaidTipas.KONSOLE)
            {
                points = konsole.taskai;
                Console.WriteLine("Tavo kortos: " + String.Join(", ", konsole.kortos.Select(korta => korta.name()).ToArray()) + "; Taskai: " + points);
                Debug.WriteLine("Konsoles taskai: " + points);
            }
            else
            {
                points = dyleris.taskai;
                char[] dylKortos = new char[dyleris.kortos.Count];
                for (int i = 0; i < (paskutinisEjimas ? dyleris.kortos.Count : dyleris.kortos.Count - 1); i++)
                {
                    dylKortos[i] += dyleris.kortos[i].name()[0];
                }
                if (!paskutinisEjimas) dylKortos[dyleris.kortos.Count - 1] = '?';
                Console.WriteLine("Dylerio kortos: " + String.Join(", ", dylKortos) + (paskutinisEjimas ? "; Taskai: " + points : ""));
                Debug.WriteLine("Dylerio taskai: " + points);
            }
        }

        private int kortosVerte(BJCard korta, int points, int indeksas, int maxIndeksas)
        {
            //Skaiciuoti pagal A kortos pozicija ar ne????????

            try
            {
                return korta.value();
            }
            catch (Exception)
            {
                return korta.value(points);
            }
        }


        private static void debugPrint(List<BJCard> cards)
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
