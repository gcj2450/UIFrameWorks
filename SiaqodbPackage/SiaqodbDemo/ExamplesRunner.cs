using UnityEngine;
using System.Collections;
using SiaqodbDemo;
using Sqo;
using GameEntities;
using System.Collections.Generic;
using System;
using System.IO;
public class ExamplesRunner : MonoBehaviour
{

    void Start()
    {

               
	//if ANDROID:
        //SiaqodbExamples.SiaqodbFactoryExample.SetDBFolder(Application.persistentDataPath);
        //if Windows or MAC
        string siaoqodbPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + @"Assets"+Path.DirectorySeparatorChar + @"SiaqodbPackage"+Path.DirectorySeparatorChar+@"dbFolder";
        SiaqodbExamples.SiaqodbFactoryExample.SetDBFolder(siaoqodbPath);
        //if iOS (iPhone /iPad)
        //SiaqodbExamples.SiaqodbFactoryExample.SetDBFolder(Application.dataPath);

        IList<SiaqodbExamples.IExample> examples = SiaqodbExamples.ExamplesBuilder.GetExamples(this.WriteToConsole);
        foreach (SiaqodbExamples.IExample example in examples)
        {
            this.WriteToConsole("Start example:" + example.GetType().Name + "...");
            example.Run();
            this.WriteToConsole("End example:" + example.GetType().Name + "...");
            this.WriteToConsole("----------------------------------------------");

        }
        this.WriteToConsole("Examples run finished!!!");
        SiaqodbExamples.SiaqodbFactoryExample.CloseDatabase();

    }
    public void WriteToConsole(string msg)
    {
        Debug.Log(msg);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
