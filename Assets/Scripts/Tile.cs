using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [System.Serializable]
    public struct TileOption {
        [System.Serializable, System.Flags]
        public enum TileId {
            Grass = 1,
            Water = 1 << 1,
            Sand = 1 << 2,
            Stone = 1 << 3,
            Ocean = 1 << 4,
            Forest = 1 << 5
        }

        public TileId id;
        public GameObject[] tile;
    }

    static Dictionary<TileOption.TileId, Dictionary<int, int>> selectionHistory = new Dictionary<TileOption.TileId, Dictionary<int, int>>();

    public List<TileOption> tileOptions;
    [Range(0f, 1f)]
    public float padding = 0.1f;
    public bool isSet = false;
    public TileOption.TileId id;
    public int x, z;
    MapMaker map;

    public static Dictionary<String, TileOption.TileId> nameToId;
    public static TileOption.TileId convertNameToId(string name) {
        if(nameToId == null) {
            nameToId = new Dictionary<string, TileOption.TileId>();
            nameToId.Add("Grass", TileOption.TileId.Grass);
            nameToId.Add("Water", TileOption.TileId.Water);
            nameToId.Add("Sand", TileOption.TileId.Sand);
            nameToId.Add("Stone", TileOption.TileId.Stone);
            nameToId.Add("Ocean", TileOption.TileId.Ocean);
            nameToId.Add("Forest", TileOption.TileId.Forest);
        }

        if(nameToId.ContainsKey(name)) {
            return nameToId[name];
        }
        Debug.LogError($"Couldn't convert name {name} to a TileId, defaulting to Grass");
        return TileOption.TileId.Grass;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the map
        map = FindObjectOfType<MapMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setLocation(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public void selectOption() {
        if(tileOptions.Count == 0) {
            Debug.LogWarning($"Tile ({x}, {z}) doesn't have any valid options.");
        }

        // Select a random tile type amongst the valid choices
        var selected = tileOptions[UnityEngine.Random.Range(0, tileOptions.Count)];
        // This is where we can add some randomization later down the road, to insert different variants of the same tile type
        var variant = UnityEngine.Random.Range(0, selected.tile.Length);
        var visuals = Instantiate(selected.tile[variant], this.transform);
        visuals.transform.localPosition = Vector3.down;
        id = selected.id;
        isSet = true;
        GetComponent<TileAnimation>().findChildren();
        logSelection(selected.id, variant);

        //Debug.Log($"Selected {Enum.GetName(typeof(TileOption.TileId), selected.id)} for location ({x}, {z}).");
    }

    void logSelection(TileOption.TileId id, int variant) {
        if(!selectionHistory.ContainsKey(id)) {
            selectionHistory.Add(id, new Dictionary<int, int>());
        }
        if(!selectionHistory[id].ContainsKey(variant)) {
            selectionHistory[id].Add(variant, 0);
        }
        selectionHistory[id][variant] = selectionHistory[id][variant] + 1;
    }

    public static void printSelections() {
        string output = "";
        foreach(var id in selectionHistory.Keys) {
            output += $"{Enum.GetName(typeof(TileOption.TileId), id)}:\n";
            foreach(var variant in selectionHistory[id].Keys) {
                output += $"\tVariant {variant}: {selectionHistory[id][variant]}\n";
            }
        }
        Debug.Log(output);
    }
}
