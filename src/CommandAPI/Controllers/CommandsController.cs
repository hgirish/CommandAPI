using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommandAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CommandsController : ControllerBase
  {
    private readonly ICommandAPIRepo _repository;
    public CommandsController(ICommandAPIRepo repository)
    {
      this._repository = repository;

    }
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
      var commandItems = _repository.GetAllCommands();
      return Ok(commandItems);
    }
    [HttpGet("{id}")]
    public ActionResult<Command> GetCommandById(int id)
    {
      var commandItem = _repository.GetCommandById(id);
      if (commandItem == null)
      {
        return NotFound();
      }
      return Ok(commandItem);
    }
  }
}