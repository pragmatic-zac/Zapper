using System;
using System.Collections.Generic;
using System.Text;

namespace Zapper.Models
{
    public class Airline
    {
        public Airline()
        {
            Employees = new List<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateFounded { get; set; }
        public int TotalEmployees { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
