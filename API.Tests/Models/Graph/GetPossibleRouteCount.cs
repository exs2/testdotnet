using NUnit.Framework;
using System;
using API.Models;
using System.Collections.Generic;

namespace API.UnitTest.Models
{
  [TestFixture]
  public class Graph_GetPossibleRouteCountShould
  {
    [Test]
    public void GetPossibleRouteCount_NoGraph_NoFrom_ThrowsArguementNullException()
    {
      Graph graph = new Graph();

      var ex = Assert.Throws<ArgumentNullException>(() => graph.GetPossibleRouteCount(null, "A"));
      Assert.That(ex.ParamName, Is.EqualTo("fromNodeName"));
    }

    [Test]
    public void GetPossibleRouteCount_NoGraph_NoTo_ThrowsArguementNullException()
    {
      Graph graph = new Graph();

      var ex = Assert.Throws<ArgumentNullException>(() => graph.GetPossibleRouteCount("A", null));
      Assert.That(ex.ParamName, Is.EqualTo("toNodeName"));
    }

    [Test]
    public void GetPossibleRouteCount_FromNodeNotInGraph_ThrowsArguementException()
    {
      Graph graph = new Graph();
      graph.AddRoute(new Route("A", "B", 5));

      var ex = Assert.Throws<ArgumentException>(() => graph.GetPossibleRouteCount("C", "B"));
      Assert.That(ex.ParamName, Is.EqualTo("fromNodeName"));
    }

    [Test]
    public void GetPossibleRouteCount_ToNodeNotInGraph_ThrowsArguementException()
    {
      Graph graph = new Graph();
      graph.AddRoute(new Route("A", "B", 5));

      var ex = Assert.Throws<ArgumentException>(() => graph.GetPossibleRouteCount("A", "C"));
      Assert.That(ex.ParamName, Is.EqualTo("toNodeName"));
    }

    [Test]
    public void GetPossibleRouteCount_SimpleGraphABCD_From_A_To_D_Returns1(){
      Graph graph = CreateSimpleGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "D", false);

      Assert.That(routeCount, Is.EqualTo(1));
    }

    [Test]
    public void GetPossibleRouteCount_SimpleGraphABCD_From_B_To_C_Returns1(){
      Graph graph = CreateSimpleGraph();

      int routeCount = graph.GetPossibleRouteCount("B", "C", false);

      Assert.That(routeCount, Is.EqualTo(1));
    }

    [Test]
    public void GetPossibleRouteCount_SimpleGraphABCD_From_A_To_D_Maximum2Legs_Returns0(){
      Graph graph = CreateSimpleGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "D", true, 2);

