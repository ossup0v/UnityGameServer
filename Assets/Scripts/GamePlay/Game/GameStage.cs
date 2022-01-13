using System;

public class GameStage
{
    public readonly Action ActionOnStageStart;
    public readonly Action ActionOnStageEnd;
    public readonly int StageId;
    private readonly Timer _stageTimer;
    private Timer _stageTimerNotifyTimer = new Timer(GameConstants.StageTimeNotify);

    public GameStage(int stageId, TimeSpan duration, TimeSpan notifyDelay, Action actionOnStageStart, Action actionOnStageEnd)
    {
        StageId = stageId;

        _stageTimer = new Timer(duration);
        _stageTimerNotifyTimer = new Timer(notifyDelay);

        ActionOnStageStart = actionOnStageStart;
        ActionOnStageEnd = actionOnStageEnd;
    }

    public bool IsTimeToEnd => _stageTimer.IsElapsed;

    public void ChangeToNextStage(GameStage nextStage = null)
    {
        ActionOnStageEnd();

        if (nextStage != null)
        {
            nextStage.Start();
        }
    }

    public void Start()
    {
        _stageTimer.Reset();
        _stageTimerNotifyTimer.Reset();

        RoomSendClient.StageChanged(StageId);
        ActionOnStageStart();
    }

    public void Tick()
    {
        if (_stageTimerNotifyTimer.ResetIfElapsed())
        {
            RoomSendClient.StageTime(_stageTimer.GetRemainingTimeTicks());
        }
    }
}