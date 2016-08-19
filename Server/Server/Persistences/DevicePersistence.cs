using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class DevicePersistence
    {
        private static Models.Context CurrentContext = Models.Model.DataBase.GetInstance();



        public Device FindByToken(String Token)
        {
            try
            {
                IQueryable<Device> _devices = from _device in CurrentContext.Devices
                                              where _device.Token == Token
                                              select _device;
                return _devices.First();
            }
            catch(System.InvalidOperationException)
            {
                return null;

            }
        }

        public void AddDeviceToUser(Device NewDevice, User User) {

            if (!(User.Devices.Contains(NewDevice))) {
                User.Devices.Add(NewDevice);
                CurrentContext.SaveChanges();
            }
        }

        public void RemoveDeviceFromUser(Device Device, User User)
        {

            if ((User.Devices.Contains(Device)))
            {
                User.Devices.Remove(Device);
            }
            if (Device.LogisticsDelegates.Count == 0) {
                if (Device.CrewMembers.Count == 0) {
                    CurrentContext.Devices.Remove(Device);
                }
            }
            CurrentContext.SaveChanges();
        }

    }
}