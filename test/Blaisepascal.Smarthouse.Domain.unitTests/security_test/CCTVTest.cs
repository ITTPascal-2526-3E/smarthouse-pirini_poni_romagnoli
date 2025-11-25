namespace Blaisepascal.Smarthouse.Domain.unitTests.security_test;
public class CCTVTest
{
    [Fact]
    public void cameraStartsRecording()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam");
        camera.StartRecording();
        Assert.True(camera.IsRecording);
    }
    [Fact]
    public void cameraStopsRecording()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam");
        camera.StartRecording();
        camera.StopRecording();
        Assert.False(camera.IsRecording);
    }
    [Fact]
    public void cameraZoomsWithinLimits()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam");
        camera.StartRecording();
        camera.zoom(5);
        int currentZoomLevel = camera.zoomLevel;
        Assert.Equal(5, currentZoomLevel);
    }
    [Fact]
    public void cameraDoesNotZoomOutsideLimits()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam");
        camera.StartRecording();
        camera.zoom(15);
        int currentZoomLevel = camera.zoomLevel;
        Assert.NotEqual(15, currentZoomLevel);
    }
    [Fact]
    public void cameraZoomsOuRecordingDoesNothing()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam");
        camera.zoom(5);
        int currentZoomLevel = camera.zoomLevel;
        Assert.Equal(0, currentZoomLevel);
    }
    [Fact]
    public void cameraTogglesNightVision()
    {
        CCTV camera = new CCTV("ModelX", "BrandY", "1080p", 10, 1, "FrontDoorCam");
        camera.ToggleNightVision();
        Assert.True(camera.IsNightVisionOn);
        camera.ToggleNightVision();
        Assert.False(camera.IsNightVisionOn);
    }
}
