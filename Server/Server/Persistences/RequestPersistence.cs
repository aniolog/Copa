using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class RequestPersistence:BasePersistence
    {
    

        public RequestPersistence(Models.Context CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }



        public Models.Request FindById(long RequestId) {

            try
            {
               return this.CurrentContext.Requests.Find(RequestId);
            }
            catch (Exception E) {
                throw new Exceptions.RequestNotFoundException();
            }
        
        }


        public Models.Request AddOrUpdateRequest(Models.Request Request)
        {
            if (Request.Id == 0)
            {
                CurrentContext.Requests.Add(Request);
                CurrentContext.SaveChanges();
                CurrentContext.Entry(Request).GetDatabaseValues();
            }
            else
            {
                CurrentContext.SaveChanges();


            }
            return Request;

        }

    }
}