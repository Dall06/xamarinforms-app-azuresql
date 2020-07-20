using System;
using System.Collections.Generic;
using System.Text;

namespace AppAzureSQL.Models
{
    public class DriverModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Picture { get; set; }
        public PositionModel Position { get; set; }
    }
}
