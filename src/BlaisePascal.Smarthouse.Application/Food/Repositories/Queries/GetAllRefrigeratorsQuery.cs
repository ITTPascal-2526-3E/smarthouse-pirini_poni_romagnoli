using System.Collections.Generic;
using BlaisePascal.SmartHouse.Domain.Food;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;

namespace BlaisePascal.Smarthouse.Application.Food.Repositories.Queries
{
    public class GetAllRefrigeratorsQuery
    {
        private readonly IRefrigeratorRepository _repository;

        public GetAllRefrigeratorsQuery(IRefrigeratorRepository repository)
        {
            _repository = repository;
        }

        public List<Refrigerator> Execute()
        {
            return _repository.GetAll();
        }
    }
}
