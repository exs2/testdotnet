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
  public class GetCheapestRouteCostController : ControllerBase
  {
    [HttpPost]
    public ActionResult<int> Post([FromBody]List<Route> routes, 
      [FromQuery]string from, 
      [FromQuery]string to)
    {
      Graph graph = new Graph();
      graph.AddRoutes(routes);
      int cheapestCost = 0;

      try{
        cheapestCost = graph.GetCheapestPath(from, to);
      }
      catch (ArgumentException ex){
        return BadRequest(ex.Message);
      }

      return Ok(cheapestCost);
    }
  }
}