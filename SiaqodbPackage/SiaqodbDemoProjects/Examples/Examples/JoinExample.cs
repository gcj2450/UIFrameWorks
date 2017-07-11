using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sqo;

namespace SiaqodbExamples
{
    class JoinExample : BaseExample, IExample
    {
        public void Run()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            siaqodb.DropType<Employee>();
            siaqodb.DropType<Company>();

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
                Employee employee;
                if (i % 2 == 0)
                {
                     employee = CreateEmployee(i, company.OID);
                }
                else
                {
                    employee = CreateEmployee(i);
                }
                siaqodb.StoreObject(employee);


                
            }

            
        }
        private void Load(Siaqodb siaqodb)
        {
            //load all employees of company with name CompanyX
            // ! will not be created any Company objects also not any Employee objects and only needed values will be loaded from DB
            var query = from Company comp in siaqodb
                        where comp.Name=="CompanyX"
                        join Employee emp in siaqodb
                        on comp.OID equals emp.CompanyOID
                        select new {emp.FirstName ,emp.LastName };

            Log("Following employees works at CompanyX:");
            foreach (var e in query)
            {
                Log(e.FirstName + " " + e.LastName);
            }
        }
        private Company CreateCompany()
        {
            Company company = new Company();
            company.Name = "CompanyX";
            company.Phone = "233-204-235";
            company.Address = "Street of CompanyX, nr.1";
            return company;
        }
        private Employee CreateEmployee(int i, int companyOID)
        {
            Employee employee = new Employee();
            employee.CompanyOID = companyOID;

            employee.FirstName = "Employee" + i.ToString();
            employee.LastName = "EmployeeLastName" + i.ToString();
            employee.HireDate = DateTime.Now.AddDays(-100);

            return employee;
        }
        private Employee CreateEmployee(int i)
        {
            return CreateEmployee(i, -1);
        }
    }
}
