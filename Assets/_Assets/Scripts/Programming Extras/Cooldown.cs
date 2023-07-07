using System;

public class Cooldown : ITickable
{
    private readonly float duration; //In seconds
    private float time = 0; //In seconds;

    public event Action Cool;

    /// <summary>
    /// Only manual cooldowns can be created from a Monobehaviour constructor. Use Start or Awake for non-manual cooldowns. 
    /// </summary>
    /// <param name="duration">In seconds</param>
    /// <param name="manualMode">Set to true to control when the remaining time is decreased through Tick</param>
    /// <param name="coolSubscribers">Any methods you would like to be subscribed to the cool event, triggered on the cooldown is cool.</param>
    /// <exception cref="System.ArgumentException">Thrown when duration is less than or equal to 0</exception>
    public Cooldown(float duration, bool manualMode = false, params Action[] coolSubscribers) 
    {
        if (duration <= 0) throw new System.ArgumentException("Duration must be greater than 0");
        this.duration = duration;
        if(!manualMode)
            Ticker.Instance.Register(this);
        foreach (var subsciber in coolSubscribers)
        {
            Cool += subsciber;
        }
    }

    /// <summary>
    /// Starts the cooldown
    /// </summary>
    public void Start() 
    {
        time = duration;
    }

    public void SetCool() 
    {
        time = 0;
        if (IsCool() && Cool != null) Cool.Invoke();
    }

    /// <summary>
    /// Only use this in manualMode to decrease the remaining time
    /// </summary>
    public void Tick(float deltaTime)
    {
        if(IsCool()) return;
        time -= deltaTime;
        if (IsCool() && Cool!=null) Cool.Invoke();
    }

    public bool IsCool()
    {
        return time <= 0;
    }

    /// <returns>Where 0 is full cool down time remaining, and 1 is totally cool</returns>
    public float NormalizedTime() 
    {
        if (IsCool()) return 1;
        if(time == duration) return 0;
        return Math.Min(Math.Max((duration - time) / duration, 0), 1);
    }
}
