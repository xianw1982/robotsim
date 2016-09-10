/// <summary>
/// ToyRobot class
/// Once initialized the robot accepts the following commands: 
///     PLACE X, Y, F
///     MOVE
///     LEFT
///     RIGHT
///     REPORT
/// 
/// - PLACE will put the toy robot on the table in position X, Y and facing NORTH,
///   SOUTH, EAST or WEST.
/// - The first valid command to the robot is a PLACE command, after that, any
///  sequence of commands may be issued, in any order, including another PLACE
///  command. The application should discard all commands in the sequence until
///  a valid PLACE command has been executed.
/// - MOVE will move the toy robot one unit forward in the direction it is
///  currently facing.
/// - LEFT and RIGHT will rotate the robot 90 degrees in the specified direction
///  without changing the position of the robot.
/// - REPORT will announce the X, Y and F of the robot to standard output
/// </summary>
namespace ToyRobot
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// enum of valid robot commands
    /// </summary>
    public enum Command
    {
        PLACE,
        REPORT,
        LEFT,
        RIGHT,
        MOVE,
        UNKNOWN
    }
    
    /// <summary>
    /// ToyRobot class
    /// </summary>
    public class ToyRobot
    {
        private ToyTable _table = null;
        private Direction _currentDirection = Direction.NORTH;
        private CoordinateXY _location = null;

        /// <summary>
        /// Current direction robot is facing
        /// </summary>
        public Direction CurrentDirection
        {
            get
            {
                return _currentDirection;
            }
        }

        /// <summary>
        /// Current location of robot
        /// </summary>
        public CoordinateXY Location
        {
            get
            {
                return _location;
            }
        }

        public ToyRobot(ToyTable table)
        {
            if (table == null)
            {
                throw new ArgumentException("table can not be null");
            }

            this._table = table;
        }

        /// <summary>
        /// Interpret user commands 
        /// few assumptions are made about the command which were not clearly specified in the requirements.
        /// 1. commands are not case sensitive
        /// 2. extra spaces in the command is skipped
        /// 3. commands that take no additional argument such as LEFT, MOVE, RIGHT and REPORT
        ///    will ignore any additional arguments provided.
        /// </summary>
        /// <param name="command">string based command</param>
        public void InterpretCommand(string command, StreamWriter outStream = null)
        {
            KeyValuePair<Command, string> argument = ParseCommand(command);

            switch (argument.Key)
            {
                case (Command.PLACE):
                    this.Place(argument.Value);
                    break;
                case (Command.MOVE):
                    this.Move();
                    break;
                case (Command.LEFT):
                    this.Turn(true);
                    break;
                case (Command.RIGHT):
                    this.Turn(false);
                    break;
                case (Command.REPORT):
                    string report = this.Report();
                    TextWriter targetOutputStream = (outStream ?? Console.Out);

                    lock (targetOutputStream)
                    {
                        targetOutputStream.WriteLine(report);
                        targetOutputStream.Flush();
                    }
                    break;
                default:
                    throw new InvalidRobotCommandException(
                        command,
                        string.Format("unsupported command {0}", argument.Key));
            }
        }

        /// <summary>
        /// Parse the command 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static KeyValuePair<Command, string> ParseCommand(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
            {
                throw new InvalidRobotCommandException(cmd, "command is null or empty");
            }

            string tc = cmd.Trim();
            int firstDelimiterIndex = tc.IndexOf(" ");
            
            string commandstr = firstDelimiterIndex >= 0 ? tc.Substring(0, firstDelimiterIndex).Trim(): tc;

            string args = firstDelimiterIndex >= 0 ? tc.Substring(firstDelimiterIndex).Trim(): string.Empty;
           
            Command finalCommand = Command.UNKNOWN;
            if (commandstr.Equals(Command.LEFT.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                finalCommand = Command.LEFT;
            }
            else if (commandstr.Equals(Command.RIGHT.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                finalCommand = Command.RIGHT;
            }
            else if (commandstr.Equals(Command.MOVE.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                finalCommand = Command.MOVE;
            }
            else if (commandstr.Equals(Command.REPORT.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                finalCommand = Command.REPORT;
            }
            else if (commandstr.Equals(Command.PLACE.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                finalCommand = Command.PLACE;
            }
            else
            {
                throw new InvalidRobotCommandException(
                    cmd, $"unsupported command {commandstr}");
            }

            return new KeyValuePair<Command, string>(finalCommand, args);
        }

        /// <summary>
        /// convert string to direction enum
        /// </summary>
        /// <param name="directionStr"></param>
        /// <returns></returns>
        private static Direction ConvertStringToDirection(string directionStr)
        {
            string td = directionStr.Trim();
            if (td.Equals(Direction.EAST.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
               return Direction.EAST;
            }
            else if (td.Equals(Direction.WEST.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return Direction.WEST;
            }
            else if (td.Equals(Direction.NORTH.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return Direction.NORTH;
            }
            else if (td.Equals(Direction.SOUTH.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return Direction.SOUTH;
            }
            else
            {
                throw new NotSupportedException($"Unsupported direction: {td}");
            }
        }

        /// <summary>
        /// Parse input argument:
        /// </summary>
        /// <param name="arguments">comma seprated inputs in groups of 3: x,y,F
        /// x = x axis coordinate
        /// y = y axis coordinate
        /// F = direction robot is facing. i.e. NORTH, EAST, SOUTH, WEST
        /// </param>
        private void Place(string arguments)
        {
            if (string.IsNullOrWhiteSpace(arguments))
            {
                throw new InvalidRobotCommandException("PLACE", "argument is empty");
            }

            string[] args = arguments.Split(new char[] { ',' });
            if (args.Length != 3)
            {
                throw new InvalidRobotCommandException("PLACE " + arguments, "invalid number of arguments");
            }

            int x = -1;
            int y = -1;
            Direction d = Direction.NORTH;

            try
            {
                x = int.Parse(args[0]);
                y = int.Parse(args[1]);
                d = ConvertStringToDirection(args[2]);
            }
            catch (Exception ex)
            {
                throw new InvalidRobotCommandException("PLACE " + arguments, ex.Message);
            }

            this.Place(new CoordinateXY(x, y), d);
        }

        /// <summary>
        /// Place function
        /// </summary>
        /// <param name="targetLocation">coordinate of the new location robot to be placed</param>
        /// <param name="f">direction we are facing</param>
        internal void Place(CoordinateXY targetLocation, Direction f = Direction.NORTH)
        {
            if (!_table.IsValidPlacement(targetLocation))
            {
                throw new RobotOutofBoundException(_table.TableBoundary, targetLocation);
            }

            this._location = targetLocation;
            this._currentDirection = f;
        }

        /// <summary>
        /// report the current robot location to console out.
        /// </summary>
        internal string Report()
        {
            if (this._location != null)
            {
                return $"{this._location},{this._currentDirection}";
                        
            }else
            {
                throw new InvalidRobotCommandException("REPORT", "Robot has not been placed.");
            }
        }

        /// <summary>
        /// Move function will move the toy robot one unit forward in the direction it is currently facing.
        /// </summary>
        /// <returns>true if move was successful, false otherwise</returns>
        internal void Move()
        {
            if (this._location != null)
            {
                CoordinateXY targetLocation = new CoordinateXY(this._location.X, this._location.Y);

                targetLocation.Move(this._currentDirection, 1);

                if (_table.IsValidPlacement(targetLocation))
                {
                    this._location = targetLocation;
                }
                else
                {
                    throw new RobotOutofBoundException(this._table.TableBoundary, targetLocation);
                }
            }
            else
            {
                throw new InvalidRobotCommandException("MOVE", "Robot has not been placed.");
            }
        }

        /// <summary>
        /// turn robot in left or right direction
        /// </summary>
        /// <param name="isLeftTurn">true if makign a left turn false if making a right turn</param>
        internal void Turn(bool isLeftTurn)
        {
            if (this._location != null)
            {
                switch (_currentDirection)
                {
                    case (Direction.NORTH):
                        _currentDirection = isLeftTurn ? Direction.WEST : Direction.EAST;
                        break;
                    case (Direction.WEST):
                        _currentDirection = isLeftTurn ? Direction.SOUTH : Direction.NORTH;
                        break;
                    case (Direction.SOUTH):
                        _currentDirection = isLeftTurn ? Direction.EAST : Direction.WEST;
                        break;
                    case (Direction.EAST):
                        _currentDirection = isLeftTurn ? Direction.NORTH : Direction.SOUTH;
                        break;
                    default:
                        throw new InvalidRobotCommandException(
                            "TURN",
                            $"Direction {_currentDirection.ToString()} is not supported in the current turn function.");
                }
            }
            else
            {
                throw new InvalidRobotCommandException("LEFT/RIGHT", "Robot has not been placed.");
            }
        }
    }
}