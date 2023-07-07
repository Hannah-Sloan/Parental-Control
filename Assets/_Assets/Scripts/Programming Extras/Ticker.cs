using System.Collections.Generic;
using UnityEngine;

public class Ticker : Singleton<Ticker>
{
    private List<ITickable> tickables = new List<ITickable>();

    public void Register(ITickable tickable) 
    {
        if(tickable == null) return;
        if (tickables.Contains(tickable)) return;
        tickables.Add(tickable);
    }

    public void Remove(ITickable tickable) 
    {
        tickables.Remove(tickable);
    }

    private void Update()
    {
        tickables.ForEach(tickable => tickable.Tick(Time.deltaTime));
    }
}
