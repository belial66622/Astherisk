using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickUpdateSystem
{
    
    public event Action<int> TickUpdate;


    TickUpdateSystemObject timeTickSystemObject;
    public TickUpdateSystem(float tickTime)
    {
        GameObject timeTickSystemObjectGO = null;
        if (timeTickSystemObjectGO == null)
        {
            timeTickSystemObjectGO = new GameObject("TimeTickSystem");
            timeTickSystemObject = timeTickSystemObjectGO.AddComponent<TickUpdateSystemObject>();
            timeTickSystemObject.SetUpdateTime(tickTime, this);
        }
    }

    public int GetTick()
    {
        return timeTickSystemObject.GetTick();
    }
    
    class TickUpdateSystemObject : MonoBehaviour
    {
        float tickUpdateTime;
        float tickTimer;
        int tick;
        TickUpdateSystem tickUpdateSystem;
        public void SetUpdateTime(float timer, TickUpdateSystem tickUpdateSystem)
        {
            tickUpdateTime = timer;
            this.tickUpdateSystem = tickUpdateSystem;
        }
        void Update()
        {
            Tick();
        }

        private void Tick()
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= tickUpdateTime)
            {
                tickTimer -= tickUpdateTime;
                tick++;

                tickUpdateSystem.TickUpdate?.Invoke(tick);
            }
        }

        public int GetTick()
        {
            return tick;
        }
    }
    
}
