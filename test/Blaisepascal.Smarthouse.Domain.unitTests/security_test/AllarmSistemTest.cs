using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaisepascal.Smarthouse.Domain.unitTests.security_test
{
    public class AllarmSistemTest
    {

        [Fact]
        public void DetectIrruption_SetsIrruptionAndSignalToTrue()
        {
            // Arrange
            var allarmSistem = new AllarmSistem("BrandX", "ModelY");
            // Act
            allarmSistem.DetectIrruption();
            // Assert
            Assert.True(allarmSistem.Irruption);
            Assert.True(allarmSistem.Signal);
            Assert.True(allarmSistem.IrruptionNotification);
        }
    }
}
