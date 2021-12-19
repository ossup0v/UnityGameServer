using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BotBase : MonoBehaviour
{
    private int _id;
    private BotState _state;
    private Player _target;
    public CharacterController Controller;
    public Transform shootPosition;

    public void Initialize()
    { 
        _id = BotManager.GetNextId();
        BotManager.AddBot(_id, this);
    }

    public int GetId()
    {
        return _id;
    }
}
