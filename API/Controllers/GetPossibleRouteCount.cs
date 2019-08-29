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
  public class GetPossibleRouteCountController : ControllerBase
  {
    [HttpPost]
    public ActionResult<int> Post([FromBody]List<Route> routes, 
      [FromQuery]string from, 
      [FromQuery]string to,
      [FromQuery]bool allowSameLeg = true,
      [FromQuery]int maxLegs = 0, 
      [FromQuery]int maxCost = 0)
    {
      Graph graph = new Graph();
      graph.AddRoutes(routes);
      int routeCount = 0;

      try{
        routeCount = graph.GetPossibleRouteCount(from, to, allowSameLeg, maxLegs, maxCost);
      }
      catch (ArgumentException ex){
        return BadRequest(ex.Message);
      }

      return Ok(routeCount);
    }
  }
}