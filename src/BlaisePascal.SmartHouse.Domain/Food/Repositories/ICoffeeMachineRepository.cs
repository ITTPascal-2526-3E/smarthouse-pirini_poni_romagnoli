using System;
using System.Collections.Generic;

namespace BlaisePascal.SmartHouse.Domain.Food.Repositories
{
    public interface ICoffeeMachineRepository
    {
        void Add(CoffeeMachine coffeeMachine);
        void Update(CoffeeMachine coffeeMachine);
        void Remove(Guid id);
        CoffeeMachine? GetById(Guid id);
        List<CoffeeMachine> GetAll();
    }
}
