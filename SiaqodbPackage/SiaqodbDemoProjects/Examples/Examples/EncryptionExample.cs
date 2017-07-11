using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo;
using System.IO;

namespace SiaqodbExamples
{
    class EncryptionExample : BaseExample, IExample
    {
      

        public void Run()
        {
            Log("Configure database to encrypt data...");
            //before open the database use SiaqodbConfigurator to encrypt database files
            SiaqodbConfigurator.EncryptedDatabase = true;
            
            this.EncryptSimple();
            this.EncryptUsingXTEA_And_CustomPassword();

            Log("Configure database to NOT encrypt data...");
            //Set back configurator to not use Encryption so next examples run process not encrypt data
            SiaqodbConfigurator.EncryptedDatabase = false;

        }
        public void EncryptSimple()
        {
           
            //now database files will be encrypted with default AES encryption algorithm and with built-in password 
            string siaqodbPathNew=SiaqodbFactoryExample.siaoqodbPath+Path.DirectorySeparatorChar + @"siaqodb";
			if (!Directory.Exists(siaqodbPathNew))
            {
                Directory.CreateDirectory(siaqodbPathNew);
            }
			Siaqodb siaqodb = new Siaqodb(siaqodbPathNew);

            Company comp = CreateCompany("MyCompany");

            Log("Store object encrypted...");
            //data of this object will be encrypted in DB
            siaqodb.StoreObject(comp);

            siaqodb.Close();

        }
        public void EncryptUsingXTEA_And_CustomPassword()
        {

            SiaqodbConfigurator.SetEncryptor(BuildInAlgorithm.XTEA);
            SiaqodbConfigurator.SetEncryptionPassword("secret_pwd");

            //now database files will be encrypted with  XTEA encryption algorithm and can be opened only with the paswword set
            //because previously, in method EncryptSimple, we used database from path SiaqodbFactoryExample.siaoqodbPath with another Encryption algorithm,
            //we cannot open now same database with another encryption settings(algorithm+pwd) so open another new DB
            string siaoqodbPathForEncrypt = SiaqodbFactoryExample.siaoqodbPath  +Path.DirectorySeparatorChar + @"siaqodbEncrypted";
            if (!Directory.Exists(siaoqodbPathForEncrypt))
            {
                Directory.CreateDirectory(siaoqodbPathForEncrypt);
            }
            Siaqodb siaqodb = new Siaqodb(siaoqodbPathForEncrypt);

            Company comp = CreateCompany("MyCompany");
            //data of this object will be encrypted in DB
            siaqodb.StoreObject(comp);

            siaqodb.Close();

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
