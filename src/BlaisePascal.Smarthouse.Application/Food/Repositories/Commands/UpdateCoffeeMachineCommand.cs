using System;
using BlaisePascal.SmartHouse.Domain.Food;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;

namespace BlaisePascal.Smarthouse.Application.Food.Repositories.Commands
{
    public class UpdateCoffeeMachineCommand
    {
        private readonly ICoffeeMachineRepository _repository;

        public UpdateCoffeeMachineCommand(ICoffeeMachineRepository repository)
        {
            _repository = repository;
        }

        public void Execute(CoffeeMachine machine)
        {
            _repository.Update(machine);
        }
    }
}
