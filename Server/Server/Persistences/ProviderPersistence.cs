using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Persistences
{
    public class ProviderPersistence
    {
         /// <summary>
        /// The Curent database context
        /// </summary>
        private Models.Context CurrentContext;

        public ProviderPersistence(Models.Context CurrentContext)
        {
            this.CurrentContext = CurrentContext;
        }

        public Models.Provider AddOrUpdateProvider(Models.Provider NewProvider){

            try
            {
                if (NewProvider.Id == 0)
                {
                    CurrentContext.Providers.Add(NewProvider);
                    CurrentContext.SaveChanges();
                    CurrentContext.Entry(NewProvider).GetDatabaseValues();
                }
                else
                {
                    CurrentContext.SaveChanges();

                }
                return NewProvider;
            }
            catch (Exception E) {
                return null;
            }

        }

        public IQueryable<Models.Provider> FindProviders(){
            return CurrentContext.Providers;
        }


        public Models.Provider FindById(long Id){
            return CurrentContext.Providers.Find(Id);
        
        }

        public Models.Provider FindByName(String ProviderName) {

            try
            {
                IQueryable<Models.Provider> _selectedProvider = from _provider in CurrentContext.Providers
                                                                where _provider.Name == ProviderName
                                                                select _provider;
                return _selectedProvider.First();
            }
            catch (Exception E)
            {
                return null;
            }
        
        }
    }
}