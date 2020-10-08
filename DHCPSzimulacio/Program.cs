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
        static Dictionary<string, string> dhcp = new Dictionary<string, string>();
        static Dictionary<string, string> reserved = new Dictionary<string, string>();
        static List<string> commands = new List<string>();

        static void BeolvasList(List<string> l, string filename)
        {
            try
            {
                StreamReader file = new StreamReader(filename);
                try
                {
                    while (!file.EndOfStream)
                    {
                        l.Add(file.ReadLine());
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
        static void beolvasDictionary(Dictionary<string,string> d, string filenev)
        {
            try
            {
                StreamReader file = new StreamReader(filenev);
                while (!file.EndOfStream)
                {
                    string[] adat = file.ReadLine().Split(';');
                    d.Add(adat[0], adat[1]);
                }
                file.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
        static void Main(string[] args)
        {
            BeolvasList(excluded,"excluded.csv");
            BeolvasList(commands,"test.csv");

            beolvasDictionary(dhcp, "dhcp.csv");
            beolvasDictionary(reserved, "reserved.csv");

            foreach (var e in commands)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("\nVége...");
            Console.ReadKey();
        }
        
    }
}
