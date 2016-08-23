using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class PlacePersistence
    {
        /// <summary>
        /// The Curent database context
        /// </summary>
        private  Models.Context CurrentContext;

        public PlacePersistence(Models.Context CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }


        public void UpdateOrAddPlace(Place NewPlace,CrewMember Member) {
            if (NewPlace.Id == 0)
            {
                Member.Places.Add(NewPlace);
            }
                CurrentContext.SaveChanges();
       

        }

        public void DeletePlace(Place DeletedPlace) {
                Place _deletedPlace= CurrentContext.Places.Find(DeletedPlace.Id);
                CurrentContext.Places.Remove(_deletedPlace);
                CurrentContext.SaveChanges();
            
        }

        public Place FindById(long PlaceId) {
            try
            {
                return CurrentContext.Places.Find(PlaceId);
            }
            catch (Exception E) {
                return null;
            }
        }

        public IQueryable<Place> FindAll() {
            try
            {
                return CurrentContext.Places;
            }
            catch (Exception E) {
                return null;
            }
        }







    }
}