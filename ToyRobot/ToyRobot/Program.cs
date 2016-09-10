
namespace ToyRobot
{
    using System;
    using System.Configuration;

    class Program
    {
        /// <summary>
        /// supported configuration enum
        /// </summary>
        private enum Configurations
        {
            TableLimitX,
            TableLimitY,
            DebugLog
        }

        /// <summary>
        ///  main function
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            Console.CancelKeyPress += new ConsoleCancelEventHandler((object sender, System.ConsoleCancelEventArgs e) 
                => {
                    Console.WriteLine("exiting...");
                    System.Environment.Exit(0);
                });

            try
            {
                string debugfile = GetConfigStringValue(Configurations.DebugLog);
                Logger.Init(debugfile);
               
                int tableRangeX = GetConfigIntValue(Configurations.TableLimitX, 5);
                int tableRangeY = GetConfigIntValue(Configurations.TableLimitY, 5);

                WelcomeMessage(tableRangeX, tableRangeY);
                ToyTable tb = new ToyTable(tableRangeY, tableRangeX);
                ToyRobot robot = new ToyRobot(tb);
                StartUserInput(robot);
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                System.Environment.Exit(1);
            }
        }

        private static void WelcomeMessage(int x, int y)
        {
            Console.WriteLine("Welcome to ToyRobot");
            Console.WriteLine($"Configuring {x}x{y} sized table");
            Console.WriteLine($"Please Enter one of the following commands:");
            Console.WriteLine($"PLACE X,Y,F | MOVE | LEFT | RIGHT | REPORT");
        }

        /// <summary>
        /// start reading standard input for robot commands
        /// </summary>
        /// <param name="robot">robot location</param>
        private static void StartUserInput(ToyRobot robot)
        {
            while (true)
            {
                string command = Console.ReadLine();
                try
                {
                    robot.InterpretCommand(command);
                    Logger.Log($"{command} succeeded. Robot current location: {robot.Location},{robot.CurrentDirection}");
                }
                // catch robot exceptions and log to debug log. else throw the exception.
                catch (RobotException ex)
                {
                    Logger.Log(ex);
                }
            }
        }

        /// <summary>
        /// Look up config name in application config, 
        /// return defaultvalue if not set or empty, 
        /// throw if the value can not be converted to int
        /// </summary>
        /// <param name="configName">name of configuration</param>
        /// <param name="defaultValue">parameter default</param>
        /// <returns>value of configuration in int</returns>
        private static int GetConfigIntValue(Configurations configName, int defaultValue = 0)
        {
            try
            {
                string value = ConfigurationManager.AppSettings[configName.ToString()];

                if (string.IsNullOrEmpty(value))
                {
                    return defaultValue;
                }

                return int.Parse(value);
            }catch(Exception ex)
            {
                throw new ArgumentException(
                        $"Exception while trying to parse config {configName} - {ex.Message}");
            }
        }

        /// <summary>
        /// Look up config name in application config, 
        /// return defaultvalue if not set or empty, 
        /// throw if the value can not be converted to int
        /// </summary>
        /// <param name="configName">name of configuration</param>
        /// <param name="defaultValue">parameter default</param>
        /// <returns>value of configuration in int</returns>
        private static string GetConfigStringValue(Configurations configName, string defaultValue = "")
        {
            string value = ConfigurationManager.AppSettings[configName.ToString()];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return value;
        }
    }
}
