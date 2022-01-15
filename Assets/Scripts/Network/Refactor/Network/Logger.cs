using UnityEngine;

public static class Logger
{
    public static void WriteLog(string caller, string msg)
    {
        Debug.Log("Log: " + caller + ": " + msg);
    }

    public static void WriteError(string caller, string msg)
    {
        Debug.LogError("Error: " + caller + ": " + msg);
    }

    public static void WriteWarning(string caller, string msg)
    {
        Debug.LogWarning("Warning: " + caller + ": " + msg);
    }
}