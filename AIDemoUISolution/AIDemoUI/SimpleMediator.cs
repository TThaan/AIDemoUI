using System;
using System.Collections.Generic;

namespace AIDemoUI
{
    public interface ISimpleMediator
    {
        void NotifyColleagues(string token, object args);
        void Register(string token, Action<object> callback);
        void Unregister(string token, Action<object> callback);
    }

    public class SimpleMediator : ISimpleMediator
    {
        IDictionary<string, Action<object>> actions = new Dictionary<string, Action<object>>();
        public SimpleMediator()
        {

        }
        public void Register(string token, Action<object> callback)
        {
            actions[token] = callback;
        }
        public void Unregister(string token, Action<object> callback)
        {
            if (actions.ContainsKey(token))
                actions.Remove(token);
        }
        public void NotifyColleagues(string token, object args)
        {
            if (actions.ContainsKey(token))
                actions[token](args);
        }
    }
}
