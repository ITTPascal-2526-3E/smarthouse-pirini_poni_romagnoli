using System;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;

namespace BlaisePascal.Smarthouse.Application.Food.Commands
{
    public class RemoveRefrigeratorCommand
    {
        private readonly IRefrigeratorRepository _repository;

        public RemoveRefrigeratorCommand(IRefrigeratorRepository repository)
        {
            _repository = repository;
        }

        public void Execute(Guid id)
        {
            _repository.Remove(id);
        }
    }
}

