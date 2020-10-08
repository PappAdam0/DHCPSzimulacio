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
        static void Feladat(string parancs)
        {
            /*eloszor csak request
             * megnezzük h request-e
            */

            if (parancs.Contains("request"))
            {
                string[] a = parancs.Split(';');
                string mac = a[1];

                if (dhcp.ContainsKey(mac))
                {
                    Console.WriteLine($"DHCP: {mac} --- {dhcp[mac]}");
                }
                else
                {
                    if (reserved.ContainsKey(mac))
                    {
                        Console.WriteLine($"Reserved: {mac} --- {reserved[mac]}");
                        dhcp.Add(mac, reserved[mac]);
                    }
                    else
                    {
                        string indulo = "192.168.10.100";
                        int okt4 = 100;

                        while (okt4 < 200 && ( dhcp.ContainsValue(indulo) || reserved.ContainsValue(indulo) || excluded.Contains(indulo)))
                        {
                            okt4++;
                            indulo = CimEggyelNo(indulo);
                        }
                        if (okt4 < 200)
                        {
                            Console.WriteLine($"Kiosztott: {mac} --- {indulo}");
                            dhcp.Add(mac, indulo);
                        }
                        else
                        {
                            Console.WriteLine($"{mac} nincs IP");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("nem ok");
            }
        }

        static void Feladatok()
        {
            foreach (var command in commands)
            {
                Feladat(command);
            }
        }
        static void Main(string[] args)
        {
            BeolvasList(excluded,"excluded.csv");
            BeolvasList(commands,"test.csv");

            beolvasDictionary(dhcp, "dhcp.csv");
            beolvasDictionary(reserved, "reserved.csv");


            Feladatok();
 

            Console.WriteLine("\nVége...");
            Console.ReadKey();
        }
        
    }
}
