

namespace API.Models
{

  public class Route
  {
    public string From { get; set; }
    public string To { get; set; }
    public int Cost { get; set; }

    public Route()
    {

    }

    public Route(string from, string to, int cost)
    {
      this.From = from;
      this.To = to;
      this.Cost = cost;
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      Route r = obj as Route;
      if (r is null)
      {
        return false;
      }

      // Return true if the fields match:
      return (From == r.From) && (To == r.To) && (Cost == r.Cost);
    }

    public bool Equals(Route r)
    {
      // If parameter is null return false:
      if (r is null)
      {
        return false;
      }

      // Return true if the fields match:
      return (From == r.From) && (To == r.To) && (Cost == r.Cost);
    }

    public override int GetHashCode()
    {
      return From.GetHashCode() ^ To.GetHashCode() ^ Cost;
    }
  }
}