      Assert.That(routeCount, Is.EqualTo(0));
    }

    [Test]
    public void GetPossibleRouteCount_SimpleGraphABCD_From_A_To_D_Maximum3Legs_Returns1(){
      Graph graph = CreateSimpleGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "D", true, 3);

      Assert.That(routeCount, Is.EqualTo(1));
    }

    [Test]
    public void GetPossibleRouteCount_SimpleGraphABCD_From_A_To_D_Maximum99Legs_Returns1(){
      Graph graph = CreateSimpleGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "D", true, 99);

      Assert.That(routeCount, Is.EqualTo(1));
    }

    [Test]
    public void GetPossibleRouteCount_SimpleGraphABCD_From_A_To_D_MaximumCost30_Returns0(){
      Graph graph = CreateSimpleGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "D", true, 0, 30);

      Assert.That(routeCount, Is.EqualTo(0));
    }

    [Test]
    public void GetPossibleRouteCount_SimpleGraphABCD_From_A_To_D_MaximumCost31_Returns1(){
      Graph graph = CreateSimpleGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "D", true, 0, 31);

      Assert.That(routeCount, Is.EqualTo(1));
    }

    [Test]
    public void GetPossibleRouteCount_SimpleGraphABCD_From_A_To_D_MaximunCost200_Returns1(){
      Graph graph = CreateSimpleGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "D", true, 0, 200);

      Assert.That(routeCount, Is.EqualTo(1));
    }

    [Test]
    public void GetPossibleRouteCount_SimpleGraphABCD_From_B_To_A_ReturnsZero(){
      Graph graph = CreateSimpleGraph();

      int routeCount = graph.GetPossibleRouteCount("B", "A", false);

      Assert.That(routeCount, Is.EqualTo(0));
    }

    [Test]
    public void GetPossibleRouteCount_CircularGraphABCDA_From_A_To_A_NoConstraints_ThrowsArgumentException(){
      Graph graph = CreateCircularGraph();

      Assert.Throws<ArgumentException>(() => graph.GetPossibleRouteCount("A", "A"));
    }

    [Test]
    public void GetPossibleRouteCount_CircularGraphABCDA_From_A_To_A_NoRepeatedRoute_Returns1(){
      Graph graph = CreateCircularGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "A", false);

      Assert.That(routeCount, Is.EqualTo(1));
    }

    [Test]
    public void GetPossibleRouteCount_CircularGraphABCDA_From_A_To_A_Maximum3Legs_Returns0(){
      Graph graph = CreateCircularGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "A", true, 3);

      Assert.That(routeCount, Is.EqualTo(0));
    }

    [Test]
    public void GetPossibleRouteCount_CircularGraphABCDA_From_A_To_A_Maximum4Legs_Returns1(){
      Graph graph = CreateCircularGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "A", true, 4);

      Assert.That(routeCount, Is.EqualTo(1));
    }

    [Test]
    public void GetPossibleRouteCount_CircularGraphABCDA_From_A_To_A_Maximum8Legs_Returns2(){
      Graph graph = CreateCircularGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "A", true, 8);

      Assert.That(routeCount, Is.EqualTo(2));
    }

    [Test]
    public void GetPossibleRouteCount_CircularGraphABCDA_From_A_To_A_Maximum16Legs_Returns4(){
      Graph graph = CreateCircularGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "A", true, 16);

      Assert.That(routeCount, Is.EqualTo(4));
    }

    [Test]
    public void GetPossibleRouteCount_CircularGraphABCDA_From_A_To_A_MaximumCost30_Returns0(){
      Graph graph = CreateCircularGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "A", true, 0, 30);

      Assert.That(routeCount, Is.EqualTo(0));
    }

    [Test]
    public void GetPossibleRouteCount_CircularGraphABCDA_From_A_To_A_MaximumCost50_Returns0(){
      Graph graph = CreateCircularGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "A", true, 0, 50);

      Assert.That(routeCount, Is.EqualTo(0));
    }


    [Test]
    public void GetPossibleRouteCount_CircularGraphABCDA_From_A_To_A_MaximumCost51_Returns1(){
      Graph graph = CreateCircularGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "A", true, 0, 51);

      Assert.That(routeCount, Is.EqualTo(1));
    }

    [Test]
    public void GetPossibleRouteCount_ComplexGraph_From_A_To_D_NoRepeatedRoute_MaximumCost1000_Returns10(){
      Graph graph = CreateComplexGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "D", false, 0, 1000);

      Assert.That(routeCount, Is.EqualTo(10));
    }

    [Test]
    public void GetPossibleRouteCount_ComplexGraph_From_G_To_E_Maximum7Legs_Returns12(){
      Graph graph = CreateComplexGraph();

      int routeCount = graph.GetPossibleRouteCount("G", "E", true, 7);

      Assert.That(routeCount, Is.EqualTo(12));
    }

    [Test]
    public void GetPossibleRouteCount_ComplexGraph_From_A_To_C_NoRepeatedRoute_Returns95(){
      Graph graph = CreateComplexGraph();

      int routeCount = graph.GetPossibleRouteCount("A", "C", false);

      Assert.That(routeCount, Is.EqualTo(95));
    }

    [Test]
    public void GetPossibleRouteCount_ComplexGraph_From_E_To_E_MaximumCost1200_Returns117(){
      Graph graph = CreateComplexGraph();

      int routeCount = graph.GetPossibleRouteCount("E", "E", true, 0, 1200);

      Assert.That(routeCount, Is.EqualTo(117));
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