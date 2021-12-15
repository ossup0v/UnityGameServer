using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MapSaveManager : MonoBehaviour 
{
    public static MapSaveManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"duplicated instances of {nameof(MapSaveManager)}");
            Destroy(this);
        }
    }

    private List<MapEntitySaveObject> Entities;
    private string CachedObjects;

    private void Start()
    {
        SaveAllObjectsToString();
    }

    public void SaveAllObjectsToString()
    {
        Entities = GameObject.FindObjectsOfType<MapEntitySaveObject>().ToList();
        SaveObjectsToString();
    }

    private void SaveObjectsToString()
    { 
        var sb = new StringBuilder();

        foreach (var entity in Entities)
        {
            sb.Append(entity.Serialize());
        }

        CachedObjects = sb.ToString();
    }

    public string GetCachedObjects()
    {
        return CachedObjects;
    }
}
