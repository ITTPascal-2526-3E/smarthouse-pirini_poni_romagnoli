using System;
using BlaisePascal.SmartHouse.Domain.Food;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;

namespace BlaisePascal.Smarthouse.Application.Food.Repositories.Commands
{
    public class UpdateRefrigeratorCommand
    {
        private readonly IRefrigeratorRepository _repository;

        public UpdateRefrigeratorCommand(IRefrigeratorRepository repository)
        {
            _repository = repository;
        }

        public void Execute(Refrigerator refrigerator)
        {
            _repository.Update(refrigerator);
        }
    }
}
