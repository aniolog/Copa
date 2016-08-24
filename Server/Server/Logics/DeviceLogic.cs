using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Logics
{
    public class DeviceLogic:BaseLogic
    {
        Persistences.CrewMemberPersistence CrewMemberPersistence;
        Persistences.LogisticsDelegatePersistence LogisticsDelegatePersistence;
        Persistences.DevicePersistence DevicePersistence;

        public DeviceLogic():base()
        {

        }


        public void AddDeviceToCrewMember(Device Device, int CrewMemberId) {
            this.CrewMemberPersistence = new Persistences.CrewMemberPersistence(this.CurrentContext);
            CrewMember _crewMember = this.CrewMemberPersistence.FindById(CrewMemberId);
            this.AddDeviceToUser(_crewMember,Device);
        }

        public void DeleteDeviceFromCrewMember(String DeviceToken, int CrewMemberId) {
            this.CrewMemberPersistence = new Persistences.CrewMemberPersistence(this.CurrentContext);
            CrewMember _crewMember = this.CrewMemberPersistence.FindById(CrewMemberId);
            this.DeleteDeviceFromUser(_crewMember,DeviceToken);
        }


        public void AddDeviceToLogisticDelegate(Device Device, int LogisticDelegareId)
        {
            this.LogisticsDelegatePersistence = new Persistences.LogisticsDelegatePersistence(this.CurrentContext);
            LogisticsDelegate _logisticDelegate = 
                this.LogisticsDelegatePersistence.FindById(LogisticDelegareId);
            this.AddDeviceToUser(_logisticDelegate, Device);
        }

        public void DeleteDeviceFromLogisticDelegate(String DeviceToken, int LogisticDelegareId)
        {
            this.LogisticsDelegatePersistence = new Persistences.LogisticsDelegatePersistence(this.CurrentContext);
            LogisticsDelegate _logisticDelegate =
                this.LogisticsDelegatePersistence.FindById(LogisticDelegareId);
            this.DeleteDeviceFromUser(_logisticDelegate, DeviceToken);
        }



        private void AddDeviceToUser(User User, Device Device){
            if (String.IsNullOrEmpty(Device.Token)) {
                throw new Exception("Invalid Token");
            }
            this.DevicePersistence = new Persistences.DevicePersistence(this.CurrentContext);
            Device _device = this.DevicePersistence.FindByToken(Device.Token);
            Device = (_device == null) ? Device : _device;
            this.DevicePersistence.AddDeviceToUser(Device, User);
            
        }

        private void DeleteDeviceFromUser(User User, String DeviceToken)
        {
            if (String.IsNullOrEmpty(DeviceToken))
            {
                throw new Exception("Invalid Token");
            }
            this.DevicePersistence = new Persistences.DevicePersistence(this.CurrentContext);
            Device _device = this.DevicePersistence.FindByToken(DeviceToken);
            if (_device == null) {
                throw new Exception("No existe");
            }
            this.DevicePersistence.RemoveDeviceFromUser(_device, User);

        }

    }
}