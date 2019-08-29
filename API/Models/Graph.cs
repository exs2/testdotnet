using System;
using System.Collections.Generic;

namespace API.Models{
  public class Graph{
    public Dictionary<string, Node> Nodes { get; private set; }
    public const int maxCostLimit = 99999;
    public const int maxNodeLimit = 1000;

    public Graph(){
      Nodes = new Dictionary<string, Node>();
    }

    public void AddRoutes(IList<Route> routes){
      foreach(Route route in routes){
        AddRoute(route);
      }
    }

    public void AddRoute(Route route){
      if(route is null){
        throw new ArgumentNullException(nameof(route));
      }

      Node from = CreateNodeIfNotExists(route.From);
      Node to = CreateNodeIfNotExists(route.To);

      from.Routes[route.To] = route;
    }

    private Node CreateNodeIfNotExists(string name){
      if(Nodes.ContainsKey(name)){
        return Nodes[name];
      }
      else{
        Nodes[name] = new Node(name);
        return Nodes[name];
      }
    }

    public int CalculateCostGivenPath(IList<string> path){
      int totalCost = 0;
      for(int i = 0; i < path.Count - 1; i++){
        string fromNodeName = path[i];
        string toNodeName = path[i+1];

        if(!Nodes.ContainsKey(fromNodeName)){
          throw new ArgumentException(
            string.Format("Path contains invalid node : {0}", fromNodeName), 
            "path");
        }
        else if(!Nodes[fromNodeName].Routes.ContainsKey(toNodeName)){
          throw new ArgumentException(
            string.Format("Path contains invalid route : {0} -> {1}", fromNodeName, toNodeName), 
            "path");
        }

        int cost = Nodes[fromNodeName].Routes[toNodeName].Cost;

        totalCost += cost;
      }

      return totalCost;
    }

    public int GetPossibleRouteCount(string fromNodeName, string toNodeName, bool allowRepeatedRoute = true, int nodeCountLimit = 0, int costLimit = 0){
      if(fromNodeName is null){
        throw new ArgumentNullException(nameof(fromNodeName));
      }
      if(toNodeName is null){
        throw new ArgumentNullException(nameof(toNodeName));
      }
      if(!Nodes.ContainsKey(fromNodeName)){
        throw new ArgumentException("fromNodeName is not in the graph", nameof(fromNodeName));
      }
      if(!Nodes.ContainsKey(toNodeName)){
        throw new ArgumentException("toNodeName is not in the graph", nameof(toNodeName));
      }
      if(allowRepeatedRoute && nodeCountLimit == 0 && costLimit == 0){
        throw new ArgumentException("Put at least 1 constraints to avoid infinite loop");
      }
      if(allowRepeatedRoute && nodeCountLimit > maxNodeLimit && costLimit > maxCostLimit){
        throw new ArgumentException("Limit is too high");
      }

      nodeCountLimit = nodeCountLimit == 0 ? maxNodeLimit : nodeCountLimit;
      costLimit = costLimit == 0 ? maxCostLimit : costLimit;

      return RecursivelyCountRoutes(fromNodeName, toNodeName, allowRepeatedRoute, nodeCountLimit, costLimit, new List<string>(){ fromNodeName });
    }

    private int RecursivelyCountRoutes(string currentNodeName, string toNodeName, bool allowRepeatedRoute, int nodeCountLimit, int costLimit, List<string> currentPath){
      Node currentNode = Nodes[currentNodeName];
      int routeCount = 0;
      foreach(string nodeName in currentPath){
        Console.Write(nodeName + " => ");
      }
      Console.WriteLine();

      foreach(Route route in currentNode.Routes.Values){
        List<string> nextPath = new List<string>(currentPath);
        nextPath.Add(route.To);

        // terminal conditions
        if(CalculateCostGivenPath(nextPath) > costLimit){
          return 0;
        }
        else if(nextPath.Count > nodeCountLimit){
          return 0;
        }
        else if(!allowRepeatedRoute && IsThePathRepeated(nextPath)){
          return 0;
        }
        else if(route.To == toNodeName){
          return 1 + RecursivelyCountRoutes(route.To, toNodeName, allowRepeatedRoute, nodeCountLimit, costLimit, nextPath);
        }

        routeCount += RecursivelyCountRoutes(route.To, toNodeName, allowRepeatedRoute, nodeCountLimit, costLimit, nextPath);
      }

      return routeCount;
    }

    public static bool IsThePathRepeated(List<string> path){
      HashSet<Route> routeTravelled = new HashSet<Route>();

      for(int i = 0; i < path.Count-1; i++){
        Route route = new Route(path[i], path[i+1], 0);
        
        if(routeTravelled.Contains(route)){
          return true;
        }

        routeTravelled.Add(route);
      }

      return false;
    }

    public int GetCheapestPath(string fromNodeName, string toNodeName){
      if(fromNodeName is null){
        throw new ArgumentNullException(nameof(fromNodeName));
      }
      if(toNodeName is null){
        throw new ArgumentNullException(nameof(toNodeName));
      }
      if(!Nodes.ContainsKey(fromNodeName)){
        throw new ArgumentException("fromNodeName is not in the graph", nameof(fromNodeName));
      }
      if(!Nodes.ContainsKey(toNodeName)){
        throw new ArgumentException("toNodeName is not in the graph", nameof(toNodeName));
      }

      // Apply dynamic programming for optimal performance
      Dictionary<string,Dictionary<string,int>> costTable = new Dictionary<string, Dictionary<string, int>>();

      foreach(Node startNode in Nodes.Values){
        foreach(Node endNode in Nodes.Values){
          costTable[startNode.Name][endNode.Name] = int.MaxValue;
        }
      }
      costTable[fromNodeName][fromNodeName] = 0;

      foreach(Node node in Nodes.Values){
        foreach(Route route in node.Routes.Values){
          int costFromStart = costTable[];
          if(route.Cost > costTable[route.From][route.To]){

          }
        }
      }


      return -1;
    }
  }
}