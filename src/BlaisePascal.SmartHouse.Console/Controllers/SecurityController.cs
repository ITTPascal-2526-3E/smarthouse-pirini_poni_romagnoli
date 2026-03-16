using System;
using System.Collections.Generic;
using System.Linq;

using BlaisePascal.SmartHouse.Domain.Security.SecurityDevices;

using BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Security;

using BlaisePascal.SmartHouse.Application.Security.Commands;
using BlaisePascal.SmartHouse.Application.Security.Queries;

namespace BlaisePascal.SmartHouse.Console.Controllers
{
    internal sealed class SecurityController
    {
        private readonly InMemoryDoorRepository _doorRepo;
        private readonly InMemoryCCTVRepository _cctvRepo;
        private readonly InMemoryAlarmSystemRepository _alarmRepo;

        // Queries
        private readonly GetAllCCTVQuery _getAllCctv;
        private readonly GetCCTVByIDQuery _getCctvById;
        private readonly GetCCTVIsNightVisionOnQuery _getCctvNightVision;
        private readonly GetCCTVWideAngleLevelQuery _getCctvWideAngle;
        private readonly GetZoomLevelQuery _getCctvZoom;

        // Commands
        private readonly AddCCTVCommand _addCctv;
        private readonly RemoveCCTVCommand _removeCctv;
        private readonly UpdateCCTVCommand _updateCctv;
        private readonly StartRecordingCCTVCommand _startRecording;
        private readonly StopRecordingCCTVCommand _stopRecording;
        private readonly TriggerAlarmCCTVCommand _triggerAlarmCctv;
        private readonly GetCCTVStatusQuery _displayCctvStatus;

        public SecurityController()
        {
            _doorRepo = new InMemoryDoorRepository();
            _cctvRepo = new InMemoryCCTVRepository();
            _alarmRepo = new InMemoryAlarmSystemRepository();

            _getAllCctv = new GetAllCCTVQuery(_cctvRepo);
            _getCctvById = new GetCCTVByIDQuery(_cctvRepo);
            _getCctvNightVision = new GetCCTVIsNightVisionOnQuery(_cctvRepo);
            _getCctvWideAngle = new GetCCTVWideAngleLevelQuery(_cctvRepo);
            _getCctvZoom = new GetZoomLevelQuery(_cctvRepo);

            _addCctv = new AddCCTVCommand(_cctvRepo);
            _removeCctv = new RemoveCCTVCommand(_cctvRepo);
            _updateCctv = new UpdateCCTVCommand(_cctvRepo);
            _startRecording = new StartRecordingCCTVCommand(_cctvRepo);
            _stopRecording = new StopRecordingCCTVCommand(_cctvRepo);
            _triggerAlarmCctv = new TriggerAlarmCCTVCommand(_cctvRepo);
            _displayCctvStatus = new GetCCTVStatusQuery(_cctvRepo);
        }

        public void InitData()
        {
            _doorRepo.Add(new Door("Front Door", false));
            _doorRepo.Add(new Door("Back Door", false));

            _addCctv.Execute("Cam-Front", "Sony", "4K", 10, 1, "Front Garden Camera", false);

            var alarm = new AlarmSystem("Verisure", "V-Pro");
            _alarmRepo.Add(alarm);

            // Security alarm subscriptions
            Action<string, string> handler = (name, msg) =>
            {
                System.Console.WriteLine($"\n[ALARM] {name}: {msg}");
                System.Console.Write("Press any key...");
                System.Console.ReadKey(true);
            };
            var cctv = _getAllCctv.Execute().First();
            cctv.OnAlarm += handler;
            alarm.OnAlarm += handler;
            foreach (var d in _doorRepo.GetAll()) d.OnAlarm += handler;
        }

        public List<Door> GetAllDoors()
        {
            return _doorRepo.GetAll();
        }

        public AlarmSystem GetAlarm()
        {
            return _alarmRepo.GetAll().First();
        }

        public CCTV GetCamera()
        {
            return _getAllCctv.Execute().First();
        }

