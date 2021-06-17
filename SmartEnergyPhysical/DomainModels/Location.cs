using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Physical.DomainModels
{
    public class Location
    {
        public int ID { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int Number { get; set; }
        public string Zip { get; set; }
        public int MorningPriority { get; set; }
        public int NoonPriority { get; set; }
        public int NightPriority { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude{ get; set; }
        public List<Device> Devices { get; set; }
    }
}
