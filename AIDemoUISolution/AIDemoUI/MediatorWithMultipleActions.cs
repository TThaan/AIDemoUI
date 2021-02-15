using System;
using System.Collections.Generic;

namespace AIDemoUI
{
    public class MediatorWithMultipleActions
    {
        IDictionary<string, List<Action<object>>> actions = new Dictionary<string, List<Action<object>>>();

        public void Register(string token, Action<object> callback)
        {
            if (!actions.ContainsKey(token))
            {
                var list = new List<Action<object>>();
                list.Add(callback);
                actions.Add(token, list);
            }
            else
            {
                bool found = false;
                foreach (var item in actions[token])
                    if (item.Method.ToString() == callback.Method.ToString())
                        found = true;
                if (!found)
                    actions[token].Add(callback);
            }
        }

        public void Unregister(string token, Action<object> callback)
        {
            if (actions.ContainsKey(token))
                actions[token].Remove(callback);
        }

        public void NotifyColleagues(string token, object args)
        {
            if (actions.ContainsKey(token))
                foreach (var callback in actions[token])
                    callback(args);
        }
    }
}
