using System;
using System.Collections.Generic;

namespace API.Models{
  public class Graph{
    public Dictionary<string, Node> Nodes { get; private set; }
    public const int maxCostLimit = 50000;
    public const int maxLegLimit = 1000;

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

    public int GetPossibleRouteCount(string fromNodeName, string toNodeName, bool allowRepeatedRoute = true, int legCountLimit = 0, int costLimit = 0){
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
      if(allowRepeatedRoute && legCountLimit == 0 && costLimit == 0){
        throw new ArgumentException("Put at least 1 constraints to avoid infinite loop");
      }
      if(allowRepeatedRoute && legCountLimit > maxLegLimit && costLimit > maxCostLimit){
        throw new ArgumentException("Limit is too high");
      }

      legCountLimit = legCountLimit == 0 ? maxLegLimit : legCountLimit;
      costLimit = costLimit == 0 ? maxCostLimit : costLimit;

      return RecursivelyCountRoutes(fromNodeName, toNodeName, allowRepeatedRoute, legCountLimit, costLimit, new List<string>(){ fromNodeName });
    }

    private int RecursivelyCountRoutes(string currentNodeName, string toNodeName, bool allowRepeatedRoute, int legCountLimit, int costLimit, List<string> currentPath){
      Node currentNode = Nodes[currentNodeName];
      int routeCount = 0;

      // terminal conditions
      if(CalculateCostGivenPath(currentPath) >= costLimit){
        return 0;
      }
      else if(currentPath.Count-1 > legCountLimit){
        return 0;
      }
      else if(!allowRepeatedRoute && IsThePathRepeated(currentPath)){
        return 0;
      }
      else if(currentPath.Count > 1 && currentNode.Name == toNodeName){
        foreach(string node in currentPath){
          Console.Write(node + "=>");
        }
        Console.WriteLine();
        routeCount++;
      }

      foreach(Route route in currentNode.Routes.Values){
        List<string> nextPath = new List<string>(currentPath);
        nextPath.Add(route.To);

        routeCount += RecursivelyCountRoutes(route.To, toNodeName, allowRepeatedRoute, legCountLimit, costLimit, nextPath);
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

      Dictionary<string,Dictionary<string,int>> cheapestCostTable = constructCheapestCostTable();

      foreach(string from in cheapestCostTable.Keys){
        foreach(string to in cheapestCostTable.Keys){
          Console.WriteLine(from + " => " + to + " = " + cheapestCostTable[from][to]);
        }
      }
      
      int cheapestCost = cheapestCostTable[fromNodeName][toNodeName];
      if(cheapestCost == int.MaxValue){
        throw new ArgumentException(string.Format("No valid path from {0} => {1}", fromNodeName, toNodeName));
      }

      return cheapestCost;
    }

    private Dictionary<string, Dictionary<string, int>> constructCheapestCostTable(){
      
      // Apply dynamic programming for optimal performance
      Dictionary<string,Dictionary<string,int>> minCostTable = new Dictionary<string, Dictionary<string, int>>();

      // initialize
      foreach(Node startNode in Nodes.Values){
        minCostTable[startNode.Name] = new Dictionary<string, int>();
        foreach(Node endNode in Nodes.Values){
          minCostTable[startNode.Name][endNode.Name] = int.MaxValue;
        }
      }

      foreach(Node node in Nodes.Values){
        foreach(Route route in node.Routes.Values){
          
          int cost = route.Cost;

          if(cost < minCostTable[route.From][route.To]){
            minCostTable[route.From][route.To] = cost;
          }
          
          foreach(Node fromNode in Nodes.Values){
            foreach(Node toNode in Nodes.Values){
              int existingMinCost = minCostTable[fromNode.Name][toNode.Name];
              int minCostFromPrevious = fromNode.Name != route.From ? minCostTable[fromNode.Name][route.From] : 0;
              int minCostFromNext = toNode.Name != route.To ? minCostTable[route.To][toNode.Name] : 0;

              if(minCostFromPrevious != int.MaxValue && minCostFromNext != int.MaxValue){
                if(minCostFromPrevious + minCostFromNext + cost < existingMinCost){
                  minCostTable[fromNode.Name][toNode.Name] = minCostFromPrevious + minCostFromNext + cost;
                }
              }
            }
          }
        }
      }

      return minCostTable;
    }
  }
}