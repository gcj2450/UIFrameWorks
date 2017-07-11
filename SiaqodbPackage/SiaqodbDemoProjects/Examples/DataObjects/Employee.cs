using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo;

namespace SiaqodbExamples
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HireDate { get; set; }
        public int OID { get; set; }
        public int CompanyOID { get; set; }
        public int Age { get; set; }
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