        public void ShowMenu()
        {
            bool stay = true;
            while (stay)
            {
                System.Console.Clear();
                System.Console.WriteLine("SECURITY");
                System.Console.WriteLine("");

                var doors = _doorRepo.GetAll();
                var cameras = _getAllCctv.Execute().ToList();
                var alarm = _alarmRepo.GetAll().First();

                System.Console.WriteLine("Doors:");
                for (int i = 0; i < doors.Count; i++)
                {
                    var d = doors[i];
                    string locked = d.IsLocked ? "LOCKED" : "UNLOCKED";
                    string state = d.Status ? "OPEN" : "CLOSED";
                    System.Console.WriteLine($"{i + 1}. {d.Name} [{locked}] ({state})");
                }
                System.Console.WriteLine("");
                System.Console.WriteLine("Cameras:");
                for (int i = 0; i < cameras.Count; i++)
                {
                    var c = cameras[i];
                    System.Console.WriteLine($"{i + 1}. {c.Name} " + (c.IsRecording ? "[RECORDING]" : "[IDLE]"));
                }
                System.Console.WriteLine($"Alarm: {(alarm.IsArmed ? "ARMED" : "DISARMED")}");
                System.Console.WriteLine("");
                System.Console.WriteLine("[D] Toggle door (open/close)");
                System.Console.WriteLine("[K] Lock/Unlock door");
                System.Console.WriteLine("[C] Toggle CCTV recording");
                System.Console.WriteLine("[V] Toggle Night Vision");
                System.Console.WriteLine("[A] Arm/Disarm Alarm");
                System.Console.WriteLine("[!] Trigger Alarm");
                System.Console.WriteLine("[N] Add new CCTV");
                System.Console.WriteLine("[R] Remove CCTV");
                System.Console.WriteLine("[I] Inspect CCTV (status + zoom + wide angle + night vision)");
                System.Console.WriteLine("[X] Trigger CCTV Alarm");
                System.Console.WriteLine("[B] Back");
                System.Console.Write("> ");

                switch (System.Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D:
                        System.Console.Write("Door #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int di) && di >= 1 && di <= doors.Count)
                        {
                            var door = doors[di - 1];
                            if (door.Status) door.CloseDoor(); else door.OpenDoor();
                            _doorRepo.Update(door);
                        }
                        break;
                    case ConsoleKey.K:
                        System.Console.Write("Door #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ki) && ki >= 1 && ki <= doors.Count)
                        {
                            var door = doors[ki - 1];
                            if (door.IsLocked) door.UnlockDoor(); else door.LockDoor();
                            _doorRepo.Update(door);
                        }
                        break;
                    case ConsoleKey.C:
                        System.Console.Write("Camera #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ci) && ci >= 1 && ci <= cameras.Count)
                        {
                            var camera = cameras[ci - 1];
                            if (camera.IsRecording)
                                _stopRecording.Execute(camera.DeviceId);
                            else
                                _startRecording.Execute(camera.DeviceId);
                        }
                        break;
                    case ConsoleKey.V:
                        System.Console.Write("Camera #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int vi) && vi >= 1 && vi <= cameras.Count)
                        {
                            var camera = cameras[vi - 1];
                            camera.ToggleNightVision();
                            _updateCctv.Execute(camera.DeviceId);
                        }
                        break;
                    case ConsoleKey.A:
                        if (alarm.IsArmed) alarm.Disarm(); else alarm.Arm();
                        _alarmRepo.Update(alarm);
                        break;
                    case ConsoleKey.D1: // '!'
                        if (alarm.IsArmed) alarm.TriggerAlarm();
                        break;
                    case ConsoleKey.N:
                        System.Console.Write("Name: ");
                        string? cName = System.Console.ReadLine();
                        System.Console.Write("Brand: ");
                        string? cBrand = System.Console.ReadLine();
                        System.Console.Write("Model: ");
                        string? cModel = System.Console.ReadLine();
                        System.Console.Write("Resolution (e.g. 4K): ");
                        string? cRes = System.Console.ReadLine();
                        System.Console.Write("Telephoto Level (>= 1): ");
                        int.TryParse(System.Console.ReadLine(), out int tele);
                        System.Console.Write("Wide Angle Level (>= 1): ");
                        int.TryParse(System.Console.ReadLine(), out int wide);
                        if (!string.IsNullOrWhiteSpace(cName) && !string.IsNullOrWhiteSpace(cBrand) && !string.IsNullOrWhiteSpace(cModel)
                            && !string.IsNullOrWhiteSpace(cRes) && tele >= 1 && wide >= 1)
                        {
                            if (tele <= wide)
                            {
                                System.Console.WriteLine("Invalid zoom levels. Telephoto level must be greater than Wide Angle level.");
                                System.Console.Write("Press any key...");
                                System.Console.ReadKey(true);
                                break;
                            }
                            _addCctv.Execute(cModel, cBrand, cRes, tele, wide, cName, false);
                            System.Console.WriteLine("CCTV added!");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        else
                        {
                            System.Console.WriteLine("Invalid input. Fields cannot be empty, levels must be >= 1.");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.R:
                        System.Console.Write("Camera # to remove: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ri) && ri >= 1 && ri <= cameras.Count)
                        {
                            _removeCctv.Execute(cameras[ri - 1].DeviceId);
                            System.Console.WriteLine("CCTV removed!");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.I:
                        System.Console.Write("Camera #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int ii) && ii >= 1 && ii <= cameras.Count)
                        {
                            var cam = cameras[ii - 1];
                            // GetCCTVStatusQuery
                            string statusStr = _displayCctvStatus.Execute(cam.DeviceId);
                            System.Console.WriteLine(statusStr);
                            // GetCCTVIsNightVisionOnQuery
                            bool nightVision = _getCctvNightVision.Execute(cam.DeviceId);
                            System.Console.WriteLine($"Night Vision: {(nightVision ? "ON" : "OFF")}");
                            // GetCCTVZoomLevelQuery
                            int zoom = _getCctvZoom.Execute(cam.DeviceId);
                            System.Console.WriteLine($"Zoom Level: {zoom}");
                            // GetCCTVWideAngleLevelQuery
                            int wideAngle = _getCctvWideAngle.Execute(cam.DeviceId);
                            System.Console.WriteLine($"Wide Angle Level: {wideAngle}");
                            // GetCCTVByIDQuery
                            var camById = _getCctvById.Execute(cam.DeviceId);
                            System.Console.WriteLine($"ID: {camById?.DeviceId}");
                            System.Console.WriteLine("");
                            System.Console.Write("Press any key...");
                            System.Console.ReadKey(true);
                        }
                        break;
                    case ConsoleKey.X:
                        System.Console.Write("Camera #: ");
                        if (int.TryParse(System.Console.ReadLine(), out int xi) && xi >= 1 && xi <= cameras.Count)
                        {
                            _triggerAlarmCctv.Execute(cameras[xi - 1].DeviceId);
                        }
                        break;
                    case ConsoleKey.B: stay = false; break;
                }
            }
        }
    }
}

