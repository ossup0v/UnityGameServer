using System;

public class Timer
{
    private long _waitTimeTicks;
    private long _startTime;
    private long _endTime => _startTime + _waitTimeTicks;

    public Timer(float waitSeconds) : this(TimeSpan.FromSeconds(waitSeconds)) { }

    public Timer(TimeSpan waitTime)
    {
        _waitTimeTicks = waitTime.Ticks;
        _startTime = DateTime.UtcNow.Ticks;
    }

    public bool IsElapsed => DateTime.UtcNow.Ticks > _endTime;

    public void Reset() => _startTime = DateTime.UtcNow.Ticks;

    public long GetRemainingTimeTicks() => Math.Max(0, _endTime - DateTime.UtcNow.Ticks);

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
