using NUnit.Framework;
using System;
using API.Models;
using System.Collections.Generic;

namespace API.UnitTest.Models
{
  [TestFixture]
  public class Graph_IsThePathRepeatedShould
  {
    [Test]
    public void IsThePathRepeated_NoPath_ReturnsFalse(){
      List<string> path = new List<string>();

      bool isRepeated = Graph.IsThePathRepeated(path);

      Assert.That(isRepeated, Is.EqualTo(false));
    }

    [Test]
    public void IsThePathRepeated_1Node_ReturnsFalse(){
      List<string> path = new List<string>(){ "A" };

      bool isRepeated = Graph.IsThePathRepeated(path);

      Assert.That(isRepeated, Is.EqualTo(false));
    }

    [Test]
    public void IsThePathRepeated_ABCD_ReturnsFalse(){
      List<string> path = new List<string>(){ "A", "B", "C", "D" };

      bool isRepeated = Graph.IsThePathRepeated(path);

      Assert.That(isRepeated, Is.EqualTo(false));
    }

    [Test]
    public void IsThePathRepeated_ABA_ReturnsFalse(){
      List<string> path = new List<string>(){ "A", "B", "A" };

      bool isRepeated = Graph.IsThePathRepeated(path);

      Assert.That(isRepeated, Is.EqualTo(false));
    }

    [Test]
    public void IsThePathRepeated_ABAC_ReturnsFalse(){
      List<string> path = new List<string>(){ "A", "B", "A", "C" };

      bool isRepeated = Graph.IsThePathRepeated(path);

      Assert.That(isRepeated, Is.EqualTo(false));
    }

    [Test]
    public void IsThePathRepeated_ABABC_ReturnsTrue(){
      List<string> path = new List<string>(){ "A", "B", "A", "B", "C" };

      bool isRepeated = Graph.IsThePathRepeated(path);

      Assert.That(isRepeated, Is.EqualTo(true));
    }    

    [Test]
    public void IsThePathRepeated_ABCABCD_ReturnsTrue(){
      List<string> path = new List<string>(){ "A", "B", "C", "A", "B", "C", "D" };

      bool isRepeated = Graph.IsThePathRepeated(path);

      Assert.That(isRepeated, Is.EqualTo(true));
    }
  }
}