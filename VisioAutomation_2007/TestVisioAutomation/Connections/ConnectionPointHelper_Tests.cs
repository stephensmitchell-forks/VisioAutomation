using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ConnectionPointHelper = VisioAutomation.Connections.ConnectionPointHelper;
using VA = VisioAutomation;

namespace TestVisioAutomation
{
    [TestClass]
    public class ConnectionPointHelper_Tests : VisioAutomationTest
    {
        [TestMethod]
        public void AddRemoveAndCountConnectionPoints()
        {
            var page1 = GetNewPage();

            var s1 = page1.DrawRectangle(0, 0, 4, 1);
            Assert.AreEqual(0, ConnectionPointHelper.GetConnectionPointCount(s1));

            var cp_type = VA.Connections.ConnectionPointType.Inward;

            var cpd1 = new VA.Connections.ConnectionPointCells();
            cpd1.X = "Width*0.25";
            cpd1.Y = "Height*0";
            cpd1.Type = (int) cp_type;

            var cpd2 = new VA.Connections.ConnectionPointCells();
            cpd2.X = "Width*0.75";
            cpd2.Y = "Height*0";
            cpd2.Type = (int) cp_type;

            ConnectionPointHelper.AddConnectionPoint(s1, cpd1);
            Assert.AreEqual(1, ConnectionPointHelper.GetConnectionPointCount(s1));

            ConnectionPointHelper.AddConnectionPoint(s1, cpd2);
            Assert.AreEqual(2, ConnectionPointHelper.GetConnectionPointCount(s1));

            var controlpoints = VA.Connections.ConnectionPointHelper.GetConnectionPoints(s1);
            Assert.AreEqual(2, controlpoints.Count);
            var cp_0 = controlpoints[0];
            AssertVA.AreEqual("0 in", 0, cp_0.DirX);
            AssertVA.AreEqual("0 in", 0, cp_0.DirY);
            AssertVA.AreEqual("0", 0, cp_0.Type);
            AssertVA.AreEqual("Width*0.25", 1, cp_0.X);
            AssertVA.AreEqual("Height*0", 0, cp_0.Y);

            var cp_1 = controlpoints[1];
            AssertVA.AreEqual("0 in", 0, cp_1.DirX);
            AssertVA.AreEqual("0 in", 0, cp_1.DirY);
            AssertVA.AreEqual("0", 0, cp_1.Type);
            AssertVA.AreEqual("Width*0.75", 3, cp_1.X);
            AssertVA.AreEqual("Height*0", 0, cp_1.Y);

            ConnectionPointHelper.DeleteConnectionPoint(s1, 1);
            Assert.AreEqual(1, ConnectionPointHelper.GetConnectionPointCount(s1));
            ConnectionPointHelper.DeleteConnectionPoint(s1, 0);
            Assert.AreEqual(0, ConnectionPointHelper.GetConnectionPointCount(s1));

            page1.Delete(0);
        }

        [TestMethod]
        public void DeleteAllConnectionPoints()
        {
            var page1 = GetNewPage();

            var s1 = page1.DrawRectangle(0, 0, 4, 1);
            Assert.AreEqual(0, ConnectionPointHelper.GetConnectionPointCount(s1));

            var cp_type = VA.Connections.ConnectionPointType.Inward;

            var xpositions = new[] {"Width*0.25", "Width*0.30", "Width*0.75", "Width*0.90"};
            var ypos = "Height*0";

            foreach (var xpos in xpositions)
            {
                var cp = new VA.Connections.ConnectionPointCells();
                cp.X = xpos;
                cp.Y = ypos;
                cp.DirX = 0;
                cp.DirY = 0;
                cp.Type = (int) cp_type;

                ConnectionPointHelper.AddConnectionPoint(s1, cp);
            }

            Assert.AreEqual(4, ConnectionPointHelper.GetConnectionPointCount(s1));

            int num_deleted = ConnectionPointHelper.DeleteAllConnectionPoints(s1);
            Assert.AreEqual(4, num_deleted);
            Assert.AreEqual(0, ConnectionPointHelper.GetConnectionPointCount(s1));

            page1.Delete(0);
        }
    }
}