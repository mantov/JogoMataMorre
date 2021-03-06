// <copyright file="PlayerTest.cs">Copyright ©  2016</copyright>
using System;
using JogoMataMorre.Entities;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JogoMataMorre.Tests
{
    /// <summary>This class contains parameterized unit tests for Player</summary>
    [PexClass(typeof(Player))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class PlayerTest
    {
    }
}
