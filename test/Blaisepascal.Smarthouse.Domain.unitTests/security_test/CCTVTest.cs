using Xunit;
using BlaisePascal.SmartHouse.Domain;

namespace Blaisepascal.Smarthouse.Domain.unitTests.security_test
{
    public class CCTVTest
    {
        // The test verifies that the camera starts recording and status becomes ON
        [Fact]
        public void CameraStartsRecording()
        {
            // Arrange
            var camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);

            // Act
            camera.StartRecording();

            // Assert
            Assert.True(camera.Status);
        }

        // The test verifies that the camera can start and then stop recording
        [Fact]
        public void CameraStopsRecording()
        {
            // Arrange
            var camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);

            // Act
            camera.StartRecording();
            camera.StopRecording();

            // Assert
            Assert.False(camera.Status);
            Assert.Equal(0, camera.ZoomLevel);
        }

        // The test verifies that zoom works correctly within the defined limits
        [Fact]
        public void CameraZoomsWithinLimits()
        {
            // Arrange
            var camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);
            camera.StartRecording();

            // Act
            camera.Zoom(5);
            int currentZoomLevel = camera.ZoomLevel;

            // Assert
            Assert.Equal(5, currentZoomLevel);
        }

        // The test verifies that the camera does not zoom outside its limits
        [Fact]
        public void CameraDoesNotZoomOutsideLimits()
        {
            // Arrange
            var camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);
            camera.StartRecording();
            int previousZoom = camera.ZoomLevel;

            // Act
            camera.Zoom(15);
            int currentZoomLevel = camera.ZoomLevel;

            // Assert
            Assert.Equal(previousZoom, currentZoomLevel);
            Assert.NotEqual(15, currentZoomLevel);
        }

        // The test verifies that zoom does nothing if the camera is not recording
        [Fact]
        public void CameraZoomWithoutRecordingDoesNothing()
        {
            // Arrange
            var camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);

            // Act
            camera.Zoom(5);
            int currentZoomLevel = camera.ZoomLevel;

            // Assert
            Assert.Equal(0, currentZoomLevel);
        }

        // The test verifies that night vision toggles correctly ON and OFF
        [Fact]
        public void CameraTogglesNightVision()
        {
            // Arrange
            var camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);

            // Act
            camera.ToggleNightVision();
            bool firstState = camera.IsNightVisionOn;
            camera.ToggleNightVision();
            bool secondState = camera.IsNightVisionOn;

            // Assert
            Assert.True(firstState);
            Assert.False(secondState);
        }
    }
}
