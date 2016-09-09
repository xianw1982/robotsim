using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ToyRobot.Tests
{
    [TestClass]
    public class ToyRobotE2E
    {
        private class Scenario
        {
            public List<string> Commands = new List<string>();
            public string ExpectedResult = string.Empty;

            public Scenario(List<string> commandlist, string expected)
            {
                this.Commands = commandlist;
                this.ExpectedResult = expected;
            }

            public void RunAndValidate(ToyRobot robot)
            {
                foreach(string c in Commands)
                {
                    try
                    {
                        robot.InterpretCommand(c);
                    }
                    // robot exceptions are count in the console app, therefore catch them here
                    catch (RobotException) { } 
                }
                Assert.AreEqual(robot.Report(), this.ExpectedResult);
            }
        }

        [TestMethod]
        public void ToyRobot_scenario_a()
        {
            ToyTable tb = new ToyTable(5, 5);
            ToyRobot robot = new ToyRobot(tb);
            Scenario s = new Scenario(new List<string> {
                                          "PLACE 0,0,NORTH",
                                          "MOVE",
                                          "REPORT"},
                                          "0,1,NORTH");
            s.RunAndValidate(robot);
        }

        [TestMethod]
        public void ToyRobot_scenario_b()
        {
            ToyTable tb = new ToyTable(5, 5);
            ToyRobot robot = new ToyRobot(tb);
            Scenario s = new Scenario(new List<string> {
                                          "PLACE 0,0,NORTH",
                                          "LEFT",
                                          "REPORT"},
                                          "0,0,WEST");
            s.RunAndValidate(robot);
        }

        [TestMethod]
        public void ToyRobot_scenario_c()
        {
            ToyTable tb = new ToyTable(5, 5);
            ToyRobot robot = new ToyRobot(tb);
            Scenario s = new Scenario(new List<string> {
                                          "PLACE 1,2,EAST",
                                          "MOVE",
                                          "MOVE",
                                          "LEFT",
                                          "MOVE",
                                          "REPORT"},
                                          "3,3,NORTH");
            s.RunAndValidate(robot);
        }


        [TestMethod]
        public void ToyRobot_scenario_d()
        {
            ToyTable tb = new ToyTable(5, 5);
            ToyRobot robot = new ToyRobot(tb);
            Scenario s = new Scenario(new List<string> {
                                          "PLACE 1,2,EAST",
                                          "MOVE",
                                          "MOVE",
                                          "MOVE",
                                          "MOVE",
                                          "LEFT",
                                          "MOVE",
                                          "MOVE",
                                          "MOVE",
                                          "REPORT"},
                                          "4,4,NORTH");
            s.RunAndValidate(robot);
        }

    }
}
