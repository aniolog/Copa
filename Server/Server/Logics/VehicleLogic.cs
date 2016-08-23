using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Logics
{
    public class VehicleLogic
    {

        public List<Models.Vehicle.VehicleType> GetAllTypes() {

            return Enum.GetValues(typeof(Models.Vehicle.VehicleType)).Cast<Models.Vehicle.VehicleType>().ToList();
        }
    }
}