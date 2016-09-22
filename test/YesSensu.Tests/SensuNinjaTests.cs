﻿using System;
using Xunit;
using YesSensu.Core;

namespace YesSensu.Tests
{
    public class SensuNinjaTests
    {
        [Fact]
        public void Get_WithNullOrEmptyString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => SensuNinja.Get(null, 0));
            Assert.Throws<ArgumentNullException>(() => SensuNinja.Get("", 0));
        }

        [Fact]
        public void Get_APacemaker_ReturnsAPacemaker()
        {
            var pacemaker = SensuNinja.Get("host", 1);
            Assert.NotNull(pacemaker);
            Assert.IsType<Pacemaker>(pacemaker);
        }

        [Fact]
        public void Get_APacemakerWithTcpClientParam_PacemakerHasTcpClient()
        {
            var pacemaker = SensuNinja.Get("host", 1, ClientType.Tcp);
            Assert.Equal(typeof(SensuTcpClient), pacemaker.Client.GetType());
        }
    }
}
