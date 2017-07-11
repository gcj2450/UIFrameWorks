using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sqo;

namespace SiaqodbExamples
{
    //These examples is for iOS and Android where JIT is not allowed
    //!!! important: this examples may run well on Emulator, but crashes are expected on device.
	class LINQ_CrashExamples : BaseExample, IExample
    {
        

        public void Run()
        {
            //there are cases when queries cannot be optimized by Siaqodb engine(eq: Expression tree cannot be translated by engine)
            //and in this case, because JIT is not allowed on device,when Siaqodb try to compile the expression,it crashes!!!
            this.Where_CrashUsingStringMethods();
            this.Where_CrashUsingLocalMethod();
            this.Where_CrashUsingUnaryOperator();
        }
        public void Where_CrashUsingStringMethods()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();

            //this query will crash, problem is Length property of String
            //siaqodb can only handle optimized following String methods:StartsWith,EndsWith and Contains
            //all other String methods/properties for object members used in LINQ statements will crash
            
			try{
				var query = from Company c in siaqodb
	                        where c.Phone.Length == 3
	                        select c;
	            
	            foreach (Company c in query)
	            {
                    //do something with c
                    UnityEngine.Debug.Log(c.OID);
                }
			}
			catch(Exception ex)
			{
				
				Log("Example crash as expected:"+ex.Message);
			}
            // !!! Important: if those String methods are used for a local variable and NOT to a member of a stored object
            //query run optimized so:
            string d = "test";
            var queryOptimized = from Company c in siaqodb
                                    where c.Name == d.Substring(2)
                                    select c;

            //the above query runs well
            foreach (Company c in queryOptimized)
            {
                //do something with c
                UnityEngine.Debug.Log(c.OID);
            }
			Log("String methods of a local variable and NOT of a member of a stored object,works fine!");

        }
        public int TestMet(int t)
        {
            return t + 1;
        }
        public void Where_CrashUsingLocalMethod()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();

           try{
				//this query will crash, problem is TestMet method that get a member of a storable object
	            //as argument
	            
				var query = from Employee e in siaqodb
	                        where TestMet(e.Age) == 30
	                        select e;
	            
	            foreach (Employee e in query)
	            {
                    //do something with e
                    UnityEngine.Debug.Log(e.OID);
                }
			}
			catch(Exception ex)
			{
				Log("Example with local method crash as expected:"+ex.Message);
				
			}
            try{
				//query bellow will crash also
				int myInt=30;
	            var query2 = from Employee e in siaqodb
	                        where e.Age == TestMet(myInt)
	                        select e;
	            foreach (Employee e in query2)
	            {
                    //do something with e
                    UnityEngine.Debug.Log(e.OID);
                }
			}
			catch(Exception ex)
			{
				Log("Example with local method crash as expected:"+ex.Message);
				
			}
			//but as workaround for above query use this:
			int myInt2=TestMet(30);
	        var queryOptimized = from Employee e in siaqodb
	                        where e.Age == myInt2
	                        select e;
	         foreach (Employee e in queryOptimized)
	           {
                //do something with e
                UnityEngine.Debug.Log(e.OID);
            }
			Log("Example with local method workaround works fine!");
            
        }
        public void Where_CrashUsingUnaryOperator()
        {

            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            for (int i = 0; i < 10; i++)
            {
                BoolAndIntExample e = new BoolAndIntExample() { SomeBool = i % 2 == 0, SomeInt = i };
                siaqodb.StoreObject(e);
            }
			
			try{
	            //this query crash, problem is !(Not) operator 
	            var query = from BoolAndIntExample e in siaqodb
	                        where e.SomeInt>5 && !e.SomeBool
	                        select e;
	            foreach (BoolAndIntExample e in query)
	            {
                    UnityEngine.Debug.Log(e.OID);
	                //do something with e
	            }
			}
			catch(Exception ex)
			{
				Log("Example with unary operator crash as expected:"+ex.Message);
				
			}

            //to optimize query above, just use Equal operator:
            var queryOptimized = from BoolAndIntExample e in siaqodb
                        where e.SomeInt > 5 && e.SomeBool==false
                        select e;
            //do something with e
            foreach (BoolAndIntExample e in queryOptimized)
            {
                UnityEngine.Debug.Log(e.OID);
            }
        }

        class BoolAndIntExample
        {
            public int OID { get; set; }
            
            public int SomeInt { get; set; }
            public bool SomeBool { get; set; }
        }
       
    }
}
