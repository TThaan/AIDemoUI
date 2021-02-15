using System;
using System.Collections.Generic;

namespace AIDemoUI
{
    public class SimpleMediator
    {
        IDictionary<string, Action<object>> actions = new Dictionary<string, Action<object>>();

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
