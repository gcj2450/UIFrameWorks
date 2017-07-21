using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEntities
{
    /// <summary>
    /// 视频列表Item信息
    /// </summary>
    public class VideoInfo
    {
        /****数据示例
                 "last": "2016-10-26 14:59:48",
                "id": "29345",
                "name": "斩首循环蓝色学者与戏言跟班",
                "type": "动漫",
                "starring": "梶裕贵/悠木碧/嶋村侑/川澄绫子",
                "directed": "新房昭之/八瀬祐樹",
                "area": "日本",
                "lang": "日语",
                "year": "2016",
                "pic": "http://img.677dy.com/upload/vod/2016-10-26/201610268524632778.png",
                "des": "西尾维新出道作《戏言》系列动画确定以OVA形式问世 知名轻小说作家西尾维新的出道作品《斩首循环 蓝色学者与戏言玩家》决定推出OVA动画，并从今年10月26日开始发售蓝光和DVD第1卷，此后每个月发售1卷，一共8卷。",
                "dt": "jjvod",
                "note": ""
                */
        /// <summary>
        /// 视频id
        /// </summary>
        public int id;
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime last;
        /// <summary>
        /// 视频名称
        /// </summary>
        public string name;
        /// <summary>
        /// 分类
        /// </summary>
        public string type;
        /// <summary>
        /// 地区
        /// </summary>
        public string area;
        /// <summary>
        /// 语言
        /// </summary>
        public string lang;
        /// <summary>
        /// 年代
        /// </summary>
        public string year;
        /// <summary>
        /// 主演
        /// </summary>
        public string starring;
        /// <summary>
        /// 导演
        /// </summary>
        public string directed;
        /// <summary>
        /// 评分
        /// </summary>
        public string score;
        /// <summary>
        /// 推荐等级，推荐列表大于0
        /// </summary>
        public string level;
        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string pic;
        /// <summary>
        /// 幻灯图地址(推荐列表一定有)
        /// </summary>
        public string picslide;
        /// <summary>
        /// 简介
        /// </summary>
        public string des;



        
        ///// <summary>
        ///// 数据类型，乐视视频，爱奇艺视频等
        ///// </summary>
        //public string dt;
        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string note;
    }

    /// <summary>
    /// 视频列表
    /// </summary>
    public class VideoList
    {
        /*
        "status": 1,
    "msg": "ok",
    "page": 1,
    "pagecount": 185,
    "pagesize": 20,
    "recordcount": "3687",
    "vodlist": [  ]
    */
        /// <summary>
        /// 数据返回状态
        /// </summary>
        public int status;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg;
        /// <summary>
        /// 当前页数
        /// </summary>
        public int page;
        /// <summary>
        /// 总页数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 每页数据数
        /// </summary>
        public int pagesize;
        /// <summary>
        /// 总数据条数
        /// </summary>
        public int recordcount;
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<VideoInfo> vodlist = new List<VideoInfo>();
    }

    /// <summary>
    /// 视频分类
    /// </summary>
    public class VideoType
    {
        /*
         "id": "1",
          "name": "电影"
            */
        /// <summary>
        /// 分类id
        /// </summary>
        public string id;
        /// <summary>
        /// 分类名称
        /// </summary>
        public string name;
    }
    /// <summary>
    /// 分类列表
    /// </summary>
    public class VideoTypeList
    {
        /*
         {
        "status": 1,
        "msg": "ok",
        "typelist": []
        }
        */
        /// <summary>
        /// 数据返回状态
        /// </summary>
        public int status;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg;
        /// <summary>
        /// 分类列表
        /// </summary>
        public List<VideoType> typelist = new List<VideoType>();
    }

    public class VideoDetailList
    {
        /*
        {
    "status": 1,
    "msg": "ok",
    "vodinfo": {}
}
*/
        /// <summary>
        /// 数据返回状态
        /// </summary>
        public int status;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg;
        /// <summary>
        /// 
        /// </summary>
        public VideoDetail vodinfo;
        
    }

    public class VideoDetail
    {
        /*
        "last": "2016-11-07 15:14:00",
        "id": "1",
        "tid": "16",
        "name": "TopGirlVR视频",
        "type": "VR视频",
        "pic": "http://localhost/MacCMS/upload/vod/2016-10-25/201610251477377915.jpg",
        "lang": "国语",
        "area": "大陆",
        "year": "0",
        "state": "0",
        "note": "G.NA",
        "actor": "G.NA",
        "director": "韩国TopGirl",
        "des": "TopGirl 韩国MV  G.NA  左右3D格式，超美画面，给你不一样的视听体验。超美画面，给你不一样的视听体验。超美画面，给你不一样的视听体验。超美画面，给你不一样的视听体验。超美画面，给你不一样的视听体验。超美画面，给你不一样的视听体验。",
        "dl": [
            {
                "iva": "http://o9tkm8ot9.bkt.clouddn.com/film02.mp4"
            },
            {
                "link": "http://o9tkm8ot9.bkt.clouddn.com/film02.mp4"
            }
        ]
        */
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime last;
        /// <summary>
        /// 视频id
        /// </summary>
        public string id;
        /// <summary>
        /// 分类id
        /// </summary>
        public string tid;
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 视频分类
        /// </summary>
        public string type;
        /// <summary>
        /// 缩略图地址
        /// </summary>
        public string pic;
        /// <summary>
        /// 语言
        /// </summary>
        public string lang;
        /// <summary>
        /// 地区
        /// </summary>
        public string area;
        /// <summary>
        /// 年代
        /// </summary>
        public string year;
        /// <summary>
        /// 连载
        /// </summary>
        public string state;
        /// <summary>
        /// 备注
        /// </summary>
        public string note;
        /// <summary>
        /// 主演
        /// </summary>
        public string actor;
        /// <summary>
        /// 导演
        /// </summary>
        public string director;
        /// <summary>
        /// 描述、简介
        /// </summary>
        public string des;

        // "dl": [
        //    {
        //        "iva": "http://o9tkm8ot9.bkt.clouddn.com/film02.mp4"
        //    },
        //    {
        //        "link": "http://o9tkm8ot9.bkt.clouddn.com/film02.mp4"
        //    }
        //]

    }

}
