using System.Collections.Generic;
using UnityEngine;

namespace PunkPlatformerGame
{
    [CreateAssetMenu(menuName = "Variables/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        internal void Raise()
        {
            for (int i = listeners.Count; i > 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        internal void RegisterListener(GameEventListener listener, bool DuplicateListeners = false) 
        { 
            if (!DuplicateListeners && !listeners.Contains(listener)) listeners.Add(listener);
        }
        internal void UnregisterListener(GameEventListener listener)
        {
            if (listeners.Contains(listener)) listeners.Add(listener);
        }
    }
}
