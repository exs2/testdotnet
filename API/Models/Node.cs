using System.Collections.Generic;

namespace API.Models{
  public class Node{
    public string Name { get; set; }
    public Dictionary<string, Route> Routes { get; private set; }

    public Node(){
      this.Routes = new Dictionary<string, Route>();
    }
    
    public Node(string name) : this(){
      this.Name = name;
    }
  }
}