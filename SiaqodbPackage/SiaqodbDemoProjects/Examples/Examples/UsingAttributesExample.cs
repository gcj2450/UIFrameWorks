using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo.Attributes;
using Sqo;
using Sqo.Exceptions;

namespace SiaqodbExamples
{
    class UsingAttributesExample : BaseExample, IExample
    {
        public void Run()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            siaqodb.DropType<AttUsage>();

            AttUsage at = new AttUsage();
            at.anotherInt = 100;
            at.SetInt(20);
            at.SetString("very long string");
            at.UniqueField = 10;
            siaqodb.StoreObject(at);

            IObjectList<AttUsage> list = siaqodb.LoadAll<AttUsage>();

            foreach (AttUsage a in list)
            {
                if (a.GetInt() == 0)
                {
                    Log("Memeber was ignored by siaqodb!");
                }
            }

            AttUsage at1 = new AttUsage();
            at1.anotherInt = 100;
            at1.SetInt(20);
            at1.SetString("very long string");
            //try to violate constraint
            at1.UniqueField = 10;

            try
            {
                siaqodb.StoreObject(at1);
            }
            catch (UniqueConstraintException ex)
            {
                Log("A unique contraint attempt to be violated!" + ex.Message) ;
            }

         
        }
    }
    class AttUsage:SqoDataObject
    {
        //by default length of a string stored=100 all strings that has Length>100 will be truncated
        //now we increase it to be possible to store bigger string values
        [MaxLength(300)] 
        private string myString;

        //means this member will be ignored by Siaqodb engine and not stored in DB
        [Ignore]
        private int test;
        
        public void SetString(string s)
        {
            myString = s;
            UnityEngine.Debug.Log(myString);
        }
        public void SetInt(int i)
        {
            test = i;
        }
        public int GetInt()
        {
            return test;
        }
        
        public int anotherInt;
        
        //values of this field should be Unique otherwise an UniqueConstraintException is thrown
        [UniqueConstraint]
        public int UniqueField { get; set; }

        int propertyBackingField;
        //Siaqodb store only fields (not properties), and sometimes inside properties can be done some operations so the engine cannot detect
        //what is backing field of this property, in this case you need to specify what is the field behind property by UseVariable attribute:
        [UseVariable("propertyBackingField")]
        public int SomeInt 
        {
            get
            {
                //do some stuff
                //int i=1+1;
                return propertyBackingField;

            }
            set
            {
                //do some stuff
                //int i=2+2;
                propertyBackingField=value;
            }
        }
        
        
    }
}
