using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    public float Frequency = 1f;

    private void Start()
    {
        StartCoroutine(SpawnBot());
    }

    private IEnumerator SpawnBot()
    {
        yield return new WaitForSeconds(Frequency);

        if (BotManager.GetBotCount() <= BotManager.MaxBotCount)
        {
            NetworkManager.Instance.InstantiateBot(transform.position);
        }

        StartCoroutine(SpawnBot());
    }
}
