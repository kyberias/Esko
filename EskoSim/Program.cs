using System;
using System.Collections.Generic;
using System.Text;

namespace EskoSim
{
    class Nauha
    {
        Queue<char> jono;

        public Nauha(string s)
        {
            s = s.Replace("\n", "");
            s = s.Replace(" ", "");
            s = s.Replace("\r", "");
            s = s.Replace("\t", "");

            jono = new Queue<char>(s);
        }

        public char Kurkkaa()
        {
            return jono.Peek();
        }

        public char Lue()
        {
            var c = jono.Dequeue();
            jono.Enqueue(c);
            return c;
        }
    }

    class Luku
    {
        public Luku()
        {

        }

        public override string ToString()
        {
            return Arvo + (Tunnusmerkki ? "*" : "");
        }

        public Luku(double arvo)
        {
            Arvo = arvo;
        }

        public double Arvo { get; set; }
        public bool Tunnusmerkki { get; set; }

        public static Luku operator +(Luku a, Luku b)
        {
            return new Luku(a.Arvo + b.Arvo)
            {
                Tunnusmerkki = a.Tunnusmerkki || b.Tunnusmerkki
            };
        }

        public static Luku operator -(Luku a, Luku b)
        {
            return new Luku(a.Arvo - b.Arvo)
            {
                Tunnusmerkki = a.Tunnusmerkki || b.Tunnusmerkki
            };
        }

        public static Luku operator *(Luku a, Luku b)
        {
            return new Luku(a.Arvo * b.Arvo)
            {
                Tunnusmerkki = a.Tunnusmerkki || b.Tunnusmerkki
            };
        }

        public static Luku operator /(Luku a, Luku b)
        {
            return new Luku(a.Arvo / b.Arvo)
            {
                Tunnusmerkki = a.Tunnusmerkki || b.Tunnusmerkki
            };
        }

        public static Luku Nolla = new Luku(0);
    }

    class Muisti<T> where T: new()
    {
        T[,] urat = new T[31,60];

        int[] uranvalitsimet = new int[10];

        public void Kirjoita(string osoite, T luku)
        {
            var o = TeeOsoite(osoite);
            urat[o.Ura, o.Sektori] = luku;
        }

        class Osoite
        {
            public int Ura { get; set; }
            public int Sektori { get; set; }
        }

        Osoite TeeOsoite(int uranvalitsin, int sektori)
        {
            return new Osoite
            {
                Ura = uranvalitsimet[uranvalitsin],
                Sektori = sektori
            };
        }

        Osoite TeeOsoite(string o)
        {
            if (o.Length == 2)
            {
                return new Osoite
                {
                    // Työmuisti!
                    Ura = 30,
                    Sektori = int.Parse(o) - 60
                };
            }
            else
            {
                return TeeOsoite(int.Parse(o.Substring(2, 1)), int.Parse(o.Substring(0, 2)));
            }
        }

        public void Kirjoita(int uranvalitsin, int sektori, T luku)
        {
            var o = TeeOsoite(uranvalitsin, sektori);
            urat[o.Ura, o.Sektori] = luku;
        }

        public T ViimeksiLuettu = new T();

        public T Lue(int uranvalitsin, int sektori)
        {
            var o = TeeOsoite(uranvalitsin, sektori);
            var d = urat[o.Ura, o.Sektori];
            if (d == null)
            {
                d = urat[o.Ura, o.Sektori] = new T();
            }
            ViimeksiLuettu = d;
            return d;
        }

        public T Lue(string osoite)
        {
            var o = TeeOsoite(osoite);
            var d = urat[o.Ura,o.Sektori];
            if(d == null)
            {
                d = urat[o.Ura, o.Sektori] = new T();
            }
            ViimeksiLuettu = d;
            return d;
        }

        public void KytkeUranvalitsinUraan(int valitsin, int ura)
        {
            uranvalitsimet[valitsin] = ura;
        }
    }

    class Esko
    {
        Luku akku;

        Nauha[] nauhat = new Nauha[10];

        int nauha;

        public Muisti<Luku> Muisti { get; set; } = new Muisti<Luku>();

        char Lue()
        {
            return nauhat[nauha].Lue();
        }

        public void AsetaNauha(int nro, Nauha n)
        {
            nauhat[nro] = n;
        }

        int edellinenLaite;
        bool päällä;
        bool hyppyKäynnissä;

