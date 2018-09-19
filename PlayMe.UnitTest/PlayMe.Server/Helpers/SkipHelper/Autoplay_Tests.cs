﻿using NUnit.Framework;
using PlayMe.Common.Model;
using PlayMe.Server.Helpers.SkipHelperRules;
using PlayMe.UnitTest.Plumbing;

namespace PlayMe.UnitTest.PlayMe.Server.Helpers.SkipHelperRules
{
    [TestFixture]
    public class Autoplay_Tests : TestBase<AutoplaySkipRule>
    {
        [TestCase("autoplay")]
        [TestCase("Autoplay")]
        [TestCase("AUTOPLAY")]
        [TestCase("autoPlay")]
        [TestCase("autoPlay - variant")]
        [TestCase("Autoplay : dope beats")]
        public void If_the_user_is_autoplay_GetRequiredVetoCount_returns_2(string user)
        {
            // Arrange
            // Act
            var result = ClassUnderTest.GetRequiredVetoCount(new QueuedTrack {User = user});

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [TestCase("milliways\fprefect")]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("rkfghagfkjsafnsdfkn")]
        [TestCase("Let's assume autoplay has to go at the very beginning")]
        public void If_the_user_is_not_autoplay_GetRequiredVetoCount_returns_intmax(string user)
        {
            // Arrange
            // Act
            var result = ClassUnderTest.GetRequiredVetoCount(new QueuedTrack { User = user });

            // Assert
            Assert.That(result, Is.EqualTo(int.MaxValue));
        }
    }
}