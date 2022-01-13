using UnityEngine;

public class UnityGame : MonoBehaviour
{
    public static UnityGame Instance;

    private void Awake()
    {
        Instance = this;
    }

    private bool _isStarted = false;

    public void StartGame()
    {
        _isStarted = true;

        Game.Instance.Start();
    }

    private void FixedUpdate()
    {
        if (!_isStarted) return;

        Game.Instance.Tick();
    }
}
