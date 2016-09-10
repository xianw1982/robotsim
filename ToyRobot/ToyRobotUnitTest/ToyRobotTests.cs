using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToyRobot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ToyRobot.Tests
{
    [TestClass()]
    public class ToyRobotTests
    {
        /// <summary>
        /// this covers constructor / report and placement tests
        /// </summary>
        [TestMethod()]
        public void ToyRobot_internal_tests()
        {
            try
            {
                // when table is null
                ToyRobot failbot = new ToyRobot(null);
            }
            catch (ArgumentException) { }

            // create table and robot instance
            ToyTable tb = ToyTableTests.ToyTableCreate_successtest(522, 522);
            Assert.IsNotNull(tb);
            ToyRobot robot = new ToyRobot(tb);
            Assert.IsNotNull(robot);

            // running commands before robot is placed 
            AssertInvalidRobotCommandException(() => { robot.Report(); });
            AssertInvalidRobotCommandException(() => { robot.Move(); });
            AssertInvalidRobotCommandException(() => { robot.Turn(true); });
            AssertInvalidRobotCommandException(() => { robot.Turn(false); });
            // placing robot at invalid locations
            AssertOutofBoundException(() => { robot.Place(new CoordinateXY(0,-1)); });
            AssertOutofBoundException(() => { robot.Place(new CoordinateXY(0, 522)); });
            AssertOutofBoundException(() => { robot.Place(new CoordinateXY(521, 522)); });
            AssertOutofBoundException(() => { robot.Place(new CoordinateXY(524, 522)); });
            AssertOutofBoundException(() => { robot.Place(new CoordinateXY(-14, -1)); });

            // place robot tests
            PlaceRobotTest(robot, new CoordinateXY(0, 0), Direction.NORTH);
            PlaceRobotTest(robot, new CoordinateXY(0, 0), Direction.SOUTH);
            PlaceRobotTest(robot, new CoordinateXY(0, 0), Direction.EAST);
            robot.Report();
            PlaceRobotTest(robot, new CoordinateXY(0, 22), Direction.WEST);
            PlaceRobotTest(robot, new CoordinateXY(521, 521), Direction.WEST);
            robot.Report();
            PlaceRobotTest(robot, new CoordinateXY(520, 21), Direction.NORTH);
            PlaceRobotTest(robot, new CoordinateXY(0, 24), Direction.SOUTH);
            PlaceRobotTest(robot, new CoordinateXY(55, 0), Direction.EAST);
            Assert.AreEqual(robot.Report(), "55,0,EAST");
            Assert.AreEqual(robot.Report(), "55,0,EAST"); //report multiple times
            Assert.AreEqual(robot.Report(), "55,0,EAST"); //report multiple times

        }

        /// <summary>
        /// test the turn function
        /// </summary>
        [TestMethod()]
        public void ToyRobot_internal_TurnTest()
        {
            ToyTable tb = ToyTableTests.ToyTableCreate_successtest(100, 500);
            ToyRobot robot = new ToyRobot(tb);
            PlaceRobotTest(robot, new CoordinateXY(55, 45), Direction.NORTH);
            Assert.AreEqual(robot.Report(), "55,45,NORTH");
            TestLocation(robot, new CoordinateXY(55, 45), Direction.NORTH);
            robot.Turn(true);
            TestLocation(robot, new CoordinateXY(55, 45), Direction.WEST);
            robot.Turn(true);
            TestLocation(robot, new CoordinateXY(55, 45), Direction.SOUTH);
            robot.Turn(true);
            TestLocation(robot, new CoordinateXY(55, 45), Direction.EAST);
            robot.Turn(true);
            TestLocation(robot, new CoordinateXY(55, 45), Direction.NORTH);
            robot.Turn(false);
            TestLocation(robot, new CoordinateXY(55, 45), Direction.EAST);
            Assert.AreEqual(robot.Report(), "55,45,EAST");
            robot.Turn(false);
            TestLocation(robot, new CoordinateXY(55, 45), Direction.SOUTH);
            robot.Turn(false);
            TestLocation(robot, new CoordinateXY(55, 45), Direction.WEST);
            robot.Turn(false);
            TestLocation(robot, new CoordinateXY(55, 45), Direction.NORTH);
            Assert.AreEqual(robot.Report(), "55,45,NORTH");

        }

        /// <summary>
        /// test move function
        /// </summary>
        [TestMethod()]
        public void ToyRobot_internal_MoveTest()
        {
            ToyTable tb = ToyTableTests.ToyTableCreate_successtest(100, 500);
            ToyRobot robot = new ToyRobot(tb);
            robot.Place(new CoordinateXY(55, 45), Direction.NORTH);
            // test move in different directions
            robot.Move();
            TestLocation(robot, new CoordinateXY(55, 46), Direction.NORTH);
            robot.Turn(true);
            robot.Move();
            robot.Move();
            TestLocation(robot, new CoordinateXY(53, 46), Direction.WEST);
            robot.Turn(true);
            robot.Move();
            robot.Move();
            TestLocation(robot, new CoordinateXY(53, 44), Direction.SOUTH);
            robot.Turn(true);
            robot.Move();
            robot.Move();
            TestLocation(robot, new CoordinateXY(55, 44), Direction.EAST);
            Assert.AreEqual(robot.Report(), "55,44,EAST");

            // test out of bound movements
            robot.Place(new CoordinateXY(0, 0), Direction.NORTH);
            robot.Turn(true);
            AssertOutofBoundException(()=> { robot.Move(); });
            robot.Turn(true);
            TestLocation(robot, new CoordinateXY(0, 0), Direction.SOUTH);
            AssertOutofBoundException(() => { robot.Move(); });
            robot.Turn(true);
            robot.Move();
            robot.Move();
            TestLocation(robot, new CoordinateXY(2, 0), Direction.EAST);
            robot.Turn(true);
            TestLocation(robot, new CoordinateXY(2, 0), Direction.NORTH);
            
            // move 499 times north to the edge.
            for (int i =0; i < 499; i ++)
            {
                robot.Move();
                TestLocation(robot, new CoordinateXY(2, i + 1), Direction.NORTH);
            }
            // next move sound be out of bound.
            AssertOutofBoundException(() => { robot.Move(); });
            TestLocation(robot, new CoordinateXY(2, 499), Direction.NORTH);
        }

        /// <summary>
        /// validate interpreter conversion of parameters into commands.
        /// The goal here is only to make sure different command types are corrected interpreted.
        /// </summary>
        [TestMethod()]
        public void InterpretCommandTest_Throw()
        {
            ToyTable tb = ToyTableTests.ToyTableCreate_successtest(300, 500);
            ToyRobot robot = new ToyRobot(tb);
            // test in valid commands
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand(""); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("blahblah asd ad 123e 123 312 ,1,1,,1"); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("PLACE123"); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("PLACE 123"); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("PLACE 123,2,NROTH"); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("PLACE X, ,NORTH"); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("PLACE ,,"); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("PLACE 3xs2,xxa,SOUTH"); });
            //negative values
            AssertOutofBoundException(() => { robot.InterpretCommand("PLACE -123,-23,NORTH"); });
            //testing limits
            AssertOutofBoundException(() => { robot.InterpretCommand($"PLACE {int.MaxValue},{int.MinValue},NORTH"); });
            AssertOutofBoundException(() => { robot.InterpretCommand($"PLACE {int.MinValue},{int.MinValue},NORTH"); });
            AssertOutofBoundException(() => { robot.InterpretCommand($"PLACE {int.MinValue},{int.MaxValue},NORTH"); });
            AssertOutofBoundException(() => { robot.InterpretCommand($"PLACE {int.MaxValue},{int.MaxValue},NORTH"); });
            //make sure interpreter throws for overflow
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand($"PLACE {int.MaxValue}3213,{int.MinValue}3213,NORTH"); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("PLACE     !@#!@#()_@!#(<<<mmm,,,,,,,"); });
            //out of bound
            AssertOutofBoundException(() => { robot.InterpretCommand("PLACE 4123,23,NORTH"); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("   PLACE      "); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand(" MOVE bassdasd "); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("REPORT bassdasd "); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("LEFTbassdasd "); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand(" LEFTba  s  sdasd "); });
            AssertInvalidRobotCommandException(() => { robot.InterpretCommand("RIGHT  sdsad "); });

        }

        [TestMethod()]
        public void InterpretCommandTest_Success()
        {
            // init
            ToyTable tb = ToyTableTests.ToyTableCreate_successtest(300, 500);
            ToyRobot robot = new ToyRobot(tb);
            // place report check
            robot.InterpretCommand("PLACE 99,230,EAST");
            TestLocation(robot, new CoordinateXY(99, 230), Direction.EAST);
            robot.InterpretCommand("REPORT");
            TestLocation(robot, new CoordinateXY(99, 230), Direction.EAST);
            robot.InterpretCommand("PLACE 99,230,WEST");
            TestLocation(robot, new CoordinateXY(99, 230), Direction.WEST);
            robot.InterpretCommand("REPORT");
            TestLocation(robot, new CoordinateXY(99, 230), Direction.WEST);
            robot.InterpretCommand("PLACE 230,99,NORTH");
            TestLocation(robot, new CoordinateXY(230, 99), Direction.NORTH);
            robot.InterpretCommand("REPORT");
            TestLocation(robot, new CoordinateXY(230, 99), Direction.NORTH);
            robot.InterpretCommand("PLACE 230,99,SOUTH");
            TestLocation(robot, new CoordinateXY(230, 99), Direction.SOUTH);
            robot.InterpretCommand("REPORT");
            TestLocation(robot, new CoordinateXY(230, 99), Direction.SOUTH);

            //move tests 
            robot.InterpretCommand("MOVE");
            TestLocation(robot, new CoordinateXY(230, 98), Direction.SOUTH);
            robot.InterpretCommand("MOVE");
            TestLocation(robot, new CoordinateXY(230, 97), Direction.SOUTH);
            robot.InterpretCommand("LEFT");
            TestLocation(robot, new CoordinateXY(230, 97), Direction.EAST);
            robot.InterpretCommand("RIGHT");
            TestLocation(robot, new CoordinateXY(230, 97), Direction.SOUTH);
        }

        [TestMethod()]
        public void InterpretCommandTest_Output()
        {
            // init
            ToyTable tb = ToyTableTests.ToyTableCreate_successtest(300, 500);
            ToyRobot robot = new ToyRobot(tb);

            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms))
            {
                // place report check
                robot.InterpretCommand("PLACE 99,230,EAST", sw);
                robot.InterpretCommand("MOVE", sw);
                robot.InterpretCommand("MOVE", sw);
                robot.InterpretCommand("LEFT", sw);
                robot.InterpretCommand("LEFT", sw);
                robot.InterpretCommand("RIGHT", sw);
                TestLocation(robot, new CoordinateXY(101, 230), Direction.NORTH);
                robot.InterpretCommand("REPORT", sw);

                ms.Seek(0,SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                Assert.AreEqual("101,230,NORTH", sr.ReadToEnd().Trim());
            }

            // when report is not called nothing is written
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms))
            {
                // place report check
                robot.InterpretCommand("PLACE 99,230,EAST", sw);
                robot.InterpretCommand("MOVE", sw);
                robot.InterpretCommand("MOVE", sw);
                robot.InterpretCommand("LEFT", sw);
                robot.InterpretCommand("LEFT", sw);
                robot.InterpretCommand("RIGHT", sw);
                TestLocation(robot, new CoordinateXY(101, 230), Direction.NORTH);

                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                Assert.AreEqual("", sr.ReadToEnd());
            }
        }


        delegate void functiondelegate();

        private void AssertInvalidRobotCommandException( functiondelegate d)
        {
            try
            {
                d.Invoke();
                Assert.Fail("command did not throw");
            }
            catch (InvalidRobotCommandException) { }
        }
        private void AssertOutofBoundException(functiondelegate d)
        {
            try { 
                d.Invoke();
                Assert.Fail("command did not throw");
            }
            catch (RobotOutofBoundException) { }
        }

        private void PlaceRobotTest(ToyRobot robot, CoordinateXY target, Direction d)
        {
            robot.Place(target, d);
            TestLocation(robot, target, d);
        }

        private void TestLocation(ToyRobot robot, CoordinateXY expected, Direction expectedDirection)
        {
            Assert.AreEqual(robot.Location.X, expected.X);
            Assert.AreEqual(robot.Location.Y, expected.Y);
            Assert.AreEqual(robot.CurrentDirection, expectedDirection);
        }
    }
}