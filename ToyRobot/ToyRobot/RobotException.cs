namespace ToyRobot
{
    using System;

    /// <summary>
    /// Base class for all robot related exceptions.
    /// </summary>
    public abstract class RobotException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RobotException():base()
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        public RobotException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// This is thrown when trying to move a robot outside of table boundary
    /// </summary>
    public class RobotOutofBoundException : RobotException
    {
        private CoordinateXY _tableBoundary;
        private CoordinateXY _robotLocation;
        public RobotOutofBoundException(CoordinateXY tableBoundary, CoordinateXY robotLocation)
        {
            this._tableBoundary = tableBoundary;
            this._robotLocation = robotLocation;
        }

        public override string Message
        {
            get
            {
                return $"Robot target location {this._robotLocation} is outside of table boundary {this._tableBoundary}.";
            }
        }
    }

    /// <summary>
    /// this is thrown when invalid robot commands are issued
    /// </summary>
    public class InvalidRobotCommandException : RobotException
    {
        private string _command = string.Empty;
        
        /// <summary>
        /// the command causing the exception
        /// </summary>
        public string InputCommand
        {
            get { return this._command; }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="command">the command causing the exception</param>
        /// <param name="message">exception message</param>
        public InvalidRobotCommandException(string command, string message ) : base(message)
        {
            this._command = command;
        }

        /// <summary>
        /// exception message. (including the command)
        /// </summary>
        public override string Message
        {
            get
            {
                return $"Invalid Robot Command \"{this.InputCommand}\" : {base.Message}";
            }
        }
    }
}
