using BlackJack.Cards;
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
        CardsActions CA = new CardsActions();
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
                    if (sk < 0 || sk > 8)
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

            for (int i = 0; i < sk; i++)
            {
                Debug.Write((i+1)+" DEKAS: ");
                CA.dekai[i].ForEach((card) => Debug.Write(card.name() + " "));
                Debug.Write("\n");
            }
            startas();
        }

        void startas()
        {
            int dekas = new Random().Next(CA.dekai.Count);
            for (int i = 0; i<2; i++)
            {
                
                Debug.WriteLine("Dekas: " + (dekas+1) + "; Zaidejas gauna: " + CA.dekai[dekas][1].name() + "; Dyleris gauna: " + CA.dekai[dekas][0].name());
                konsole.kortos.Add((BJCard)CA.dekai[dekas][0]);
                CA.dekai[dekas].RemoveAt(0);
                dyleris.kortos.Add((BJCard)CA.dekai[dekas][0]);
                CA.dekai[dekas].RemoveAt(0);
            }


            //Console.WriteLine(String.Join(", ", zaidejo.Select(korta => korta.name()).ToArray()));
            //Console.WriteLine(String.Join(", ", dylerio.Select(korta => korta.name()).ToArray()));
            konsole.taskai = taskuSkaiciavimas(ZaidTipas.KONSOLE);
            dyleris.taskai = taskuSkaiciavimas(ZaidTipas.DYLERIS);
            KortuIsvedimas(ZaidTipas.KONSOLE, false);
            KortuIsvedimas(ZaidTipas.DYLERIS, false);

            while (true)
            {
                Console.WriteLine("");
                if (konsole.taskai == dyleris.taskai && konsole.paskutinisStatymas)
                {
                    Console.WriteLine("LYGU");
                    KortuIsvedimas(ZaidTipas.KONSOLE, false);
                    KortuIsvedimas(ZaidTipas.DYLERIS, true);
                    break;
                }
                else if (konsole.taskai == BLACKJACK)
                {
                    Console.WriteLine("KONSOLE BLACKJACK");
                    KortuIsvedimas(ZaidTipas.KONSOLE, false);
                    KortuIsvedimas(ZaidTipas.DYLERIS, true);
                    break;
                }
                else if (dyleris.taskai == BLACKJACK)
                {
                    Console.WriteLine("DYLERIS BLACKJACK");
                    KortuIsvedimas(ZaidTipas.KONSOLE, false);
                    KortuIsvedimas(ZaidTipas.DYLERIS, true);
                    break;
                }
                else if (dyleris.taskai > BLACKJACK)
                {
                    Console.WriteLine("DYLERIS PRALAIMI");
                    KortuIsvedimas(ZaidTipas.KONSOLE, false);
                    KortuIsvedimas(ZaidTipas.DYLERIS, true);
                    break;
                }
                else if (konsole.taskai > BLACKJACK) {
                    Console.WriteLine("TU PRALAIMI");
                    KortuIsvedimas(ZaidTipas.KONSOLE, false);
                    KortuIsvedimas(ZaidTipas.DYLERIS, true);
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
                        paimtiKorta(dekas, ZaidTipas.KONSOLE);
                        konsole.taskai = taskuSkaiciavimas(ZaidTipas.KONSOLE);
                        KortuIsvedimas(ZaidTipas.KONSOLE, false);
                        KortuIsvedimas(ZaidTipas.DYLERIS, false);
                    }
                    continue;
                }
                else if (dyleris.taskai > konsole.taskai)
                {
                    Console.WriteLine("TU PRALAIMI");
                    KortuIsvedimas(ZaidTipas.KONSOLE, false);
                    KortuIsvedimas(ZaidTipas.DYLERIS, true);
                    break;
                }
                else if (dyleris.taskai < konsole.taskai && dyleris.paskutinisStatymas)
                {
                    Console.WriteLine("TU LAIMI");
                    KortuIsvedimas(ZaidTipas.KONSOLE, false);
                    KortuIsvedimas(ZaidTipas.DYLERIS, true);
                    break;
                }
                else if (dyleris.taskai < BLACKJACK && konsole.paskutinisStatymas)
                {
                    paimtiKorta(dekas, ZaidTipas.DYLERIS);
                    dyleris.taskai = taskuSkaiciavimas(ZaidTipas.DYLERIS);
                    if (dyleris.taskai >= 20) dyleris.paskutinisStatymas = true;
                    KortuIsvedimas(ZaidTipas.KONSOLE, false);
                    KortuIsvedimas(ZaidTipas.DYLERIS, true);
                    continue;
                }
                



                break;
            }

        }

        void paimtiKorta(int dekas, ZaidTipas zaidejas)
        {
            if (zaidejas == ZaidTipas.KONSOLE)
            {
                Debug.WriteLine("Dekas: " + (dekas + 1) + "; Zaidejas gauna: " + CA.dekai[dekas][0].name() + ";");
                konsole.kortos.Add((BJCard)CA.dekai[dekas][0]);
            } else
            {
                Debug.WriteLine("Dekas: " + (dekas + 1) + "; Dyleris gauna: " + CA.dekai[dekas][0].name() + ";");
                dyleris.kortos.Add((BJCard)CA.dekai[dekas][0]);
            }
            CA.dekai[dekas].RemoveAt(0);
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

        void KortuIsvedimas(ZaidTipas zaidejas, bool paskutinisEjimas)
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
            try
            {
                return korta.value();
            }
            catch (Exception)
            {
                return korta.value(points);
            }
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
