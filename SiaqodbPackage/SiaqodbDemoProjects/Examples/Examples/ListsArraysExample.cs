using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo;

namespace SiaqodbExamples
{
    class ListsArraysExample :BaseExample,IExample
    {
        
        public void Run()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            siaqodb.DropType<MyClassWithLists>();
            for (int i = 0; i < 10; i++)
            {
                MyClassWithLists ps = new MyClassWithLists();
                ps.ID = i;
                ps.myList = new List<int>();
                if (i % 2 == 0)
                {
                   for(int j=0;j<3;j++)
                   {
                       ps.myList.Add(j * i);
                    }
                }
                ps.blob = new byte[] { 1, 2, 3 };

                siaqodb.StoreObject(ps);
            }

            //this query will run optimazed, only instances that satify condition are created by engine
            var q = from MyClassWithLists myListObj in siaqodb
                    where myListObj.myList.Contains(2)
                    select myListObj;

            Log("Following objects contains in myList prop element = 2:");
            //do somthing with obj
            foreach (MyClassWithLists mcl in q)
            {
                UnityEngine.Debug.Log(mcl.OID);
            }

        }

        
    }
    public class MyClassWithLists
    {
        public int OID { get; set; }

        public int ID { get; set; }

        public List<int> myList;

        public byte[] blob;

        public int[] intArr;

    }
}
