using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Logics
{
    public class ProviderLogic:BaseLogic
    {

        private Persistences.ProviderPersistence ProviderPersistence;
        private Persistences.VehiclePersistence VehiclePersistence;

        public ProviderLogic():base()
        {
            this.ProviderPersistence = new Persistences.ProviderPersistence(this.CurrentContext);
            this.VehiclePersistence = new Persistences.VehiclePersistence(this.CurrentContext);
        }

        public void AddProvider(Models.Provider NewProvider) {

            Models.Provider _provider = 
                this.ProviderPersistence.FindByName(NewProvider.Name);
            if (_provider != null) {
                throw new Exception("existe");
            }
            List<Vehicle> _vehicles = new List<Vehicle>();
            foreach(Vehicle _loopVehicle in NewProvider.Vehicles){

                Vehicle _selectedVehicle = 
                    this.VehiclePersistence.FindByType(_loopVehicle.Type);
                _vehicles.Add((_selectedVehicle==null)?_loopVehicle:_selectedVehicle);
            
            }
            NewProvider.Vehicles = _vehicles;
            this.ProviderPersistence.AddOrUpdateProvider(NewProvider);
        
        }

        public void UpdateProvider(long Id,Models.Provider ExistingProvider)
        {

            Models.Provider _provider =
                this.ProviderPersistence.FindById(Id);
            if (_provider == null)
            {
                throw new Exception("existe");
            }
            _provider.Name = ExistingProvider.Name;
            _provider.Telephone = ExistingProvider.Telephone;
            _provider.ContactName = ExistingProvider.ContactName;
            List<Vehicle> _vehicles = new List<Vehicle>();
            foreach (Vehicle _loopVehicle in ExistingProvider.Vehicles)
            {

                Vehicle _selectedVehicle =
                    this.VehiclePersistence.FindByType(_loopVehicle.Type);
                _vehicles.Add((_selectedVehicle == null) ? _loopVehicle : _selectedVehicle);

            }
            _provider.Vehicles = _vehicles;
            this.ProviderPersistence.AddOrUpdateProvider(_provider);

        }

        public Models.Provider FindProvider(long Id) {
            return this.ProviderPersistence.FindById(Id);
        }

        public IQueryable<Models.Provider> GetProviders() {

            return this.ProviderPersistence.FindProviders();
        }


    }
}