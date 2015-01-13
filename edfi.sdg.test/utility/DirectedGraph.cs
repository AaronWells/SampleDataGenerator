using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdFi.SampleDataGenerator.Test.Utility
{
    using EdFi.SampleDataGenerator.Utility;

    [TestClass]
    public class DirectedGraph
    {
        [TestMethod]
        public void ConnectedGraph()
        {
            var graph = new DirectedGraph<string>(new String[] { "A", "B", "C", "D" });
            graph.SetDependencies("A", new String[] { "B", "C" });
            graph.SetDependencies("B", new String[] { "C" });
            graph.SetDependencies("C", new String[] { "D" });
            var result = string.Join(",", graph.GetEvaluationOrder());
            Assert.AreEqual("D,C,B,A", result);
        }

        [TestMethod]
        public void DisconnectedGraph()
        {
            var graph = new DirectedGraph<string>(new String[] { "A", "B", "C", "D" });
            graph.SetDependencies("A", new String[] { "B" });
            graph.SetDependencies("C", new String[] { "D" });
            var result = string.Join(",", graph.GetEvaluationOrder());
            Assert.AreEqual("B,A,D,C", result);
        }

        [TestMethod]
        [ExpectedException(typeof(GraphCycleException))]
        public void BackEdgeDetection1()
        {
            var graph = new DirectedGraph<string>(new String[] { "A" });
            graph.SetDependencies("A", new String[] { "A" });
            var result = string.Join(",", graph.GetEvaluationOrder());
        }

        [TestMethod]
        [ExpectedException(typeof(GraphCycleException))]
        public void BackEdgeDetection2()
        {
            var graph = new DirectedGraph<string>(new String[] { "A", "B" });
            graph.SetDependencies("A", new String[] { "B" });
            graph.SetDependencies("B", new String[] { "A" });
            var result = string.Join(",", graph.GetEvaluationOrder());
        }
    
    }
}
