using System.Collections;
using System;
using UJNet.Data;
using UnityEngine;

namespace UJNet
{
    class CommandHandler
    {

        private NetClient client;

        public CommandHandler(NetClient client)
        {
            this.client = client;
        }

        public void Handle(UJObject param)
        {
            // Check recv data 
            int cmd = param.GetInt("c");
            UJObject dataBin = param.GetUJObject("p");
            //string jid = param.GetUtfString("jid");
            //string rdurl = param.GetUtfString("rdurl");
            //==============自定义修改=====================
            //         if (!string.IsNullOrEmpty(jid)) {
            //	ScopeHolder.attr[Const.SCOPE_JID] = jid;
            //}
            //if (!string.IsNullOrEmpty(rdurl)) {
            //	ScopeHolder.attr[Const.SCOPE_HTTP_SVR_URL] = rdurl;
            //}
            //==============自定义修改=====================
            // Fire event!
            Hashtable parameters = Hashtable.Synchronized(new Hashtable());
            parameters.Add("cmd", cmd);
            parameters.Add("dataObj", dataBin);

            client.DispatchEvent(new UJNetEvent(UJNetEvent.onResponseEvent, parameters));
        }
    }
}

