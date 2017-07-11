using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo.Attributes;
using Sqo;

namespace SiaqodbExamples
{
    class IndexExample : BaseExample, IExample
    {
        public void Run()
        {

            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            siaqodb.DropType<ClassWithIndex>();//all objects of this Type will be deleted from database

            for (int i = 0; i < 10000; i++)
            {
                ClassWithIndex myobj = new ClassWithIndex() { MyID = i % 10, Name = "MyTest" + i.ToString() };

                siaqodb.StoreObject(myobj);
            }
            DateTime start = DateTime.Now;
            var q = from ClassWithIndex myobj in siaqodb
                    where myobj.MyID == 8//index will be used so very fast retrieve from DB
                    select myobj;
            int k = 0;
            foreach (ClassWithIndex obj in q)
            {
                //do something with object
                UnityEngine.Debug.Log(obj.OID);
                k++;
            }
            string timeElapsed = (DateTime.Now - start).ToString();

            Log("Time elapsed to load:" + k.ToString() + " objects from 10.000 stored objects filtered by index:" + timeElapsed);
        }

       
    }
    class ClassWithIndex
    {
        public int OID { get; set; }

        [Index]
        public int MyID { get; set; }

        public string Name { get; set; }
    }
}
