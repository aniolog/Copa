using Server.Models;
using Server.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Logics
{
    public class PlaceLogic:BaseLogic
    {
        CrewMemberPersistence MemberPersistence;
        PlacePersistence PlacePersistence;

        public PlaceLogic():base()
        {
            this.PlacePersistence = new PlacePersistence(this.CurrentContext);
            this.MemberPersistence = new CrewMemberPersistence(this.CurrentContext);
        }

        public Models.Place AddPlace(int MemberId, Place NewPlace) {
            try
            {
                CrewMember _currentMember = this.MemberPersistence.FindById(MemberId);

                if (NewPlace.Name == null)
                {
                    throw new Exception("Invalid place name");
                }
               return this.PlacePersistence.UpdateOrAddPlace(NewPlace, _currentMember);
            }
            catch (Exceptions.CrewMemberNotFoundException E)
            {
                throw E;
            }
            catch (Exception E)
            {
                throw E;
            }
        
        }

        public void DeletePlace(int PlaceId,int MemberId) {
            try{
                CrewMember _currentMember = this.MemberPersistence.FindById(MemberId);
                Place _deletedPlace = this.PlacePersistence.FindById(PlaceId);
                if (!(_currentMember.Places.Contains(_deletedPlace))) {
                    throw new Exception("CrewMemberDoesNotOwnPlace");
                }
                this.PlacePersistence.DeletePlace(_deletedPlace);
            }

            catch (Exception E)
            {
                throw E;
            }

        }

        public void UpdatePlace(Place UpdatedPlace, int MemberId) {
            try
            {
                CrewMember _currentMember = this.MemberPersistence.FindById(MemberId);
                Place _updatedPlace = this.PlacePersistence.FindById(UpdatedPlace.Id);
                if (UpdatedPlace.Name == null)
                {
                    throw new Exception("Invalid place name");
                }
                _updatedPlace.Lat = UpdatedPlace.Lat;
                _updatedPlace.Long = UpdatedPlace.Long;
                _updatedPlace.Name = UpdatedPlace.Name;
                this.PlacePersistence.UpdateOrAddPlace(_updatedPlace, _currentMember);
            }
            catch (Exception E)
            {
                throw E;
            }
        }

        public List<Models.Place> FindPlaces(int MemberId) {
            try
            {
                this.CurrentContext.Configuration.ProxyCreationEnabled = false;
                CrewMember _currentMember = this.MemberPersistence.FindById(MemberId);
                this.CurrentContext.Entry(_currentMember)
                    .Collection(_crewMember => _crewMember.Places).Load();
                return (_currentMember.Places.Any()) ? 
                    _currentMember.Places.ToList() : null;
              
            }
            catch (Exception E)
            {
                throw E;
            }
        }
    }
}