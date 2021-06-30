using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericRepository.Interfaces;

namespace Fanword.Data.Entities
{
    public class EventManagementPin : ISaveable<string>, ISaveableCreate
    {
        public string Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public bool IsActive { get; set; }
        public string PinNumber { get; set; }
    }
}
