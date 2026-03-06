using System;
using System.Collections.Generic;
using System.Linq;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;
using BlaisePascal.SmartHouse.Domain.Food;

namespace BlaisePascal.SmartHouse.Infrastructure.Repositories.Devices.Food
{
    public class InMemoryCoffeeMachineRepository : ICoffeeMachineRepository
    {
        private readonly List<CoffeeMachine> _machines = new List<CoffeeMachine>();

        public void Add(CoffeeMachine machine)
        {
            _machines.Add(machine);
        }

        public void Remove(Guid id)
        {
            var machine = _machines.FirstOrDefault(m => m.DeviceId == id);
            if (machine != null)
            {
                _machines.Remove(machine);
            }
        }

        public void Update(CoffeeMachine machine)
        {
            var index = _machines.FindIndex(m => m.DeviceId == machine.DeviceId);
            if (index != -1)
            {
                _machines[index] = machine;
            }
        }

        public CoffeeMachine? GetById(Guid id)
        {
            return _machines.FirstOrDefault(m => m.DeviceId == id);
        }

        public List<CoffeeMachine> GetAll()
        {
            return _machines.ToList();
        }
    }
}
