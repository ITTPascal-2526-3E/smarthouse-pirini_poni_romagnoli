using System;
using BlaisePascal.SmartHouse.Domain.Food;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;

namespace BlaisePascal.Smarthouse.Application.Food.Repositories.Commands
{
    public class AddRefrigeratorCommand
    {
        private readonly IRefrigeratorRepository _repository;

        public AddRefrigeratorCommand(IRefrigeratorRepository repository)
        {
            _repository = repository;
        }

        public void Execute(Fridge fridge, string name, Freezer freezer)
        {
            var refrigerator = new Refrigerator(fridge, name, freezer);
            _repository.Add(refrigerator);
        }
    }
}
