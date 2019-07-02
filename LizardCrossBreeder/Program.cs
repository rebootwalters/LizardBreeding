using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LizardCrossBreeder
{
    class Lizard
    {
        public int LizardID { get; set; }
        public string Name { get; set; }
        public List<Morph> Morphs { get; set; }
        public override string ToString()
        {
            return $"Lizard:{Name},Morphs:{Morphs.Count}";
        }

    }

    class Morph
    {
        public int MorphID { get; set; }
        public string MorphName { get; set; }
        public bool het { get; set; }
        public override string ToString()
        {
            return $"{MorphName}:{het}";
        }

    }

    class MorphPlus : IEquatable<MorphPlus>
    {
        public MorphPlus(Morph one, Morph two)
        {
            // one will never be null(normal), it can only be truebreed or het
            // two can be null(normal) or truebreed or het
            MorphName = one.MorphName;
            MorphOne = one;
            MorphTwo = two;
            // two is null = normal
            if (two == null)
            {
                // het to normal
                if (one.het)
                {
                    PercentNormal = 50;
                    PercentHet = 50;
                    PercentTrue = 0;
                }
                // truebreed to normal
                 else
                {
                    PercentNormal = 0;
                    PercentHet = 100;
                    PercentTrue = 0;
                }

            }
            else if (one.het == two.het)
            {
                // het to het
                if (one.het)
                {
                    PercentNormal = 25;
                    PercentHet = 50;
                    PercentTrue = 25;
                }
                // truebreed to truebreed
                else
                {
                    PercentNormal = 0;
                    PercentHet = 0;
                    PercentTrue = 100;
                }
            }
            else
             // het to truebreed (or truebreed to het)
            {
                PercentNormal = 0;
                PercentHet = 50;
                PercentTrue = 50;
            }

        }
        public Morph MorphOne;
        public Morph MorphTwo;
        public string MorphName { get; set; }
       // public bool Same { get; set; }
        public bool het { get; set; }

        public decimal PercentNormal { get; set; }
        public decimal PercentHet { get; set; }
        public decimal PercentTrue { get; set; }

        public override bool Equals(object obj)
        {

            MorphPlus other = obj as MorphPlus;
            if (other == null) return false;
            return other.MorphName.Equals(MorphName);
        }

        public bool Equals(MorphPlus obj)
        {
           
            if (obj == null) return false;
            return obj.MorphName.Equals(MorphName);
        }
        public override int GetHashCode()
        {
            return MorphName.GetHashCode();
        }

        public override string ToString()
        {
            char p = '\'';
            char s = ' ';
            return $"{MorphOne.MorphName}{(MorphOne.het?p:s)} & {MorphTwo?.MorphName??null}{(((MorphTwo?.het??false))?p:s)}{MorphName,15} N:{PercentNormal,4} H:{PercentHet,4} T:{PercentTrue}";
        }

    }

 


    class Program
    {

        static List<MorphPlus> Compute(Lizard One, Lizard two)
        {
            var leftouter =
                (from m1 in One.Morphs
                 join m2 in two.Morphs on m1.MorphName equals
                  m2.MorphName
              //select new MorphPlus(m1, m2);
              into loj from mempty in loj.DefaultIfEmpty() select new MorphPlus(m1, loj.FirstOrDefault())).ToList();
            var rightouter =
                (from m1 in two.Morphs
                 join m2 in One.Morphs on m1.MorphName equals m2.MorphName

                into loj
                 from mempty in loj.DefaultIfEmpty()
                 select new MorphPlus(m1,loj.FirstOrDefault())).ToList();

            return (leftouter.Union(rightouter)).ToList();

        }

      
        static void Main(string[] args)
        {
            Morph M1 = new Morph() { MorphID = 1, het = false, MorphName = "M1" };
            Morph M2 = new Morph() { MorphID = 2, het = false, MorphName = "M2" };
            Morph M3prime = new Morph() { MorphID = 3, het = true, MorphName = "M3" };
            Morph M4prime = new Morph() { MorphID = 4, het = true, MorphName = "M4" };
            Morph Mfour = new Morph() { MorphID = 7, het = false, MorphName = "M4" };
            Morph Mfive = new Morph() { MorphID = 8, het = false, MorphName = "M5" };
            Morph Mfiveprime = new Morph() { MorphID = 9, het = true, MorphName = "M5" };

            Lizard L1 = new Lizard() { LizardID = 1, Name = "Liz 1" };
            Lizard L2 = new Lizard() { LizardID = 2, Name = "Liz 2" };

            L1.Morphs = new List<Morph>() { M1, M2, M3prime, M4prime };
            L2.Morphs = new List<Morph>() { M1, M3prime, Mfour, Mfiveprime };

            var total = Compute(L1, L2);
            foreach(var x in total)
            {
                Console.WriteLine(x);
            }



        }
    }
}
