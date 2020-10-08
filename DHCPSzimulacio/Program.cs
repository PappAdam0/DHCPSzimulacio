using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHCPSzimulacio
{
    class Program
    {
        static List<string> excluded = new List<string>();

        static string CimEggyelNo(string cim)
        {

            string[] adat = cim.Split('.');
            int okt4 = int.Parse(adat[3]);

            if (okt4 < 255)
            {
                okt4++;
            }

            string vissza = adat[0] + "." + adat[1] + "." + adat[2] + "." + okt4.ToString();
            return vissza;
        }
        //192.168.10.100 --> 192.168.10.101
        /*szetvagni a pontok mentek
         * az utolsot inte konvertalni
         * egyet hozaadni 255-ot ne lepje tul
         * osszefuzni string-e
         */

        static void Main(string[] args)
        {
            beolvasexcluded();
            Console.WriteLine(CimEggyelNo("192.168.10.100"));

            //foreach (var e in excluded)
            //{
            //    Console.WriteLine(e);
            //}

            Console.WriteLine("\nVége...");
            Console.ReadKey();
        }
        static void beolvasexcluded()
        {
            try
            {
                StreamReader file = new StreamReader("excluded.csv");
                try
                {
                    while (!file.EndOfStream)
                    {
                        excluded.Add(file.ReadLine());
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
                finally
                {
                    file.Close();
                }
                file.Close();
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
