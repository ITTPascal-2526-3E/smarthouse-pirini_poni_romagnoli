using System;
using BlaisePascal.SmartHouse.Domain.Food;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;
using BlaisePascal.SmartHouse.Domain.Illumination.LampOptions;

namespace BlaisePascal.Smarthouse.Application.Food.Repositories.Commands
{
    public class AddCoffeeMachineCommand
    {
        private readonly ICoffeeMachineRepository _repository;

        public AddCoffeeMachineCommand(ICoffeeMachineRepository repository)
        {
            _repository = repository;
        }

        public void Execute(string name, string brand, string model, EnergyClass energyClass, bool isConnected)
        {
            var machine = new CoffeeMachine(name, brand, model, energyClass, isConnected);
            _repository.Add(machine);
        }
    }
}
