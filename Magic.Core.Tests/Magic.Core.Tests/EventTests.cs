using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Magic.Core.Tests
{
    [TestFixture]
    public class EventTests
    {
        [Test]
        public void TestEventLoad()
        {
            var _sut = new Magic.Core.Event();
            _sut.LoadEvent("FRF");
        }
    }
}
