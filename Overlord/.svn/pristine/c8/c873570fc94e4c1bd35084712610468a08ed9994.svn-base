

using SimpleJson;
using System;
using System.Collections.Generic;

namespace Pomelo.DotNetClient
{
    public class EventManager : IDisposable
    {
        private Dictionary<uint, Action<JsonObject>> callBackMap;
        private Dictionary<string, List<Action<JsonObject>>> eventMap;

        public EventManager()
        {
            this.callBackMap = new Dictionary<uint, Action<JsonObject>>();
            this.eventMap = new Dictionary<string, List<Action<JsonObject>>>();
        }

        public void AddCallBack(uint id, Action<JsonObject> callback)
        {
            if (id <= 0U || callback == null)
                return;
            this.callBackMap.Add(id, callback);
        }

        public void InvokeCallBack(uint id, JsonObject data)
        {
            if (!this.callBackMap.ContainsKey(id))
                return;
            this.callBackMap[id](data);
        }

        public void AddOnEvent(string eventName, Action<JsonObject> callback)
        {
            List<Action<JsonObject>> list = (List<Action<JsonObject>>)null;
            if (this.eventMap.TryGetValue(eventName, out list))
                list.Add(callback);
            else
                this.eventMap.Add(eventName, new List<Action<JsonObject>>()
        {
          callback
        });
        }

        public void InvokeOnEvent(string route, JsonObject msg)
        {
            if (!this.eventMap.ContainsKey(route))
                return;
            foreach (Action<JsonObject> action in this.eventMap[route])
                action(msg);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected void Dispose(bool disposing)
        {
            this.callBackMap.Clear();
            this.eventMap.Clear();
        }
    }
}
