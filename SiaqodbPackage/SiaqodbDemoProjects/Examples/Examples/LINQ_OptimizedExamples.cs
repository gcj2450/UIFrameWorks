using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo;


namespace SiaqodbExamples
{
    class LINQ_OptimizedExamples : BaseExample, IExample
    {
        //siaqodb engine can optimize queries to database for following Extension methods:
        //Any<T>(…)
        //Count<T>(…)
        //First<T>(…)
        //FirstOrDefault<T>(…)
        //Last<T>(…)
        //LastOrDefault(…)
        //Single<T>(…)
        //SingleOrDefault(…)
        //Take<T>(…)
        //Skip<T>(…)


        public void Run()
        {
            this.Select();
            this.SelectOnlySomeProperties();
            this.Any();
            this.Count();
            this.First();
            this.FirstOrDefault();
            this.Last();
            this.LastOrDefault();
            this.Single();
            this.SingleOrDefault();
            this.Where();

            this.PaginationWith_Skip_Take();

        }
        public void Select()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();

            for (int i = 0; i < 30; i++)
            {
                Developer deve = new Developer() { Age = 18 + i, FirstName = "Devo" + i.ToString(), LastName = "None" };
                siaqodb.StoreObject(deve);
            }
            var query = from Company c in siaqodb
                        select c;
            //same query written using lambda expression arguments
            var q2 = siaqodb.Cast<Company>().Select(c => c);
            UnityEngine.Debug.Log(q2.ToString());   //just for close warning log
            foreach (Company c in query)
            {
                //do something with c object
                UnityEngine.Debug.Log(c.OID);
            }
        }
        public void SelectOnlySomeProperties()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();

            //the query will NOT create any Company objects,
            //it will read ONLY that 2 properties values and anonymous Type will be created with that values
            //so query is optimal
            var query = from Company c in siaqodb
                        select new { Name = c.Name, Phone = c.Phone };

