using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class VehiclePersistence
    {
        /// <summary>
        /// The Curent database context
        /// </summary>
        private static Models.Context CurrentContext = Models.Model.DataBase.GetInstance();

        public Vehicle FindByType(Models.Vehicle.VehicleType Type) {
            try
            {
                IQueryable<Vehicle> _selectedVehicle = from _vehicle in CurrentContext.Vehicles
                                                       where _vehicle.Type == Type
                                                       select _vehicle;
                return _selectedVehicle.First();

            }
            catch (Exception E) {
                return null;
            }
        }
    }
}