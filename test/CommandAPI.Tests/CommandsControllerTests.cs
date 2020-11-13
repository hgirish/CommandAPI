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