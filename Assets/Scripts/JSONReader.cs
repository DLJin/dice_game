using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour {
    public TextAsset jsonFile;
    public MapConfig mapConfig;

    [System.Serializable]
    public class MapConfig {
        public NeighborOptions[] options;
    }

    [System.Serializable]
    public class NeighborOptions {
        public string tile;
        public DirectionOptions[] up;
        public DirectionOptions[] right;
        public DirectionOptions[] down;
        public DirectionOptions[] left;
    }

    [System.Serializable]
    public class DirectionOptions {
        public string tile;
        public float probability;
    }

    void Start() {
        mapConfig = JsonUtility.FromJson<MapConfig>(jsonFile.text);
        // foreach (NeighborOptions option in mapConfig.options) {
        //     Debug.Log($"Got options for {option.tile}");
        // }
    }
}