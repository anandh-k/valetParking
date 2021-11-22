using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ValetParking
{
    class Program
    {
        // Main function will read the input file and call the core functionality accordingly.
        static void Main(string[] args)
        {
            // Basic input validation
            if ((args.Length > 0) && (!string.IsNullOrWhiteSpace(args[0])))
            {
                // Fetch filename
                string fileName = args[0];

                try
                {
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs);

                    sr.BaseStream.Seek(0, SeekOrigin.Begin);

                    // First line should have total lots for car & motorcycle
                    string str = sr.ReadLine();
                    var words = Regex.Split(str, @"\W+").ToList();

                    if (words.Count == 2)
                    {
                        uint totalCarLots = uint.Parse(words.FirstOrDefault());
                        uint totalMotorCycleLots = uint.Parse(words.LastOrDefault());

                        ValetParking valetParking = new ValetParking(totalCarLots, totalMotorCycleLots);

                        str = sr.ReadLine();

                        while (str != null)
                        {
                            words = Regex.Split(str, @"\W+").ToList();

                            if ((0 == string.Compare("Enter", words.FirstOrDefault(), true)) && (words.Count == 4))
                            {
                                valetParking.Entry(words[1], words[2], ulong.Parse(words[3]));
                            }
                            else if ((0 == string.Compare("Exit", words.FirstOrDefault(), true)) && (words.Count == 3))
                            {
                                valetParking.Exit(words[1], ulong.Parse(words[2]));
                            }
                            else
                            {
                                Console.WriteLine("Error processing line; skipping");
                            }

                            str = sr.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("First line should have total lots for both vehicles");
                        return;
                    }

                    sr.Close();
                    fs.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exiting with error. {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Error with input file. Pass a valid filename as input parameter");
            }
        }
    }
}
