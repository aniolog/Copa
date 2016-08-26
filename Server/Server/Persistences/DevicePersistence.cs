using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class DevicePersistence:BasePersistence
    {

        public DevicePersistence(Models.Context CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }


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
                throw new Exception("Device not found");

            }
        }

        public void AddDeviceToUser(Device NewDevice, User User) {
                User.Device = NewDevice;
                CurrentContext.SaveChanges();
        }



        public void RemoveDeviceFromUser(Device Device, User User)
        {
            User.Device = null;
            CurrentContext.Devices.Remove(Device);
            CurrentContext.SaveChanges();
        }

    }
}