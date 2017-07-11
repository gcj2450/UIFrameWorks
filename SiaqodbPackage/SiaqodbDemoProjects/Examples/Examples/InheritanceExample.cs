using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo;
using System.IO;

namespace SiaqodbExamples
{
    class InheritanceExample : BaseExample, IExample
    {

        public void Run()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            
            Insert(siaqodb);

            Load(siaqodb);
            
           
            
        }

        private void Insert(Siaqodb siaqodb)
        {
            Company company = CreateCompany();

            siaqodb.StoreObject(company);

            //Save into DB some objects with random data
            for (int i = 0; i < 10; i++)
            {
                Developer developer = CreateDeveloper(i, company.OID);

                siaqodb.StoreObject(developer);
            }
        }
        private void Load(Siaqodb siaqodb)
        {
            //load all employees that develop in C#
            var query = from Developer dev in siaqodb
                        where dev.DevelopIn == ProgrammingLanguage.CSharp
                        select dev;

            Log("Following developers work in CSharp:");
            foreach (Developer d in query)
            {
                Log(d.FirstName + " " + d.LastName);
            }
        }
        private Company CreateCompany()
        {
            Company company = new Company();
			company.Name = "MyCompany";
			company.Phone = "234-234-234";
			company.Address = "Street Example, nr.10";
            return company;
        }
        private Developer CreateDeveloper(int i,int companyOID)
        {
            Developer developer = new Developer();
            developer.CompanyOID = companyOID;
            if ((i % 3) == 0)
            {
                developer.DevelopIn = ProgrammingLanguage.CSharp;
            }
            else
            {
                developer.DevelopIn = ProgrammingLanguage.Java;
            }
            developer.FirstName = "Developer" + i.ToString();
            developer.LastName = "DeveloperLastName" + i.ToString();
            developer.HireDate = DateTime.Now.AddDays(-10);
            developer.YearsExperience = i % 6;

            return developer;
        }
       
    }
}
