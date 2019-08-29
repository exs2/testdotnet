using NUnit.Framework;
using System;
using API.Models;
using System.Collections.Generic;

namespace API.UnitTest.Models{
  
  [TestFixture]
  public class Graph_GetCheapestPathShould{

    [Test]
    public void GetCheapestPath_fromNodeNull_ThrowsArguementNullException(){
      Graph graph = new Graph();
      graph.AddRoute(new Route("A", "B", 5));

      var ex = Assert.Throws<ArgumentNullException>(() => graph.GetCheapestPath(null, "B"));
      Assert.That(ex.ParamName, Is.EqualTo("fromNodeName"));
    }

    [Test]
    public void GetCheapestPath_toNodeNull_ThrowsArguementNullException(){
      Graph graph = new Graph();
      graph.AddRoute(new Route("A", "B", 5));

      var ex = Assert.Throws<ArgumentNullException>(() => graph.GetCheapestPath("A", null));
      Assert.That(ex.ParamName, Is.EqualTo("toNodeName"));
    }

    [Test]
    public void GetCheapestPath_NoNode_ThrowsArguementException(){
      Graph graph = new Graph();

      var ex = Assert.Throws<ArgumentException>(() => graph.GetCheapestPath("A", "B"));
      Assert.That(ex.ParamName, Is.EqualTo("fromNodeName"));
    }

    [Test]
    public void GetCheapestPath_FromNodeNotInGraph_ThrowsArguementException(){
      Graph graph = new Graph();
      graph.AddRoute(new Route("A", "B", 5));

      var ex = Assert.Throws<ArgumentException>(() => graph.GetCheapestPath("C", "B"));
      Assert.That(ex.ParamName, Is.EqualTo("fromNodeName"));
    }

    [Test]
    public void GetCheapestPath_toNodeNotInGraph_ThrowsArguementException(){
      Graph graph = new Graph();
      graph.AddRoute(new Route("A", "B", 5));

      var ex = Assert.Throws<ArgumentException>(() => graph.GetCheapestPath("A", "C"));
      Assert.That(ex.ParamName, Is.EqualTo("toNodeName"));
    }

    [Test]
    public void GetCheapestPath_SimpleGraph_AB_Returns5(){
      Graph graph = CreateSimpleGraph();

      int cost = graph.GetCheapestPath("A", "B");

      Assert.That(cost, Is.EqualTo(5));
    }

    [Test]
    public void GetCheapestPath_SimpleGraph_AD_Returns30(){
      Graph graph = CreateSimpleGraph();

      int cost = graph.GetCheapestPath("A", "D");

      Assert.That(cost, Is.EqualTo(30));
    }

    [Test]
    public void GetCheapestPath_SimpleGraph_BA_ThrowsArgumentException(){
      Graph graph = CreateSimpleGraph();

      var ex = Assert.Throws<ArgumentException>(() => graph.GetCheapestPath("B", "A"));
    }

    [Test]
    public void GetCheapestPath_InterconnectedGraph_AD_Returns15(){
      Graph graph = CreateInterconnectedGraph();

      var cost = graph.GetCheapestPath("A", "D");

      Assert.That(cost, Is.EqualTo(15));
    }

    [Test]
    public void GetCheapestPath_InterconnectedGraph_BD_Returns25(){
      Graph graph = CreateInterconnectedGraph();

      var cost = graph.GetCheapestPath("B", "D");

      Assert.That(cost, Is.EqualTo(25));
    }

    [Test]
    public void GetCheapestPath_ComplexGraph_AD_Returns600(){
      Graph graph = CreateComplexGraph();

      var cost = graph.GetCheapestPath("A", "D");

      Assert.That(cost, Is.EqualTo(600));
    }

    [Test]
    public void GetCheapestPath_ComplexGraph_CA_Returns370(){
      Graph graph = CreateComplexGraph();

      var cost = graph.GetCheapestPath("C", "A");

      Assert.That(cost, Is.EqualTo(370));
    }

    [Test]
    public void GetCheapestPath_ComplexGraph_AC_Returns110(){
      Graph graph = CreateComplexGraph();

      var cost = graph.GetCheapestPath("A", "C");

      Assert.That(cost, Is.EqualTo(110));
    }

    [Test]
    public void GetCheapestPath_ComplexGraph_HH_Returns550(){
      Graph graph = CreateComplexGraph();

      var cost = graph.GetCheapestPath("H", "H");

      Assert.That(cost, Is.EqualTo(550));
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
      Route r2 = new Route("B", "A", 10);
      Route r3 = new Route("B", "C", 15);
      Route r4 = new Route("C", "B", 20);
      Route r5 = new Route("C", "D", 25);
      Route r6 = new Route("D", "C", 30);
      Graph graph = new Graph();
      
      graph.AddRoute(r1);
      graph.AddRoute(r2);
      graph.AddRoute(r3);
      graph.AddRoute(r4);
      graph.AddRoute(r5);
      graph.AddRoute(r6);

      return graph;
    }

    public Graph CreateInterconnectedGraph(){
      List<Route> routes = new List<Route>(){
        new Route("A", "B", 5),
        new Route("A", "C", 10),
        new Route("A", "D", 50),
        new Route("B", "C", 20),
        new Route("B", "D", 30),
        new Route("C", "D", 5)
      };

      Graph graph = new Graph();

      graph.AddRoutes(routes);

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