            //same query written using lambda expression arguments
            var q2 = siaqodb.Cast<Company>().Select(c => new { Name = c.Name, Phone = c.Phone });
            UnityEngine.Debug.Log(q2.ToString());   //Just for close warning log
            foreach (var c in query)
            {
                //do something with c object
                UnityEngine.Debug.Log(c.Name);
            }
        }
        public void Where()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            //this query run optimized-> only Developers objects will be created that match the condition
            //it does not matter how complex your WHERE is, it will run OPTIMIZED except some cases: see LINQ_UnOptimizedExamples
            DateTime dt = new DateTime(2008, 1, 1);
            var query = from Developer emp in siaqodb
                        where emp.Age > 20 && emp.HireDate < dt
                        select emp;

            //same query written in other way using lambda expression arguments
            var q2 = siaqodb.Cast<Developer>().Where(emp => emp.Age > 20 && emp.HireDate < dt);
            UnityEngine.Debug.Log(q2.ToString());   //Just for close warning log

            foreach (Developer c in query)
            {
                //do something with c object
                UnityEngine.Debug.Log(c.OID);
            }
        }
        public void First()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();

            try
            {
                //this query run optimized-> only first Developer object will be created and read from database that match the condition,otherwise InvalidOperationException is thrown
                Developer first = (from Developer emp in siaqodb
                                   where emp.Age > 20
                                   select emp).First();
                UnityEngine.Debug.Log(first.ToString());   //Just for close warning log
                //same query written in other way using lambda expression arguments
                Developer first2 = siaqodb.Cast<Developer>().Where(emp => emp.Age > 20).First();
                UnityEngine.Debug.Log(first2.ToString());   //Just for close warning log
                // or similar-> also run optimized:
                Developer first3 = siaqodb.Cast<Developer>().First(emp => emp.Age > 20);
                UnityEngine.Debug.Log(first3.ToString());   //Just for close warning log
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }

        }
        public void FirstOrDefault()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            //this query run optimized-> only first Developer object will be created and read from database that match the condition
            Developer first = (from Developer emp in siaqodb
                               where emp.Age > 20
                               select emp).FirstOrDefault();
            UnityEngine.Debug.Log(first.ToString());   //Just for close warning log
            //same query written in other way using lambda expression arguments
            Developer first2 = siaqodb.Cast<Developer>().Where(emp => emp.Age > 20).FirstOrDefault();
            UnityEngine.Debug.Log(first2.ToString());   //Just for close warning log
            // or similar-> also run optimized:
            Developer first3 = siaqodb.Cast<Developer>().FirstOrDefault(emp => emp.Age > 20);
            UnityEngine.Debug.Log(first3.ToString());   //Just for close warning log

        }
        public void Count()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            // to count all objects stored in db of a certain Type:
            int nrObjects = siaqodb.Count<Developer>();
            UnityEngine.Debug.Log(nrObjects.ToString());   //Just for close warning log
            //count all objects that match a condition
            //-> will run optimized, so NO object is created, the engine only counts objects stored
            int nrObjects2 = (from Developer e in siaqodb
                              where e.Age > 20
                              select e).Count();
            UnityEngine.Debug.Log(nrObjects2.ToString());   //Just for close warning log
            //same query written in other way using lambda expression arguments
            int nrObjects3 = siaqodb.Cast<Developer>().Where(emp => emp.Age > 20).Count();
            UnityEngine.Debug.Log(nrObjects3.ToString());   //Just for close warning log
            //similar query->same result -> also optimized
            int nrObjects4 = siaqodb.Cast<Developer>().Count(emp => emp.Age > 20);
            UnityEngine.Debug.Log(nrObjects4.ToString());   //Just for close warning log
        }
        public void Any()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            //check if exists at least one object that match the condition
            //run optimized-> create only first fetched object if exists
            bool exists = (from Developer e in siaqodb
                           where e.Age > 20
                           select e).Any();
            UnityEngine.Debug.Log(exists.ToString());   //Just for close warning log
            //same query written in other way using lambda expression arguments
            bool exists2 = siaqodb.Cast<Developer>().Where(emp => emp.Age > 20).Any();
            UnityEngine.Debug.Log(exists2.ToString());   //Just for close warning log
            //similar query->same result -> also optimized
            bool exists3 = siaqodb.Cast<Developer>().Any(emp => emp.Age > 20);
            UnityEngine.Debug.Log(exists3.ToString());   //Just for close warning log
        }
        public void Last()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();

            try
            {
                //this query run optimized-> only last Developer object will be created and read from database that match the condition,otherwise InvalidOperationException is thrown
                Developer last = (from Developer emp in siaqodb
                                  where emp.Age > 20
                                  select emp).Last();
                UnityEngine.Debug.Log(last.ToString());   //Just for close warning log
                //same query written in other way using lambda expression arguments
                Developer last2 = siaqodb.Cast<Developer>().Where(emp => emp.Age > 20).Last();
                UnityEngine.Debug.Log(last2.ToString());   //Just for close warning log
                // or similar-> also run optimized:
                Developer last3 = siaqodb.Cast<Developer>().Last(emp => emp.Age > 20);
                UnityEngine.Debug.Log(last3.ToString());   //Just for close warning log
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }
        }
        public void LastOrDefault()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();


            //this query run optimized-> only last Developer object will be created and read from database that match the condition
            Developer last = (from Developer emp in siaqodb
                              where emp.Age > 20
                              select emp).LastOrDefault();
            UnityEngine.Debug.Log(last.ToString());   //Just for close warning log
            //same query written in other way using lambda expression arguments
            Developer last2 = siaqodb.Cast<Developer>().Where(emp => emp.Age > 20).LastOrDefault();
            UnityEngine.Debug.Log(last2.ToString());   //Just for close warning log
            // or similar-> also run optimized:
            Developer last3 = siaqodb.Cast<Developer>().LastOrDefault(emp => emp.Age > 20);
            UnityEngine.Debug.Log(last3.ToString());   //Just for close warning log
        }
        public void Single()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();

            try
            {

                //this query run optimized-> only one Developer object will be created and read from database
                //AND ONLY if is only one object that match the condition, otherwise InvalidOperationException is thrown 
                Developer single = (from Developer emp in siaqodb
                                    where emp.Age > 20
                                    select emp).Single();
                UnityEngine.Debug.Log(single.ToString());   //Just for close warning log
                //same query written in other way using lambda expression arguments
                Developer single2 = siaqodb.Cast<Developer>().Where(emp => emp.Age > 20).Single();
                UnityEngine.Debug.Log(single2.ToString());   //Just for close warning log
                // or similar-> also run optimized:
                Developer single3 = siaqodb.Cast<Developer>().Single(emp => emp.Age > 20);
                UnityEngine.Debug.Log(single3.ToString());   //Just for close warning log
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }

        }
        public void SingleOrDefault()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();

            try
            {

                //this query run optimized-> only one Developer object will be created and read from database IF EXISTS and IF is the only one in database that match the condition,
                //if not exists return null, if there are MANY InvalidOperationException is thrown 
                Developer single = (from Developer emp in siaqodb
                                    where emp.Age > 20
                                    select emp).SingleOrDefault();
                UnityEngine.Debug.Log(single.Age);
                //same query written in other way using lambda expression arguments
                Developer single2 = siaqodb.Cast<Developer>().Where(emp => emp.Age > 20).SingleOrDefault();
                UnityEngine.Debug.Log(single2.Age);
                // or similar-> also run optimized:
                Developer single3 = siaqodb.Cast<Developer>().SingleOrDefault(emp => emp.Age > 20);
                UnityEngine.Debug.Log(single3.Age);
            }
            catch (InvalidOperationException ex)
            {
                Log(ex.Message);
            }

        }
        public void PaginationWith_Skip_Take()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();

            //if you need pagination in your application you can use Skip/Take methods 
            //in optimized way to pull only needed objects from DB
            //bellow query only pull and create 10 objects
            var query = (from Developer emp in siaqodb
                         where emp.Age > 20
                         select emp).Skip(10).Take(10);
            //do something with c object
            foreach (var c in query)
            {
                UnityEngine.Debug.Log(c.OID);
            }
        }
    }
}
