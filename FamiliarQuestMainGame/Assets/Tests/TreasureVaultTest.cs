using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TreasureVaultTest
    {
        TreasureVault treasureVault = null;

        [Test]
        [Timeout(10000)]
        [Description("It creates rooms in a network graph.")]
        public void CreateRoomsRuns() {
            for (int i = 0; i < 1000; i++) SetUpTreasureVault();
        }

        [Test]
        [Timeout(10000)]
        [Description("It creates the layout object successfully.")]
        public void InitTreasureVaultLayoutRuns() {
            SetUpTreasureVault();
            for (int i = 0; i < 1000; i++) {
                var tvl = new TreasureVaultLayout(treasureVault);
            }
        }

        [Test]
        [Timeout(10000)]
        [Description("It consistently completes the entire level creation process in <1 second.")]
        public void LayoutRoomsRuns() {
            int iterations = 100;
            for (int i=0; i<iterations; i++) {
                TestContext.WriteLine("Completed iteration " + (i + 1).ToString() + "/" + iterations.ToString());
                treasureVault = new TreasureVault();
                Assert.IsTrue(Task.Factory.StartNew(treasureVault.Initialize).Wait(TimeSpan.FromSeconds(1)));
            }
        }

        private void SetUpTreasureVault() {
            treasureVault = new TreasureVault();
            treasureVault.rooms = treasureVault.CreateRooms();
        }
    }

}

