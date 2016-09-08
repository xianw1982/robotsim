/// <summary>
/// Simulator class, this will handle user input and manage robot movements on a table.
/// </summary>
namespace ToyRobot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RobotSim
    {
        private Table _table;
        private List<Robot> _robots;

        public RobotSim(Table table)
        {
            _table = table;
        }

        public void WaitForInput()
        {
            string command = Console.In.ReadLine();
            if (string.IsNullOrEmpty(command))
            {
               
            }
        }
    }
}
