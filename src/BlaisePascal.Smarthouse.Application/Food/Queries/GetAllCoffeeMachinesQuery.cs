using System.Collections.Generic;
using BlaisePascal.SmartHouse.Domain.Food;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;

namespace BlaisePascal.Smarthouse.Application.Food.Queries
{
    public class GetAllCoffeeMachinesQuery
    {
        private readonly ICoffeeMachineRepository _repository;

        public GetAllCoffeeMachinesQuery(ICoffeeMachineRepository repository)
        {
            _repository = repository;
        }

        public List<CoffeeMachine> Execute()
        {
            return _repository.GetAll();
        }
    }
}

