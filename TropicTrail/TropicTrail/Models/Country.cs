using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TropicTrail.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string state { get; set; }
        public ICollection<City> states { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        public string city { get; set; }
        public string stateName { get; set; }
        public Province provine { get; set; }
        public ICollection<Street> streets { get; set; }
    }
    public class Streets
    {
        public int Id { get; set; }
        public string street { get; set; }
        public string cityName { get; set; }
        public City city { get; set; }
    }
}