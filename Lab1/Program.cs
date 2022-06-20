using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Lab1

{
    class Program
    {
        class Polinom
        {
            private List<Monomial> monom;


            public Polinom(List<Monomial> monomials)
            {
                this.monom = monomials;
            }


            public List<Monomial> Monom { get => monom; set => monom = value; }


            public Polinom sum(Polinom other)
            {
                List<Monomial> result = new List<Monomial>();
                for (int i=0; i<Monom.Count; i++)
                {
                    Monomial left = Monom[i];
                    Monomial right = other.Monom[i];

                    Monomial sum = new Monomial(left.Power, left.Name, left.Coef + right.Coef);
                    result.Add(sum);
                }

                return new Polinom(result);
            }


            public Polinom subtraction(Polinom other)
            {
                List<Monomial> result = new List<Monomial>();
                for (int i = 0; i < Monom.Count; i++)
                {
                    Monomial left = Monom[i];
                    Monomial right = other.Monom[i];

                    Monomial sum = new Monomial(left.Power, left.Name, left.Coef - right.Coef);
                    result.Add(sum);
                }

                return new Polinom(result);
            }


            public bool contains(Monomial monomial)
            {
                int index = Monom.FindIndex(e => e.Equals(monomial));
                return index != -1;
            }


            public Polinom multiply(Polinom other)
            {
                List<Monomial> result = new List<Monomial>();
                for (int i = 0; i < Monom.Count; i++)
                {
                    Monomial left = Monom[i];
                    Monomial right = other.Monom[i];

                    Monomial sum = new Monomial(left.Power + right.Power, left.Name, left.Coef * right.Coef);
                    result.Add(sum);
                }

                return new Polinom(result);
            }


            public Polinom divide(Polinom other)
            {
                List<Monomial> result = new List<Monomial>();
                for (int i = 0; i < Monom.Count; i++)
                {
                    Monomial left = Monom[i];
                    Monomial right = other.Monom[i];

                    Monomial sum = new Monomial(left.Power - right.Power, left.Name, left.Coef / right.Coef);
                    result.Add(sum);
                }

                return new Polinom(result);
            }


            public bool equals(Polinom other)
            {
                for (int i = 0; i < Monom.Count; i++)
                {
                    Monomial left = Monom[i];
                    Monomial right = other.Monom[i];

                    if (!left.Equals(right))
                    {
                        return false;
                    }
                }

                return true;
            }


            public Polinom compact()
            {
                List<Monomial> result = new List<Monomial>();
                Dictionary<int, Boolean> taken = new Dictionary<int, bool>();
                for (int i = 0; i < Monom.Count; i++)
                {
                    Monomial m = Monom[i];

                    Monomial r = new Monomial(m.Power, m.Name, m.Coef);
                    for (int j = i + 1; j < Monom.Count; j++)
                    {
                        Monomial other = Monom[j];

                        if (!taken[j] && m.Name == other.Name && m.Power == other.Power)
                        {
                            r.Coef += other.Coef;
                            taken[j] = true;
                        }
                    }

                    result.Add(r);
                }

                return new Polinom(result);
            }


            public void print()
            {
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("Polinom:");
                for (int i = 0; i < Monom.Count; i++)
                {
                    Console.Write(Monom[i].ToString());
                    Console.WriteLine();
                }
                Console.WriteLine("---------------------------------------");
            }


            public void writeToFile(string filename)
            {
                string toWrite = JsonConvert.SerializeObject(this);
                File.WriteAllText(filename, toWrite);
            }


            public static Polinom readFromFile(string filename)
            {
                StreamReader r = new StreamReader(filename);
                string jsonString = r.ReadToEnd();
                return JsonConvert.DeserializeObject<Polinom>(jsonString);
            }
        }



        class Monomial
        {
            private int power;
            private string name;
            private int coef;


            public Monomial(int power, string name, int coeficient)
            {
                Power = power;
                Name = name;
                Coef = coeficient;
            }


            public int Power { get => power; set => power = value; }
            public string Name { get => name; set => name = value; }
            public int Coef { get => coef; set => coef = value; }


            override public string ToString()
            {
                return $"Monomial, Name = {Name}, Power = {Power}, Coef = {Coef}";
            }
        }



        static void Main(string[] args)
        {

            Polinom p = new Polinom(new List<Monomial> { new Monomial(2, "a", 5), new Monomial(2, "a", 8) });


            p.writeToFile("file-1.json");

            Polinom p1 = Polinom.readFromFile("file-1.json");
            p1.print();

            Polinom p2 = new Polinom(new List<Monomial> { new Monomial(2, "a", 2), new Monomial(2, "a", 10) });

            Polinom p3 = new Polinom(new List<Monomial> { new Monomial(5, "a", 10), new Monomial(2, "b", 5), new Monomial(6, "a", 10) });

            Polinom p4 = new Polinom(new List<Monomial> { new Monomial(9, "a", 11), new Monomial(2, "b", 7), new Monomial(6, "a", 13) });




            Polinom p5 = p.sum(p2);
            Console.WriteLine("Сума: ");
            p5.print();

            Polinom p6 = p2.subtraction(p);
            Console.WriteLine("Вiднiмання: ");
            p6.print();


            Polinom p7 = p3.multiply(p4);
            Console.WriteLine("Множення: ");
            p7.print();


            Polinom p8 = p4.divide(p3);
            Console.WriteLine("Дiлення: ");
            p8.print();


        }
    }
}
