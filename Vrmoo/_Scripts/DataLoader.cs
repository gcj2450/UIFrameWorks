using UnityEngine;
using System.Collections;
using System;
using GameEntities;
using System.Collections.Generic;
/// <summary>
/// CMS视频数据加载器
/// </summary>
public class DataLoader : MonoBehaviour
{
    /// <summary>
    /// 分类列表
    /// </summary>
    string TypeListUrl = "http://localhost/MacCMS/inc/json_api.php?ac=typelist";
    /// <summary>
    /// 推荐列表
    /// </summary>
    string RecommendURL = "http://localhost/MacCMS/inc/json_api.php?ac=vodrec";

    /// <summary>
    /// 视频列表，pg=1为页数，t=1 t为分类ID
    /// 完全地址示例//"http://localhost/MacCMS/inc/json_api.php?ac=vodlist&pg=1&t=1";
    /// </summary>
    string VideoListPartUrl = "http://localhost/MacCMS/inc/json_api.php?ac=vodlist";

    /// <summary>
    /// 视频详情，id为视频id
    /// 完全地址示例//"http://localhost/MacCMS/inc/json_api.php?ac=vodinfo&id=1";
    /// </summary>
    string VideoDetailPartUrl = "http://localhost/MacCMS/inc/json_api.php?ac=vodinfo";
    /// <summary>
    /// 当前请求地址
    /// </summary>
    string _curListUrl = "";

    /// <summary>
    /// 视频分类列表回调
    /// </summary>
    public Action<List<VideoType>> VideoTypeListCallBack;
    /// <summary>
    /// 视频列表回调
    /// </summary>
    public Action<List<VideoInfo>> VideoListCallBack;
    /// <summary>
    /// 视频详情回调
    /// </summary>
    public Action<VideoDetail> VideoDetailCallBack;

    /// <summary>
    /// 获取视频分类列表
    /// </summary>
    public void LoadVideoTypeList()
    {
        Action<RequestData, string> callback = TypeListCallback;
        RequestData requestData;
        requestData = new RequestData(TypeListUrl, callback, true);
        RequestManager.Instance.AddRequest(requestData);
    }
    /// <summary>
    /// 视频分类列表回调
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void TypeListCallback(RequestData arg1, string arg2)
    {
        if (!arg1.Url.Equals(TypeListUrl))    //验证一下地址，这一步可以不做
        {
            return;
        }
        //解析jsonObjects，发给需要的接口
        VideoTypeList videoTpList = JsonStringConverter.JsonToClass<VideoTypeList>(arg2);
        if (videoTpList.status==1&&videoTpList.typelist.Count>0)
        {
            if (VideoTypeListCallBack != null)
                VideoTypeListCallBack(videoTpList.typelist);
        }
    }

    /// <summary>
    /// 获取视频推荐列表
    /// </summary>
    public void LoadRecommendVideoList()
    {
        Action<RequestData, string> callback = ListCallBack;
        _curListUrl = RecommendURL;
        RequestData requestData;
        requestData = new RequestData(RecommendURL, callback, true);
        RequestManager.Instance.AddRequest(requestData);
    }

    /// <summary>
    /// 获取视频列表
    /// </summary>
    /// <param name="_page"></param>
    /// <param name="_type"></param>
    public void LoadVideoList(int _page,int _type)
    {
        string _url = VideoListPartUrl + "&pg=" + _page + "&t=" + _type;
        _curListUrl = _url;
        Action<RequestData, string> callback = ListCallBack;
        RequestData requestData;
        requestData = new RequestData(_url, callback, true);
        RequestManager.Instance.AddRequest(requestData);

    }
    /// <summary>
    /// 视频列表回调
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void ListCallBack(RequestData arg1, string arg2)
    {
        if (!arg1.Url.Equals(_curListUrl))    //验证一下地址，这一步可以不做
        {
            return;
        }
        VideoList vodList = JsonStringConverter.JsonToClass<VideoList>(arg2);
        if (vodList.status == 1 && vodList.vodlist.Count > 0)
        {
            if (VideoListCallBack != null)
                VideoListCallBack(vodList.vodlist);
        }
    }

    /// <summary>
    /// 获取视频详情
    /// </summary>
    /// <param name="_id"></param>
    public void LoadVideoDetail(int _id)
    {
        string _url = VideoDetailPartUrl + "&id=" + _id;
        _curListUrl = _url;
        Action<RequestData, string> callback = DetailListCallBack;
        RequestData requestData;
        requestData = new RequestData(_url, callback, true);
        RequestManager.Instance.AddRequest(requestData);
    }

    private void DetailListCallBack(RequestData arg1, string arg2)
    {
        if (!arg1.Url.Equals(_curListUrl))    //验证一下地址，这一步可以不做
        {
            return;
        }
        
        VideoDetailList vodList = JsonStringConverter.JsonToClass<VideoDetailList>(arg2);
        Debug.Log("vodList.status:" + vodList.status + "__vodList.msg : " + vodList.msg+"__"+( vodList.vodinfo != null));
        if (vodList.status == 1 && vodList.vodinfo!=null)
        {
            if (VideoDetailCallBack != null)
                VideoDetailCallBack(vodList.vodinfo);
        }
    }

    void Start()
    {
        LoadVideoDetail(1995525);
    }
}
