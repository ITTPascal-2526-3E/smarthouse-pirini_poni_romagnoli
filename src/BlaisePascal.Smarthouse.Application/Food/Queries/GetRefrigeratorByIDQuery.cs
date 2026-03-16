using System;
using BlaisePascal.SmartHouse.Domain.Food;
using BlaisePascal.SmartHouse.Domain.Food.Repositories;

namespace BlaisePascal.Smarthouse.Application.Food.Queries
{
    public class GetRefrigeratorByIDQuery
    {
        private readonly IRefrigeratorRepository _repository;

        public GetRefrigeratorByIDQuery(IRefrigeratorRepository repository)
        {
            _repository = repository;
        }

        public Refrigerator? Execute(Guid id)
        {
            return _repository.GetById(id);
        }
    }
}

