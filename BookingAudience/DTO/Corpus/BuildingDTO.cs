using BookingAudience.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.DTO.Corpus
{
    public class BuildingDTO
    {
        public BuildingDTO(Building building)
        {
            Id = building.Id;
            Address = building.Address;
            CodeLetter = building.CodeLetter;
            Title = building.Title;
        }

        public int Id { get; }
        /// <summary>
        /// наименование корпуса
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// адрес здания
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// кодовая буква, с помощью которой формируется номер кабинета (в А корпусе кабинеты аля А123)
        /// </summary>
        public char CodeLetter { get; set; }
    }
}
