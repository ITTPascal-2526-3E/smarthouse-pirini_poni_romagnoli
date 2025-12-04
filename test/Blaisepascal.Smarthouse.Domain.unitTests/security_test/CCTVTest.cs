namespace Blaisepascal.Smarthouse.Domain.unitTests.security_test;
public class CCTVTest
{
    //Testing if the camera starts recording
    [Fact]
    public void cameraStartsRecording()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);
        camera.StartRecording();
        Assert.True(camera.Status);
    }
    //Testing if the camera starts recording and then it stops
    [Fact]
    public void cameraStopsRecording()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);
        camera.StartRecording();
        camera.StopRecording();
        Assert.False(camera.Status);
    }
    //Testing if the zoom function works correctly and within the limits
    [Fact]
    public void cameraZoomsWithinLimits()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);
        camera.StartRecording();
        camera.zoom(5);
        int currentZoomLevel = camera.zoomLevel;
        Assert.Equal(5, currentZoomLevel);
    }
    //Testing that that the camera does NOT zoom outside of its limits
    [Fact]
    public void cameraDoesNotZoomOutsideLimits()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);
        camera.StartRecording();
        camera.zoom(15);
        int currentZoomLevel = camera.zoomLevel;
        Assert.NotEqual(15, currentZoomLevel);
    }
    //Testing if the camera does NOT zoom if not recording
    [Fact]
    public void cameraZoomsOuRecordingDoesNothing()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);
        camera.zoom(5);
        int currentZoomLevel = camera.zoomLevel;
        Assert.Equal(0, currentZoomLevel);
    }
    //Testing the toggle vision of the camera
    [Fact]
    public void cameraTogglesNightVision()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam", false);
        camera.ToggleNightVision();
        Assert.True(camera.IsNightVisionOn);
        camera.ToggleNightVision();
        Assert.False(camera.IsNightVisionOn);
    }
}
