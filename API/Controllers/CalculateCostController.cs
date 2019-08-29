using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;

namespace API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CalculateCostController : ControllerBase
  {
    [HttpPost]
    public ActionResult<int> Post([FromBody]CalculateCostRequest request)
    {
      Graph graph = new Graph();
      graph.AddRoutes(request.Routes);
      int cost = 0;

      try{
        cost = graph.CalculateCostGivenPath(request.Path);
      }
      catch (ArgumentException ex){
        return BadRequest(ex.Message);
      }

      return Ok(cost);
    }
  }
  
  public class CalculateCostRequest{
    public List<Route> Routes { get; set; }
    public List<string> Path { get; set; }
  }
}