using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InvestotoolUnitTest
{
    class ErrcheckTEST1
    {

        public bool ErrCheck(String Fname)
        {
            StreamReader Reader = new StreamReader(@Fname);

            int dimensions;
            bool check = int.TryParse(Reader.ReadLine(), out dimensions);

            if (!check)
            {

                Console.WriteLine("Error 1111111111111");
                return false;
            }
            String[] currencyarray = Reader.ReadLine().Split(' ');

            if (currencyarray.Length != dimensions)
            {

                Console.WriteLine("Error 2222222222222");
                return false;
            }





            Double[,] Inputarray = new Double[dimensions, dimensions];

            for (int i = 0; i < dimensions; i++)
            {
                if (Reader.Peek() < 0 && i < dimensions - 1)
                {

                    Console.WriteLine("Error 3333333333333333");
                    return false;
                }



                String phrase = Reader.ReadLine();
                String[] line = phrase.Split(' ');

                if (line.Length != dimensions)
                {

                    Console.WriteLine("Error 44444444444444444444" + "   " + i + "  " + line.Length + "  " + dimensions);

                    for (int j = 0; j < line.Length; j++)
                    {
                        Console.WriteLine(j + "   " + line[j]);
                    }

                    return false;
                }




                for (int j = 0; j < dimensions; j++)
                {



                    Double item;



                    bool mybool = Double.TryParse(line[j], out item);

                    if (!mybool)
                    {


                        Console.WriteLine("Error 555555555555555555");

                        return false;

                    }

                    Inputarray[i, j] = item;

                }





            }

            for (int i = 0; i < dimensions; i++)
            {
                if (Inputarray[i, i] != 1)
                {

                    Console.WriteLine("Error 6666666666666666");
                    return false;

                }
            }

            if (Reader.Peek() >= 0)
            {
                Console.WriteLine("Error 7777777777777");
                return false;
            }

            return true;


        }
    }
}
