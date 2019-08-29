using NUnit.Framework;
using System;
using API.Models;
using System.Collections.Generic;

namespace API.UnitTest.Models{

  [TestFixture]
  public class Graph_CalculateCostGivenPathShould
  {
    [Test]
    public void CalculateCostGivenPath_NoGraphNoPath_ReturnsZero(){
      Graph graph = new Graph();
      List<string> path = new List<string>();

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(0));
    }

    [Test]
    public void CalculateCostGivenPath_NoGraph1HopPath_ReturnsZero(){
      Graph graph = new Graph();
      List<string> path = new List<string>() { "A" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(0));
    }

    [Test]
    public void CalculateCostGivenPath_SimpleGraphNoPath_ReturnsZero(){
      Graph graph = CreateSimpleGraph();
      List<string> path = new List<string>();

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(0));
    }

    [Test]
    public void CalculateCostGivenPath_Simple_5_10_15_GraphTravelAll4Nodes_Returns30(){
      Graph graph = CreateSimpleGraph();
      List<string> path = new List<string>() { "A", "B", "C", "D" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(30));
    }

    [Test]
    public void CalculateCostGivenPath_Simple_5_10_15_GraphTravelFirst3Nodes_Returns15(){
      Graph graph = CreateSimpleGraph();
      List<string> path = new List<string>() { "A", "B", "C" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(15));
    }

    [Test]
    public void CalculateCostGivenPath_Simple_5_10_15_GraphTravelLast3Nodes_Returns25(){
      Graph graph = CreateSimpleGraph();
      List<string> path = new List<string>() { "B", "C", "D" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(25));
    }

    [Test]
    public void CalculateCostGivenPath_Simple_5_10_15_GraphTravel1Node_ReturnsZero(){
      Graph graph = CreateSimpleGraph();
      List<string> path = new List<string>() { "A" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(0));
    }    

    [Test]
    public void CalculateCostGivenPath_Simple_5_10_15_GraphTravelInvalidPath_ThrowsArgumentException(){
      Graph graph = CreateSimpleGraph();
      List<string> path = new List<string>() { "A","C" };

      var ex = Assert.Throws<ArgumentException>(() => graph.CalculateCostGivenPath(path));
      Assert.That(ex.ParamName, Is.EqualTo("path"));
    }

    [Test]
    public void CalculateCostGivenPath_Simple_5_10_15_GraphTravelInvalidNodeName_ThrowsArgumentException(){
      Graph graph = CreateSimpleGraph();
      List<string> path = new List<string>() { "X","Y" };

      var ex = Assert.Throws<ArgumentException>(() => graph.CalculateCostGivenPath(path));
      Assert.That(ex.ParamName, Is.EqualTo("path"));
    }

    [Test]
    public void CalculateCostGivenPath_CircularGraph_5_10_15_20_Travel_ABCDA_Returns50(){
      Graph graph = CreateCircularGraph();
      List<string> path = new List<string>() { "A", "B", "C", "D", "A" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(50));
    }

    [Test]
    public void CalculateCostGivenPath_CircularGraph_5_10_15_20_Travel_ABCDABCDA_Returns100(){
      Graph graph = CreateCircularGraph();
      List<string> path = new List<string>() { "A", "B", "C", "D", "A", "B", "C", "D", "A" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(100));
    }

    [Test]
    public void CalculateCostGivenPath_TwoWayGraph_5_10_15_20_Travel_ABABA_Returns20(){
      Graph graph = CreateTwoWayGraph();
      List<string> path = new List<string>() { "A", "B", "A", "B", "A" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(20));
    }

    [Test]
    public void CalculateCostGivenPath_TwoWayGraph_5_10_15_20_Travel_ABCBA_Returns30(){
      Graph graph = CreateTwoWayGraph();
      List<string> path = new List<string>() { "A", "B", "C", "B", "A" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(30));
    }

    [Test]
    public void CalculateCostGivenPath_ComplexGraph_Travel_ACGF_Returns590(){
      Graph graph = CreateComplexGraph();
      List<string> path = new List<string>() { "A", "C", "G", "F" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(590));
    }

    [Test]
    public void CalculateCostGivenPath_ComplexGraph_Travel_CBEC_Returns385(){
      Graph graph = CreateComplexGraph();
      List<string> path = new List<string>() { "C", "B", "E", "C" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(385));
    }

    [Test]
    public void CalculateCostGivenPath_ComplexGraph_Travel_CGFGH_Returns625(){
      Graph graph = CreateComplexGraph();
      List<string> path = new List<string>() { "C", "G", "F", "G", "H" };

      int cost = graph.CalculateCostGivenPath(path);

      Assert.That(cost, Is.EqualTo(625));
    }

    [Test]
    public void CalculateCostGivenPath_ComplexGraph_Travel_ACFG_ThrowArgumentException(){
      Graph graph = CreateComplexGraph();
      List<string> path = new List<string>() { "A", "C", "F", "G" };

      Assert.Throws<ArgumentException>(() => graph.CalculateCostGivenPath(path));
    }

    public Graph CreateSimpleGraph(){
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("B", "C", 10);
      Route r3 = new Route("C", "D", 15);
      Graph graph = new Graph();
      
      graph.AddRoute(r1);
      graph.AddRoute(r2);
      graph.AddRoute(r3);

      return graph;
    }

    public Graph CreateCircularGraph(){
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("B", "C", 10);
      Route r3 = new Route("C", "D", 15);
      Route r4 = new Route("D", "A", 20);
      Graph graph = new Graph();
      
      graph.AddRoute(r1);
      graph.AddRoute(r2);
      graph.AddRoute(r3);
      graph.AddRoute(r4);

      return graph;
    }

    public Graph CreateTwoWayGraph(){
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("B", "A", 5);
      Route r3 = new Route("B", "C", 10);
      Route r4 = new Route("C", "B", 10);
      Route r5 = new Route("C", "D", 15);
      Route r6 = new Route("D", "C", 15);
      Graph graph = new Graph();
      
      graph.AddRoute(r1);
      graph.AddRoute(r2);
      graph.AddRoute(r3);
      graph.AddRoute(r4);
      graph.AddRoute(r5);
      graph.AddRoute(r6);

      return graph;
    }

    public Graph CreateComplexGraph(){
      List<Route> routes = new List<Route>(){
        new Route("A", "B", 60),
        new Route("A", "C", 150),
        new Route("B", "C", 50),
        new Route("B", "E", 80),
        new Route("C", "B", 220),
        new Route("C", "G", 350),
        new Route("D", "I", 120),
        new Route("E", "A", 70),
        new Route("E", "C", 85),
        new Route("F", "A", 230),
        new Route("F", "G", 110),
        new Route("G", "F", 90),
        new Route("G", "H", 75),
        new Route("H", "I", 35),
        new Route("I", "C", 90),
        new Route("I", "D", 30)
      };

      Graph graph = new Graph();
      
      graph.AddRoutes(routes);

      return graph;
    }
  }
}