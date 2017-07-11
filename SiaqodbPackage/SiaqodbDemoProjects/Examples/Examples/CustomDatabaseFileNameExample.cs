using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo;
using System.IO;

namespace SiaqodbExamples
{
    class CustomDatabaseFileNameExample : BaseExample, IExample
    {


        public void Run()
        {
            //siaqodb store one file on disk for each Type and names of db files are composed by AssmeblyName+fullNameSpace of Type to be sure about uniqueness
            //but sometimes name of file can be too long, so would be better if you can customize it, Siaqodb allow that by:
            SiaqodbConfigurator.SetDatabaseFileName<DummyType>("mydummy");
            //1.you have to call it before open any database that has stored/will store objects of DummyType type
            //2.as you see is not necessary to add extension to fileName, engine put by default .sqo extension
            //3.only one call to the method above is required(static stored)

            //open another DB since SiaqodbFactoryExample.GetInstance() return an opened DB

            string siaoqodbPathNew = SiaqodbFactoryExample.siaoqodbPath+ Path.DirectorySeparatorChar + @"siaqodbWithCustomFileNames";
            if (!Directory.Exists(siaoqodbPathNew))
            {
                Directory.CreateDirectory(siaoqodbPathNew);
            }
            Siaqodb siaqodb = new Siaqodb(siaoqodbPathNew);
            
            siaqodb.StoreObject(new DummyType());

            Log("In database:"+siaoqodbPathNew+", fileName for DummyType is mydummy.sqo");
            siaqodb.Close();
        }

      
    }
    class DummyType
    {
        public int OID { get; set; }
        public string Name { get; set; }
    }
}
