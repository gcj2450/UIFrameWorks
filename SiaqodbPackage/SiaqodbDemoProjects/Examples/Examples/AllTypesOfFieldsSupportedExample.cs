using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sqo;


namespace SiaqodbExamples
{
    class AllTypesOfFieldsSupportedExample : BaseExample, IExample
    {
        //siaqodb support as members of a storable class following types:
        //**********************************************************************
        //int,uint,short,string,ushort,byte,sbyte,long,ulong,float,double,decimal,char,
        //bool,TimeSpan,DateTime,Guid, enum
        //************************************************************************

        public void Run()
        {
            Siaqodb siaqodb = SiaqodbFactoryExample.GetInstance();
            //clear all objects of Type BigClass created by previous run of app
            siaqodb.DropType<BigClass>();

            siaqodb.StoreObject(new BigClass());

            IObjectList<BigClass> list = siaqodb.LoadAll<BigClass>();
            UnityEngine.Debug.Log(list.Count);  //Just for close warning log
            

           
        }
    }
    public class BigClass :SqoDataObject
    {
       
        public int i;
        public uint iu;
        public short s;
        public string str = "test";
        public ushort us;
        public byte b;
        public sbyte sb;
        public long l;
        public ulong ul;
        public float f;
        public double d;
        public decimal de;
        public char c='k';
        public bool bo;
        public TimeSpan ts;
        public DateTime dt;
        public Guid g;
        public TestEnum enn = TestEnum.Two;
       

    }
    public enum TestEnum { One,Two}
}