        public void Aja()
        {
            akku = new Luku(0);
            päällä = true;
            while(päällä)
            {
                var komento = Lue();

                //Console.WriteLine($"akku: {akku}, komento: {komento}");

//                Console.ReadKey();

                if(hyppyKäynnissä && komento != 'e')
                {
                    continue;
                }

                switch(komento)
                {
                    case '+':
                        {
                            var osoite = LueOsoite();
                            var luku = Muisti.Lue(osoite);
                            akku = akku + luku;
                        }
                        break;
                    case '-':
                        {
                            var osoite = LueOsoite();
                            var luku = Muisti.Lue(osoite);
                            akku = akku - luku;
                        }
                        break;
                    case ':':
                        {
                            var n = LueOsoite();
                            var luku = Muisti.Lue(n);
                            akku = akku / luku;
                        }
                        break;
                    case '|':
                        {
                            akku.Arvo = Math.Abs(akku.Arvo);
                        }
                        break;
                    case 'x':
                        {
                            var n = LueOsoite();
                            var luku = Muisti.Lue(n);
                            akku = akku * luku;
                        }
                        break;
                    case 'r':
                        {
                            akku = new Luku(Math.Abs(akku.Arvo));
                        }
                        break;
                    case '⇴':
                        {
                            var osoite = LueOsoite();
                            Muisti.Kirjoita(osoite, akku);
                            akku = Luku.Nolla;
                        }
                        break;
                    case '→':
                        {
                            var osoite = LueOsoite();
                            Muisti.Kirjoita(osoite, akku);
                        }
                        break;
                    case 'v':
                        {
                            var valitsija = Lue();
                            var osoite = LueOsoite();
                            Muisti.Kirjoita(osoite, akku);
                        }
                        break;
                    case 'k':
                        {
                            var osoite = LueOsoite();
                            var tyyppi = int.Parse(osoite[2].ToString());
                            if(tyyppi != 9)
                            {
                                Console.Write(akku.Arvo);
                            }
                            akku = Luku.Nolla;
                        }
                        break;
                    case 's': // Syklinen permutaatio
                        {
                            var osoite = LueOsoite();

                            var n = int.Parse(osoite.Substring(0, 2));
                            var uranvalitsin = int.Parse(osoite[2].ToString());

                            var nSis = Muisti.Lue($"01{uranvalitsin}");

                            for(int i=1;i<n;i++)
                            {
                                Muisti.Kirjoita(uranvalitsin, i, Muisti.Lue(uranvalitsin, i + 1));
                            }

                            Muisti.Kirjoita(uranvalitsin, n, nSis);
                        }
                        break;
                    case 't':
                        {
                            Console.Write("\t");
                        }
                        break;
                    case 'e':
                        {
                            var ehto = Lue();
                            long bits = BitConverter.DoubleToInt64Bits(akku.Arvo);
                            // Note that the shift is sign-extended, hence the test against -1 not 1
                            bool negative = (bits < 0);
                            int exponent = (int)((bits >> 52) & 0x7ffL);
                            long mantissa = bits & 0xfffffffffffffL;
                            bool tulos;
                            switch(ehto)
                            {
                                case '0':
                                    tulos = negative;//mantissa < 0;
                                    break;
                                case '1':
                                    tulos = !negative;// mantissa > 0;
                                    break;
                                case '2':
                                    tulos = exponent < 0;
                                    break;
                                case '3':
                                    tulos = exponent > 0;
                                    break;
                                case '4':
                                    tulos = !Muisti.ViimeksiLuettu.Tunnusmerkki;
                                    break;
                                case '5':
                                    tulos = Muisti.ViimeksiLuettu.Tunnusmerkki;
                                    break;
                                case 'e':
                                    hyppyKäynnissä = false;
                                    tulos = false;
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }

                            if (tulos)
                            {
                                // Skip to next 'ee'
                                hyppyKäynnissä = true;
                            }
                        }
                        break;
                    case 'a':
                        {
                            var laite = Lue();
                            if(laite == 'a')
                            {
                                päällä = false;
                            }
                            else if(laite == '*')
                            {
                                nauha = edellinenLaite;
                            }
                            else if (laite == 't')
                            {
                                Console.WriteLine();
                            }
                            else
                            {
                                edellinenLaite = nauha;
                                nauha = int.Parse(laite.ToString());
                            }
                        }
                        break;
                    case '*': // Varusta seuraavan o-> n tai -> n käskyn luku tunnusmerkillä
                        {
                            throw new NotSupportedException();
                        }
                    default:
                        throw new NotImplementedException($"Käskyä {komento} ei ole toteutettu.");
                }
            }
        }

        string LueOsoite()
        {
            var builder = new StringBuilder();

            while(char.IsDigit(nauhat[nauha].Kurkkaa()))
            {
                builder.Append(nauhat[nauha].Lue());
            }
            return builder.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var n0 = new Nauha("+010 →61 a 1 aa");
            var n1 = new Nauha("⇴90 +61 ⇴60 +010 :60 +60 x70 →61 -60 | -71 e1 a2 ee");
            var n2 = new Nauha("⇴90 +010 k102 t +61 k102 at s 260 +010 →61 e4 a0 ee a*");

            var esko = new Esko();
            esko.AsetaNauha(0, n0);
            esko.AsetaNauha(1, n1);
            esko.AsetaNauha(2, n2);

            esko.Muisti.Kirjoita("70", new Luku(0.5));
            esko.Muisti.Kirjoita("71", new Luku(0.1));
            esko.Muisti.Kirjoita("010", new Luku(6000) { Tunnusmerkki = false });
            esko.Muisti.Kirjoita("020", new Luku(5000) { Tunnusmerkki = false });
            esko.Muisti.Kirjoita("030", new Luku(4000) { Tunnusmerkki = false });
            esko.Muisti.Kirjoita("040", new Luku(3000) { Tunnusmerkki = false });
            esko.Muisti.Kirjoita("050", new Luku(64) { Tunnusmerkki = false });
            esko.Muisti.Kirjoita("060", new Luku(64) { Tunnusmerkki = true });

            esko.Aja();

            // Pääohjelma
            var nauha1 = new Nauha(@"
⇴ 98
av2
0lp\00
→ 66
x 66
⇴ 67
→ 63
a 2
⇴ 60
a 2
⇴ 61
a 2
⇴ 70
+ 61
+ 67
⇴ 65
avo
ee
⇴ 98
+ 63
+ 67
k 109
v 0**
+70
⇴ 64
a 3
⇴ 98
+ 63
+ 66
→ 63
- 60
- 67
e 0
aa
");
        }
    }
}
