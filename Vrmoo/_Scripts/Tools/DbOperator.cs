using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sqo;
using System.IO;
using System.Linq;

public class DbOperator : MonoBehaviour
{
    private static Siaqodb instance;
    
    /// <summary>获取数据库实例
    /// </summary>
    /// <returns></returns>
    public static Siaqodb GetDBInstance()
    {
        SiaqodbConfigurator.EncryptedDatabase = true;
        if (instance == null)
        {
            SiaqodbConfigurator.SetEncryptor(BuildInAlgorithm.XTEA);
            SiaqodbConfigurator.SetEncryptionPassword("secret_pwd");

            //now database files will be encrypted with  XTEA encryption algorithm and can be opened only with the paswword set
            //because previously, in method EncryptSimple, we used database from path SiaqodbFactoryExample.siaoqodbPath with another Encryption algorithm,
            //we cannot open now same database with another encryption settings(algorithm+pwd) so open another new DB
            string siaoqodbPathForEncrypt = Application.persistentDataPath + Path.DirectorySeparatorChar + @"siaqodbEncrypted";
            if (!Directory.Exists(siaoqodbPathForEncrypt))
            {
                Directory.CreateDirectory(siaoqodbPathForEncrypt);
            }
            Siaqodb siaqodb = new Siaqodb(siaoqodbPathForEncrypt);
            instance = siaqodb;
        }
        return instance;
    }
    public static void CloseDatabase()
    {
        if (instance != null)
        {
            instance.Close();
            SiaqodbConfigurator.EncryptedDatabase = false;
            instance = null;
        }
    }
    /// <summary>获取数据库中某个类的所有数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> GetDataList<T>() where T : class
    {
        Siaqodb database = GetDBInstance();
        try
        {
            List<T> retValue = null;
            IEnumerable<T> query;
            query = (from T pfinfo in database
                         //where pfinfo.InfoType == _infoType
                         //orderby pfinfo.LocalInfoID descending
                     select pfinfo);
            //Debug.Log("================query  _is public ==0");
            if (query == null)
            {
                //Debug.Log("AAAAAA     query is null");
                return null;
            }
            else
            {
                //Debug.Log("query out put " + query);
                //Debug.Log("query.ToList<PlayerFrendsInfo>() .Count : " + query.ToList<PlayerFrendsInfo>().Count);
                retValue = query.ToList<T>();
                //Debug.Log("----------retValue.Count : " + retValue.Count);
                //if (retValue.Count < totalCount)
                return retValue;
                //else
                //    return retValue.GetRange(0, totalCount);
            }
        }
        catch (System.Exception ex)
        {
            //Debug.Log("query  catch exception: ++++++++++++" + ex.Message);
            Debug.Log(ex);
            return null;
        }

    }

    //public static void SaveDataList(List<object> classList)
    //{
    //    Siaqodb database = GetDBInstance();
    //    database.BeginTransaction();
    //    for (int i = 0; i < classList.Count; i++)
    //    {

    //        if (database != null)
    //        {
    //            try
    //            {
    //                int curInfoID = classList[i].id;
    //                var query = (from ServerDataObj pfi in database where pfi.id == curInfoID select pfi);
    //                if (query != null && query.ToList<ServerDataObj>().Count > 0)
    //                {
    //                    var tempValue = query.ToList<ServerDataObj>()[0];
    //                    VideoResList[i].OID = tempValue.OID;
    //                }
    //            }
    //            catch (System.Exception ex)
    //            {
    //                //Debug.Log("------------OperateBusinessCategoryInfo-try Exception ex : " + ex.Message);
    //                //Debug.Log(ex);
    //            }
    //            database.StoreObject(VideoResList[i]);
    //        }
    //    }
    //}

}
