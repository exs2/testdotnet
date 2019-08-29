using NUnit.Framework;
using System;
using API.Models;

namespace API.UnitTest.Models
{
  [TestFixture]
  public class Graph_AddRouteShould
  {
    [Test]
    public void AddRoute_AddNullRoute_ThrowArgumentNullException()
    {
      Graph graph = new Graph();
      
      var ex = Assert.Throws<ArgumentNullException>(() => graph.AddRoute(null));
      Assert.That(ex.ParamName, Is.EqualTo("route"));
    }

    [Test]
    public void AddRoute_Add1Route_NodeCountIs2()
    {
      Route r = new Route("A", "B", 5);
      Graph graph = new Graph();

      graph.AddRoute(r);

      Assert.That(graph.Nodes.Count, Is.EqualTo(2));
    }

    [Test]
    public void AddRoute_Add1Route_NodeAHasRoute()
    {
      Route r = new Route("A", "B", 5);
      Graph graph = new Graph();

      graph.AddRoute(r);

      Assert.That(graph.Nodes["A"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["A"].Routes["B"], Is.EqualTo(r));
    }

    [Test]
    public void AddRoute_Add1Route_NodeBHasNoRoute()
    {
      Route r = new Route("A", "B", 5);
      Graph graph = new Graph();

      graph.AddRoute(r);

      Assert.That(graph.Nodes["B"].Routes.Count, Is.EqualTo(0));
    }

    [Test]
    public void AddRoute_Add3DifferenceRoutes_6NodesIsInGraph()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("C", "D", 10);
      Route r3 = new Route("E", "F", 10);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);
      graph.AddRoute(r3);

      Assert.That(graph.Nodes.Count, Is.EqualTo(6));
    }

    [Test]
    public void AddRoute_Add3DifferenceRoutes_AllFromNodeHasRoute()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("C", "D", 10);
      Route r3 = new Route("E", "F", 10);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);
      graph.AddRoute(r3);

      Assert.That(graph.Nodes["A"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["A"].Routes["B"], Is.EqualTo(r1));
      Assert.That(graph.Nodes["C"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["C"].Routes["D"], Is.EqualTo(r2));
      Assert.That(graph.Nodes["E"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["E"].Routes["F"], Is.EqualTo(r3));
    }

    [Test]
    public void AddRoute_Add3DifferenceRoutes_AllToNodeHasNoRoute()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("C", "D", 10);
      Route r3 = new Route("E", "F", 10);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);
      graph.AddRoute(r3);

      Assert.That(graph.Nodes["B"].Routes.Count, Is.EqualTo(0));
      Assert.That(graph.Nodes["D"].Routes.Count, Is.EqualTo(0));
      Assert.That(graph.Nodes["F"].Routes.Count, Is.EqualTo(0));
    }

    [Test]
    public void AddRoute_Add2DuplicateRoutes_2NodesIsInGraph()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("A", "B", 5);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);

      Assert.That(graph.Nodes.Count, Is.EqualTo(2));
    }

    [Test]
    public void AddRoute_Add2DuplicateRoutes_NodeAHasRoute()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("A", "B", 5);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);

      Assert.That(graph.Nodes["A"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["A"].Routes["B"], Is.EqualTo(r1));
    }

    [Test]
    public void AddRoute_Add2DuplicateRoutes_NodeBHasNoRoute()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("A", "B", 5);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);

      Assert.That(graph.Nodes["B"].Routes.Count, Is.EqualTo(0));
    }

    [Test]
    public void AddRoute_Add2DuplicateRoutesWithDifferenceCost_2NodesIsInGraph()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("A", "B", 50);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);

      Assert.That(graph.Nodes.Count, Is.EqualTo(2));
    }

    [Test]
    public void AddRoute_Add2DuplicateRoutesWithDifferenceCost_LatestRouteIsInGraph()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("A", "B", 50);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);

      Assert.That(graph.Nodes["A"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["A"].Routes["B"], Is.EqualTo(r2));
    }

    [Test]
    public void AddRoute_Add2DuplicateAnd2UniqueRoutes_4NodesIsInGraph()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("A", "B", 50);
      Route r3 = new Route("C", "D", 55);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);
      graph.AddRoute(r3);

      Assert.That(graph.Nodes.Count, Is.EqualTo(4));
    }

    [Test]
    public void AddRoute_Add3OverlappedRoutes_4NodesIsInGraph()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("B", "C", 50);
      Route r3 = new Route("C", "D", 55);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);
      graph.AddRoute(r3);

      Assert.That(graph.Nodes.Count, Is.EqualTo(4));
    }

    [Test]
    public void AddRoute_Add3OverlappedRoutes_3FromNodesHasRoute()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("B", "C", 50);
      Route r3 = new Route("C", "D", 55);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);
      graph.AddRoute(r3);

      Assert.That(graph.Nodes["A"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["A"].Routes["B"], Is.EqualTo(r1));
      Assert.That(graph.Nodes["B"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["B"].Routes["C"], Is.EqualTo(r2));
      Assert.That(graph.Nodes["C"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["C"].Routes["D"], Is.EqualTo(r3));
    }

    [Test]
    public void AddRoute_Add2ForkedRoutes_3NodesIsInGraph()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("A", "C", 50);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);

      Assert.That(graph.Nodes.Count, Is.EqualTo(3));
    }

    [Test]
    public void AddRoute_Add2ForkedRoutes_1FromNodesHas2Routes()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("A", "C", 50);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);

      Assert.That(graph.Nodes["A"].Routes.Count, Is.EqualTo(2));
      Assert.That(graph.Nodes["A"].Routes["B"], Is.EqualTo(r1));
      Assert.That(graph.Nodes["A"].Routes["C"], Is.EqualTo(r2));
    }

    [Test]
    public void AddRoute_AddRoundTripRoutes_2NodesIsInGraph()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("B", "A", 50);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);

      Assert.That(graph.Nodes.Count, Is.EqualTo(2));
    }

    [Test]
    public void AddRoute_AddRoundTripRoutes_BothNodeHasRoute()
    {
      Route r1 = new Route("A", "B", 5);
      Route r2 = new Route("B", "A", 50);
      Graph graph = new Graph();

      graph.AddRoute(r1);
      graph.AddRoute(r2);

      Assert.That(graph.Nodes["A"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["A"].Routes["B"], Is.EqualTo(r1));
      Assert.That(graph.Nodes["B"].Routes.Count, Is.EqualTo(1));
      Assert.That(graph.Nodes["B"].Routes["A"], Is.EqualTo(r2));
    }
  }
}