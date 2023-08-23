using System;
using static UnityEngine.Rendering.DebugUI;

namespace Utilities 
{
    public abstract class TimerUtils 
    {
        protected float initialTime;
        protected float Time { get; set; }

        protected float timeToTick = 0;

        public bool IsRunning { get; protected set; }
        public float Progress => Time / initialTime;
        public float InverseProgress => 1 - Progress;

        public int Ticks { get; protected set; }
        
        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };
        public Action<int> OnTimerTickUpdate = delegate { };

        protected TimerUtils(float value)
        {
            initialTime = value;
            IsRunning = false;

        }

        public void Start() {
            Time = initialTime;
            Ticks = 0;
            if (!IsRunning) {
                IsRunning = true;
                OnTimerStart.Invoke();
            }
        }

        public void Stop() {
            if (IsRunning) {
                IsRunning = false;
                   OnTimerStop.Invoke();
            }
        }
        
        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;
        
        public abstract void Tick(float deltaTime);
    }
    
    public class CountdownTimer : TimerUtils 
    {
        public CountdownTimer(float value) : base(value) 
        {
            timeToTick = value / 10;
            tickTimeTemp = timeToTick;
        }

        float tickTimeTemp;
        public override void Tick(float deltaTime) {
            if (IsRunning && Time > 0) {
                Time -= deltaTime;
                tickTimeTemp -= deltaTime;
                if(tickTimeTemp <= 0){
                    Ticks++;
                    tickTimeTemp += timeToTick;
                    OnTimerTickUpdate.Invoke(Ticks);
                }
            }
            
            if (IsRunning && Time <= 0) {
                Stop();
            }
        }
        
        public void Test(int tick)
        {
            if(Ticks == tick)
            {
                OnTimerTickUpdate.Invoke(tick);
            }
        }

        public bool IsFinished => Time <= 0;
        
        public void Reset() => Time = initialTime;
        
        public void Reset(float newTime) {
            initialTime = newTime;
            Reset();
        }
    }
    
    public class StopwatchTimer : TimerUtils {
        public StopwatchTimer() : base(0) { }

        public override void Tick(float deltaTime) {
            if (IsRunning) {
                Time += deltaTime;
            }
        }
        
        public void Reset() => Time = 0;
        
        public float GetTime() => Time;
    }
}