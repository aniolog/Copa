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
        private static Models.Context CurrentContext = Models.Model.DataBase.GetInstance();

        public void UpdateOrAddPlace(Place NewPlace,CrewMember Member) {
            if (NewPlace.Id == 0)
            {
                Member.Places.Add(NewPlace);
            }
                CurrentContext.SaveChanges();
       

        }

        public void DeletePlace(Place DeletedPlace) {
            if (CurrentContext.Places.Contains(DeletedPlace)) {
                CurrentContext.Places.Remove(DeletedPlace);
            }
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