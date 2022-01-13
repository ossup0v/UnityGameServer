using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private readonly Queue<GameStage> _gameStages = new Queue<GameStage>();
    private GameStage _currentStage;
    private bool _isStagesOver => _currentStage == null;

    public static Game Instance { get; } = new Game(new List<GameStage>
    {
        new GameStage(1, GameConstants.StageTime, GameConstants.StageTimeNotify, () =>
        {
            Debug.Log("Stage PVE is started!");
        }, () =>
        {
            MapSaveManager.Instance.DestroyItems();
            Debug.Log("Stage PVE is ended!");
        }),

        new GameStage(2, GameConstants.StageTime, GameConstants.StageTimeNotify, () =>
        {
            Debug.Log("Stage PVP is started!");
        }, () =>
        {
            Debug.Log("Stage PVP is ended!");
        }),
    });

    public Game(List<GameStage> gameStages)
    {
        foreach (var stage in gameStages)
            _gameStages.Enqueue(stage);

        _currentStage = _gameStages.Dequeue();
    }

    public void Start()
    {
        _currentStage.Start();
    }

    public void Tick()
    {
        if (!_isStagesOver)
        {
            if (_currentStage.IsTimeToEnd)
            {
                SwitchToNextGameStage();
            }

            _currentStage?.Tick();
        }
    }

    private void SwitchToNextGameStage()
    {
        if (_gameStages.Count > 0)
        {
            var nextStage = _gameStages.Dequeue();
            _currentStage.ChangeToNextStage(nextStage);
            _currentStage = nextStage;
        }
        else
        {
            _currentStage.ChangeToNextStage();
            _currentStage = null;

            Debug.LogError("Stages is over, can't switch to next!");
        }
    }

}