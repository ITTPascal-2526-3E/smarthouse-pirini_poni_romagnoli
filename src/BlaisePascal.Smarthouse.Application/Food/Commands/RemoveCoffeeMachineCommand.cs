using System;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;

namespace BlaisePascal.Smarthouse.Application.Food.Commands
{
    public class RemoveCoffeeMachineCommand
    {
        private readonly ICoffeeMachineRepository _repository;

        public RemoveCoffeeMachineCommand(ICoffeeMachineRepository repository)
        {
            _repository = repository;
        }

        public void Execute(Guid id)
        {
            _repository.Remove(id);
        }
    }
}

