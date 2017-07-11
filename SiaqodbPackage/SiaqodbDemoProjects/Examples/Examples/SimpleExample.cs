using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo;


namespace SiaqodbExamples
{
    class SimpleExample : BaseExample,IExample
    {
       
        public void Run()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            //clear all objects of Type Company created by previous run of this example
            siaqodb.DropType<Company>();

            Insert(siaqodb);

            LoadAndUpdate(siaqodb);

            Delete(siaqodb);

            
        }

       
        private void Insert(Siaqodb siaqodb)
        {
            Company company = CreateCompany("FirstCompany");
            siaqodb.StoreObject(company);

            Company company2 = CreateCompany("SecondCompany");
            siaqodb.StoreObject(company2);

            

        }
 
        private void LoadAndUpdate(Siaqodb siaqodb)
        {
            
            var query = from Company comp in siaqodb
                        where comp.Name.StartsWith("First")
                        select comp;

            Log("Following companies has name that starts with 'First' string");
            foreach (var e in query)
            {
                e.Address = "Address changed";
                siaqodb.StoreObject(e);//update will occur
            }
            siaqodb.Flush();
          
            //load all objects of type Company from DB
            IObjectList<Company> allCompanies = siaqodb.LoadAll<Company>();
            foreach (Company comp in allCompanies)
            {
                UnityEngine.Debug.Log(comp.Address);
                //see address of FirstCompany is updated
            }

        }
        private void Delete(Siaqodb siaqodb)
        {
            
            IObjectList<Company> allCompanies = siaqodb.LoadAll<Company>();
            
            //delete object in database( in memory will still be available until GC will take it)
            siaqodb.Delete(allCompanies[0]);


            int  count = siaqodb.Count<Company>();
            Log("actual number of companies after delete:"+count.ToString());
            
        }
        private Company CreateCompany(string companyName)
        {
            Company company = new Company();
            company.Name = companyName;
            company.Phone = "233-204-235";
            company.Address = "Street of SimpleCompany, nr.1";
            return company;
        }


       
    }
}
