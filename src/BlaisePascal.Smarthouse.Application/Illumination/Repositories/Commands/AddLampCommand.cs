using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.SmartHouse.Domain.Illumination.Repositories;
using BlaisePascal.SmartHouse.Domain.Illumination;
using BlaisePascal.SmartHouse.Domain.ValueObjects;
using BlaisePascal.SmartHouse.Domain.Illumination.LampTypes;



namespace BlaisePascal.SmartHouse.Application.Illumination.Repositories.Commands
{
    public class AddLampCommand
    {
        private readonly ILampRepository _lampRepository;

        public AddLampCommand(ILampRepository lampRepository)
        {
            _lampRepository = lampRepository;
        }

        public void Execute(string name)// L'utente mi passa il nome 
        {
            var lamp = new Lamp(new DeviceName(name)); // Creo una nuova lampada con il nome passato dall'utente
            _lampRepository.Add(lamp); // Aggiungo la lampada al repository

        }



    }
}