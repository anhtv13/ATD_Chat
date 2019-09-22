using IptLib.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ATDChatClient.Manager
{
    public class ChatEventFirer
    {
        protected static EventHandlerList m_eventList = new EventHandlerList();

        public static bool Subscribe(string eventName, EventHandler handler)
        {
            if (m_eventList != null)
            {
                lock (m_eventList)
                {
                    m_eventList.AddHandler(eventName, handler);
                }
                return true;
            }
            return false;
        }

        public static void Unsubscribe(string perceptType, EventHandler handler)
        {
            lock (m_eventList)
            {
                m_eventList.RemoveHandler(perceptType, handler);
            }
        }

        protected virtual void FireEvent(string eventName, object[] objData)
        {
            DataEventArgument args = new DataEventArgument(objData);
            lock (m_eventList)
            {
                EventHandler handler = (EventHandler)m_eventList[eventName];
                if (handler != null)
                {
                    EventHelper.FireEventAsync(handler, this, args);
                }
            }
        } 
    }
}
