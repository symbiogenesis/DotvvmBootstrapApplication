using System;
using RingDownConsole.Utils.Extensions;
using Xunit;

namespace RingDownConsole.Tests
{
    public class DateTimeExtensionTests
    {
        [Fact]
        public void IsBetween_OlderIsFalse()
        {
            var date = DateTime.UtcNow.AddDays(-20);

            var isBetween = date.IsBetween(10);

            Assert.False(isBetween);
        }

        [Fact]
        public void IsBetween_FutureIsFalse()
        {
            var date = DateTime.UtcNow.AddDays(2);

            var isBetween = date.IsBetween(10);

            Assert.False(isBetween);
        }

        [Fact]
        public void IsBetween_WithinBoundsIsTrue()
        {
            var date = DateTime.UtcNow.AddDays(-1);

            var isBetween = date.IsBetween(10);

            Assert.True(isBetween);
        }
    }
}
