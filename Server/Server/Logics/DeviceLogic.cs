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
            try
            {
                this.CrewMemberPersistence = new Persistences.CrewMemberPersistence(this.CurrentContext);
                CrewMember _crewMember = this.CrewMemberPersistence.FindById(CrewMemberId);
                this.AddDeviceToUser(_crewMember, Device);
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

        public void DeleteDeviceFromCrewMember(String DeviceToken, int CrewMemberId) {
            try
            {
                this.CrewMemberPersistence = new Persistences.CrewMemberPersistence(this.CurrentContext);
                CrewMember _crewMember = this.CrewMemberPersistence.FindById(CrewMemberId);
                this.DeleteDeviceFromUser(_crewMember, DeviceToken);
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


        public void AddDeviceToLogisticDelegate(Device Device, int LogisticDelegareId)
        {
            try
            {
                this.LogisticsDelegatePersistence = new Persistences.LogisticsDelegatePersistence(this.CurrentContext);
                LogisticsDelegate _logisticDelegate =
                    this.LogisticsDelegatePersistence.FindById(LogisticDelegareId);
                this.AddDeviceToUser(_logisticDelegate, Device);
            }
            catch (Exception E) {
                throw E;
            }
        }

        public void DeleteDeviceFromLogisticDelegate(String DeviceToken, int LogisticDelegareId)
        {
            this.LogisticsDelegatePersistence = new Persistences.LogisticsDelegatePersistence(this.CurrentContext);
            LogisticsDelegate _logisticDelegate =
                this.LogisticsDelegatePersistence.FindById(LogisticDelegareId);
            this.DeleteDeviceFromUser(_logisticDelegate, DeviceToken);
        }



        private void AddDeviceToUser(User User, Device Device){
            try
            {
                if (String.IsNullOrEmpty(Device.Token))
                {
                    throw new Exceptions.InvalidTokenException();
                }
                this.DevicePersistence = new Persistences.DevicePersistence(this.CurrentContext);
                Device _device = this.DevicePersistence.FindByToken(Device.Token);
                Device = (_device == null) ? Device : _device;
                this.DevicePersistence.AddDeviceToUser(Device, User);
            }
            catch (Exception E) {
                throw E;
            }
            
        }

        private void DeleteDeviceFromUser(User User, String DeviceToken)
        {
            try
            {
                if (String.IsNullOrEmpty(DeviceToken))
                {
                    throw new Exceptions.InvalidTokenException();
                }
                this.DevicePersistence = new Persistences.DevicePersistence(this.CurrentContext);
                Device _device = this.DevicePersistence.FindByToken(DeviceToken);
                this.DevicePersistence.RemoveDeviceFromUser(_device, User);
            }
            catch (Exception E)
            {
                throw E;
            }

        }

    }
}