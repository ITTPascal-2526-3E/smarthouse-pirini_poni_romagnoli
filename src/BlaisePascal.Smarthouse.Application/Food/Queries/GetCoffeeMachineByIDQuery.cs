using System;
using BlaisePascal.SmartHouse.Domain.Food;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;

namespace BlaisePascal.Smarthouse.Application.Food.Queries
{
    public class GetCoffeeMachineByIDQuery
    {
        private readonly ICoffeeMachineRepository _repository;

        public GetCoffeeMachineByIDQuery(ICoffeeMachineRepository repository)
        {
            _repository = repository;
        }

        public CoffeeMachine? Execute(Guid id)
        {
            return _repository.GetById(id);
        }
    }
}

