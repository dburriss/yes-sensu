using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YesSensu.Messages;

namespace YesSensu.Tests
{
    public class MessageTests
    {
        [Fact]
        public void OkCtor_WithAppNameNullOrEmptyString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Ok(null, "key"));
            Assert.Throws<ArgumentNullException>(() => new Ok("", "key"));

        }

        [Fact]
        public void OkCtor_WithNameNullOrEmptyString_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Ok("app_name", null));
            Assert.Throws<ArgumentNullException>(() => new Ok("app_name", ""));
        }

        [Fact]
        public void OkCtor_WithAppName_SourceIsAppName()
        {
            var sut = new Ok("app_name", "my_key");
            Assert.Equal("app_name", sut.Source);
        }

        [Fact]
        public void OkCtor_WithNameMyKey_NameIsMyKey()
        {
            var sut = new Ok("app_name", "my_key");
            Assert.Equal("my_key", sut.Name);
        }

    }
}
