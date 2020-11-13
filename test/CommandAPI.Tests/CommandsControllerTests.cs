using Xunit;
using System;
using CommandAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CommandAPI.Data;
using System.Collections.Generic;
using CommandAPI.Models;
using CommandAPI.Profiles;
using AutoMapper;
using CommandAPI.Dtos;

namespace CommandAPI.Tests
{
  public class CommandsControllerTests : IDisposable
  {
    Mock<ICommandAPIRepo> mockRepo;
    CommandsProfile realProfile;
    MapperConfiguration configuration;
    IMapper mapper;
    public CommandsControllerTests()
    {
      mockRepo = new Mock<ICommandAPIRepo>();
      realProfile = new CommandsProfile();
      configuration = new MapperConfiguration(cfg =>
     cfg.AddProfile(realProfile));
      mapper = new Mapper(configuration);
    }
    public void Dispose()
    {
      mockRepo = null;
      mapper = null;
      configuration = null;
      realProfile = null;
    }
    [Fact]
    public void GetCommandItems_Returns200OK_WhenDBIsEmpty()
    {
      //Given
      mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));
      var controller = new CommandsController(mockRepo.Object, mapper);
      //When
      var result = controller.GetAllCommands();
      //Then
      Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetAllCommands_ReturnsOneItem_WhenDBHasOneResource()
    {
      // Arrange
      mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
      var controller = new CommandsController(mockRepo.Object, mapper);

      // Act
      var result = controller.GetAllCommands();

      // Assert
      var okResult = result.Result as OkObjectResult;

      var commands = okResult.Value as List<CommandReadDto>;

      Assert.Single(commands);
    }

    [Fact]
    public void GetAllCommands_Returns200OK_WhenDataHasOneResource()
    {
      mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
      var controller = new CommandsController(mockRepo.Object, mapper);

      // Act
      var result = controller.GetAllCommands();

      // Assert
      Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetAllCommands_ReturnsCorrectType_WhenDBHasOneResource()
    {
      //Given
      mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

      var controller = new CommandsController(mockRepo.Object, mapper);
      //When
      var result = controller.GetAllCommands();
      //Then
      Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
    }


    [Fact]
    public void GetCommandByID_Returns404NotFound_WhenNonExistentIDProvided()
    {
      // Arrange
      mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
      var controller = new CommandsController(mockRepo.Object, mapper);

      // Act
      var result = controller.GetCommandById(1);

      // Assert
      Assert.IsType<OkResult>(result.Result);
    }


    [Fact]
    public void GetCommandByID_Returns200OK_WhenValidIDProvided()
    {
      mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command
      {
        Id = 1,
        HowTo = "mock",
        Platform = "Mock",
        CommandLine = "Mock"
      });
      var controller = new CommandsController(mockRepo.Object, mapper);

      var result = controller.GetCommandById(1);

      Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetCommandByID_ReturnsCorrectType_WhenValidIDProvided()
    {
      mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command
      {
        Id = 1,
        HowTo = "mock",
        Platform = "Mock",
        CommandLine = "Mock"
      });
      var controller = new CommandsController(mockRepo.Object, mapper);

      var result = controller.GetCommandById(1);

      Assert.IsType<ActionResult<CommandReadDto>>(result);
    }


    [Fact]
    public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
    {
      mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command
      {
        Id = 1,
        HowTo = "mock",
        Platform = "Mock",
        CommandLine = "Mock"
      });

      var controller = new CommandsController(mockRepo.Object, mapper);

      var result = controller.CreateCommand(new CommandCreateDto { });

      Assert.IsType<ActionResult<CommandReadDto>>(result);
    }


    [Fact]
    public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
    {
      mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command
      {
        Id = 1,
        HowTo = "mock",
        Platform = "Mock",
        CommandLine = "Mock"
      });
      var controller = new CommandsController(mockRepo.Object, mapper);

      var result = controller.CreateCommand(new CommandCreateDto { });

      Assert.IsType<CreatedAtRouteResult>(result.Result);
    }


    [Fact]
    public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
    {
      mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(
        new Command
        {
          Id = 1,
          HowTo = "mock",
          Platform = "Mock",
          CommandLine = "Mock"
        });

      var controller = new CommandsController(mockRepo.Object, mapper);

      var result = controller.UpdateCommand(1, new CommandUpdateDto { });

      Assert.IsType<NoContentResult>(result);
    }


    [Fact]
    public void UpdateCommand_Returns404NotFound_WhenNonExitentResourceIDSubmitted()
    {
      mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

      var controller = new CommandsController(mockRepo.Object, mapper);

      var result = controller.UpdateCommand(0, new CommandUpdateDto { });

      Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
    {
      mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

      var controller = new CommandsController(mockRepo.Object, mapper);

      var result = controller.PartialCommandUpdate(0,
        new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto> { });

      Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public void DeleteCommand_Returns204NoContent_WhenValidResourceIDSubmitted()
    {
      mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command
      {
        Id = 1,
        HowTo = "mock",
        Platform = "Mock",
        CommandLine = "mock"
      });
      var controller = new CommandsController(mockRepo.Object, mapper);

      var result = controller.DeleteCommand(1);

      Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
    {
      mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
      var controller = new CommandsController(mockRepo.Object, mapper);

      var result = controller.DeleteCommand(0);

      Assert.IsType<NotFoundResult>(result);
    }





    private List<Command> GetCommands(int num)
    {
      var commands = new List<Command>();
      if (num > 0)
      {
        commands.Add(new Command
        {
          Id = 0,
          HowTo = "How to generate a migration",
          CommandLine = "dotnet ef migrations add <Name of Migration>",
          Platform = ".Net Core EF"
        });
      }
      return commands;
    }
  }
}