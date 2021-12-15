using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build All")]
    public static void BuildAll()
    {
        BuildWindowsServer();
        BuildLinuxServer();
        BuildWindowsClient();
    }

    [MenuItem("Build/Build Server (Windows)")]
    public static void BuildWindowsServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Game.unity" };
        buildPlayerOptions.locationPathName = "Builds/Windows/Server/Server.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

        Debug.Log("Building Server (Windows)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log("Built Server (Windows).");
    }

    [MenuItem("Build/Build Server (Linux)")]
    public static void BuildLinuxServer()
    {
        
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Game.unity" };
        buildPlayerOptions.locationPathName = "Builds/Linux/Server/Server.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

        Debug.Log("Building Server (Linux)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log("Built Server (Linux).");
    }


    [MenuItem("Build/Build Client (Windows)")]
    public static void BuildWindowsClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Game.unity" };
        buildPlayerOptions.locationPathName = "Builds/Windows/Client/Client.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

        Debug.Log("Building Client (Windows)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log("Built Client (Windows).");
    }
}