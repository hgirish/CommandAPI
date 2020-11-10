using System;
using System.Collections.Generic;
using System.Linq;
using CommandAPI.Models;

namespace CommandAPI.Data
{
  public class SqlCommandAPIRepo : ICommandAPIRepo
  {
    private readonly CommandContext _context;
    public SqlCommandAPIRepo(CommandContext context)
    {
      this._context = context;

    }
    public void CreateCommand(Command command)
    {
      if (command == null)
      {
        throw new ArgumentNullException(nameof(command));
      }
      _context.CommandItems.Add(command);
    }

    public void DeleteCommand(Command command)
    {
      if (command == null)
      {
        throw new ArgumentNullException(nameof(command));
      }
      _context.CommandItems.Remove(command);
    }

    public IEnumerable<Command> GetAllCommands()
    {
      return _context.CommandItems.ToList();
    }

    public Command GetCommandById(int id)
    {
      return _context.CommandItems.FirstOrDefault(p => p.Id == id);
    }

    public bool SaveChanges()
    {
      return (_context.SaveChanges() >= 0);
    }

    public void UpdateCommand(Command command)
    {
      // We don't need to do anything here.
    }
  }
}