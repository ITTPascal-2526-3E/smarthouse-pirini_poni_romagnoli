using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;
using BlaisePascal.SmartHouse.Domain.Security.SecurityAbstraction;

namespace Blaisepascal.Smarthouse.Domain.unitTests.security_test
{
    public class AlarmSystemTest
    {

        [Fact]
        public void DetectIntrusion_SetsIntrusionAndSignalToTrue()
        {
            // Arrange
            var alarmSystem = new AlarmSystem("BrandX", "ModelY");
            // Act
            alarmSystem.DetectIntrusion();
            // Assert
            Assert.True(alarmSystem.Intrusion);
            Assert.True(alarmSystem.Signal);
            Assert.True(alarmSystem.IntrusionNotification);
        }
    }
}
