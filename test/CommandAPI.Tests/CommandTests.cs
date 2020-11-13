using Xunit;
using CommandAPI.Models;
using System;

namespace CommandAPI.Tests
{
  public class CommandTests : IDisposable
  {
    Command testCommand;
    public CommandTests()
    {
      testCommand = new Command
      {
        HowTo = "Do something awesome",
        Platform = "Some Platform",
        CommandLine = "Some CommandLine"
      };
    }
    [Fact]
    public void CanChangeHowTo()
    {
      testCommand.HowTo = "Execute Unit Tests";
      //Then
      Assert.Equal("Execute Unit Tests", testCommand.HowTo);
    }
    [Fact]
    public void CanChangePlatform()
    {
      //Given

      //When
      testCommand.Platform = "xunit";
      //Then
      Assert.Equal("xunit", testCommand.Platform);
    }
    [Fact]
    public void CanChangeCommandLine()
    {
      //Given


      //When
      testCommand.CommandLine = "dotnet test";
      //Then
      Assert.Equal("dotnet test", testCommand.CommandLine);
    }

    public void Dispose()
    {
      testCommand = null;
    }
  }
}