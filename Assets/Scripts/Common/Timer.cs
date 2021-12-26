using System;

public class Timer
{
    private long _waitTimeTicks;
    private long _startTime;

    public Timer(float seconds) : this(TimeSpan.FromSeconds(seconds)) { }

    public Timer(TimeSpan waitTime)
    {
        _waitTimeTicks = waitTime.Ticks;
        _startTime = DateTime.UtcNow.Ticks;
    }

    public bool IsElapsed => DateTime.UtcNow.Ticks > _startTime + _waitTimeTicks;

    public void Reset() => _startTime = DateTime.UtcNow.Ticks;

    public bool ResetIfElapsed()
    {
        var isElapsed = IsElapsed;
        if (isElapsed)
        {
            Reset();
        }

        return isElapsed;
    }
}
