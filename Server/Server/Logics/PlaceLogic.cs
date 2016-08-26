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

        public void AddPlace(int MemberId, Place NewPlace) {
            CrewMember _currentMember = this.MemberPersistence.FindById(MemberId);
            if (NewPlace.Name == null) {
                throw new Exception("Invalid place name");
            }
            this.PlacePersistence.UpdateOrAddPlace(NewPlace,_currentMember);
        
        }

        public void DeletePlace(int PlaceId,int MemberId) {
            CrewMember _currentMember = this.MemberPersistence.FindById(MemberId);
            Place _deletedPlace = this.PlacePersistence.FindById(PlaceId);
            if (!(_currentMember.Places.Contains(_deletedPlace))) {
                throw new Exception("CrewMemberDoesNotOwnPlace");
            }
            this.PlacePersistence.DeletePlace(_deletedPlace);

        }

        public void UpdatePlace(Place UpdatedPlace, int MemberId) {
            CrewMember _currentMember = this.MemberPersistence.FindById(MemberId);
            Place _updatedPlace = this.PlacePersistence.FindById(UpdatedPlace.Id);
            if (UpdatedPlace.Name == null) {
                throw new Exception("Invalid place name");
            }
            _updatedPlace.Lat = UpdatedPlace.Lat;
            _updatedPlace.Long = UpdatedPlace.Long;
            _updatedPlace.Name = UpdatedPlace.Name;
            this.PlacePersistence.UpdateOrAddPlace(_updatedPlace,_currentMember);
        }

        public List<Models.Place> FindPlaces(int MemberId) {
            CrewMember _currentMember = this.MemberPersistence.FindById(MemberId);
            return _currentMember.Places.ToList();
        }
    }
}