using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class PlacePersistence:BasePersistence
    {
   

        public PlacePersistence(Models.Context CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }


        public Models.Place UpdateOrAddPlace(Place NewPlace,CrewMember Member) {


            if (NewPlace.Id == 0)
            {
                Member.Places.Add(NewPlace);
                CurrentContext.SaveChanges();
               CurrentContext.Entry(NewPlace).GetDatabaseValues();
               return NewPlace;
            }
                CurrentContext.SaveChanges();
                return NewPlace;
       

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
                throw new Exceptions.PlaceNotFoundException();
            }
        }

        public IQueryable<Place> FindAll() {
            try
            {
                return CurrentContext.Places;
            }
            catch (Exception E) 
            {
                return null;
            }
        }







    }
}