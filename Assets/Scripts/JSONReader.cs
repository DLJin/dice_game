using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JSONReader : MonoBehaviour {
    public TextAsset jsonFile;
    public Dictionary<Tile.TileOption.TileId, NeighborOptions> mapConfig;

    public Dictionary<Tile.TileOption.TileId, HashSet<Tile.TileOption.TileId>> upOptions;
    public Dictionary<Tile.TileOption.TileId, HashSet<Tile.TileOption.TileId>> rightOptions;
    public Dictionary<Tile.TileOption.TileId, HashSet<Tile.TileOption.TileId>> downOptions;
    public Dictionary<Tile.TileOption.TileId, HashSet<Tile.TileOption.TileId>> leftOptions;

    [System.Serializable]
    public class MapConfig {
        public NeighborOptions[] options;
    }

    [System.Serializable]
    public class NeighborOptions {
        public string tile;
        public OptionProbability[] up;
        public OptionProbability[] right;
        public OptionProbability[] down;
        public OptionProbability[] left;
    }

    [System.Serializable]
    public class OptionProbability {
        public string tile;
        public float probability;
    }

    void Awake() {
        Debug.Log("Awaking JSONReader");
        mapConfig = new Dictionary<Tile.TileOption.TileId, NeighborOptions>();
        upOptions = new Dictionary<Tile.TileOption.TileId, HashSet<Tile.TileOption.TileId>>();
        rightOptions = new Dictionary<Tile.TileOption.TileId, HashSet<Tile.TileOption.TileId>>();
        downOptions = new Dictionary<Tile.TileOption.TileId, HashSet<Tile.TileOption.TileId>>();
        leftOptions = new Dictionary<Tile.TileOption.TileId, HashSet<Tile.TileOption.TileId>>();
        foreach(var id in Enum.GetValues(typeof(Tile.TileOption.TileId))) {
            upOptions.Add((Tile.TileOption.TileId)id, new HashSet<Tile.TileOption.TileId>());
            rightOptions.Add((Tile.TileOption.TileId)id, new HashSet<Tile.TileOption.TileId>());
            downOptions.Add((Tile.TileOption.TileId)id, new HashSet<Tile.TileOption.TileId>());
            leftOptions.Add((Tile.TileOption.TileId)id, new HashSet<Tile.TileOption.TileId>());
        }

        var config = JsonUtility.FromJson<MapConfig>(jsonFile.text);
        foreach(var o in config.options) {
            mapConfig.Add(Tile.convertNameToId(o.tile), o);

            foreach(var p in o.up) {
                upOptions[Tile.convertNameToId(o.tile)].Add(Tile.convertNameToId(p.tile));
            }
            foreach(var p in o.right) {
                rightOptions[Tile.convertNameToId(o.tile)].Add(Tile.convertNameToId(p.tile));
            }
            foreach(var p in o.down) {
                downOptions[Tile.convertNameToId(o.tile)].Add(Tile.convertNameToId(p.tile));
            }
            foreach(var p in o.left) {
                leftOptions[Tile.convertNameToId(o.tile)].Add(Tile.convertNameToId(p.tile));
            }
        }
        // foreach (NeighborOptions option in mapConfig.options) {
        //     Debug.Log($"Got options for {option.tile}");
        // }
    }
}