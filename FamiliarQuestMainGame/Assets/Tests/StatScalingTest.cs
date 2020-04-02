using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StatScalingTest
    {
        [Test]
        [Description("It correctly assesses the appropriate range of level 1 core attributes")]
        public void AssessCoreAttributeRange() {
            Assert.IsTrue(SecondaryStatUtility.DetermineMinimum(1) == 1);
            Assert.IsTrue(SecondaryStatUtility.DetermineAverage(1) == 10);
            Assert.IsTrue(SecondaryStatUtility.DetermineMaximum(1) == 20);
        }
    }
}
