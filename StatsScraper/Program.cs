using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatsScraper
{
    class Program
    {
        static void Main(string[] args)
        {
           while(true)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(getStats()));
                Thread.Sleep(500);
            }
        }

        public static Dictionary<string, Dictionary<string, string>> getStats()
        {
            var stats = new Dictionary<string, Dictionary<string, string>>();
            OpenHardwareMonitor.Hardware.Computer computer = new OpenHardwareMonitor.Hardware.Computer
            {
                // enabling CPU to monitor
                CPUEnabled = true,
                GPUEnabled = true,
                RAMEnabled = true,
                MainboardEnabled = true,
                HDDEnabled = true
            };
            try
            {
                computer.Close();
                try
                {
                    computer.Open();
                }
                catch (Exception)
                {
                    //computer fails to open 1/2 times. no idea why
                    computer.Open();
                }
                foreach (OpenHardwareMonitor.Hardware.IHardware hw in computer.Hardware)
                {
                    hw.Update();
                    var hwStats = new Dictionary<string, string>();
                    // searching for all sensors and adding data to listbox
                    foreach (OpenHardwareMonitor.Hardware.ISensor s in hw.Sensors)
                    {
                        hwStats.Add(s.Name + " (" + s.Identifier.ToString() + ")", s.Value.ToString());

                    }
                    stats.Add(hw.Name + " (" + hw.Identifier.ToString() + ")", hwStats);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                computer.Close();
            }
            return stats;


        }
    }
}
