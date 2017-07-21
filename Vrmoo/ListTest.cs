using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListTest : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Removetest();
            AddTest();
            InsertTest();
        }
    }

    void Removetest()
    {
        List<string> Strs = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            Strs.Add(i + "_string");
        }
        Strs.RemoveRange(2, 2);
        LogStrs(Strs, "Strs.RemoveRange(2, 2)");
        Strs.Remove("5_string");
        LogStrs(Strs, "Strs.Remove(5_string)");
        Strs.RemoveAt(3);
        LogStrs(Strs, "Strs.RemoveAt(3)");
    }
    
    void AddTest()
    {
        List<string> Strs = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            Strs.Add(i + "_string");
        }
        List<string> addstr = new List<string>();
        addstr.Add("a");
        addstr.Add("b");
        addstr.Add("c");
        addstr.Add("d");
        Strs.Add("show");
        LogStrs(Strs, "Strs.Add(show)");
        Strs.AddRange(addstr);
        LogStrs(Strs, "Strs.AddRange(addstr)");
    }

    void InsertTest()
    {
        List<string> Strs = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            Strs.Add(i + "_string");
        }
        Strs.Insert(0, "insert0");
        LogStrs(Strs, "insert1次");
        Strs.Insert(0, "insert1");
        LogStrs(Strs, "insert2次");
        Strs.Insert(0, "insert2");
        LogStrs(Strs, "insert3次");
        List<string> addstr = new List<string>();
        addstr.Add("a");
        addstr.Add("b");
        addstr.Add("c");
        addstr.Add("d");
        Strs.InsertRange(0, addstr);
        LogStrs(Strs,"insrtRange");
    }


    void LogStrs(List<string>_str, string _method)
    {
        Debug.Log("____________" + _method + "______________");
        for (int i = 0; i < _str.Count; i++)
        {
            Debug.Log(i + "__" + _str[i]);
        }
    }

}
