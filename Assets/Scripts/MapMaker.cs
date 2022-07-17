using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MapMaker : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public GameObject blankTile;

    GameObject[,] map;
    List<GameObject> toBeSelected;

    JSONReader neighbors;

    void Start() {
        neighbors = GetComponent<JSONReader>();

        // Instantiate the grid
        map = new GameObject[width,height];
        toBeSelected = new List<GameObject>();
        for(int z = 0; z < height; ++z) {
            for(int x = 0; x < width; ++x) {
                // Create a new blank tile copy and position it
                var newTile = Instantiate(blankTile, transform);
                // newTile.transform.localPosition = new Vector3(
                //     (2f * x + 1f - width) / 2f,
                //     0,
                //     (2f * z + 1f - height) / 2f
                // );
                newTile.transform.localPosition = new Vector3(x, 0, z);
                // Add the new tile to the map
                map[x,z] = newTile;
                newTile.GetComponent<Tile>().setLocation(x, z);
                newTile.name = $"{x}, {z}";
                // Prepare a list of tiles that still need to be collapsed
                toBeSelected.Add(newTile);
            }
        }

        // Start collapsing the tiles
        while(toBeSelected.Count > 0) {
            collapse();
        }

        Tile.printSelections();
    }

    public void collapse() {
        // Pick a random un-collapsed tile
        var current = toBeSelected[UnityEngine.Random.Range(0, toBeSelected.Count)];

        // Pick a random valid option
        var currentTile = current.GetComponent<Tile>();
        currentTile.selectOption();
        propagateSelection(currentTile.x, currentTile.z);

        // Remove this tile from the working list
        toBeSelected.Remove(current);
    }

    public void propagateSelection(int x, int z) {
        Queue<Tile> toBePropagated = new Queue<Tile>();
        HashSet<Vector2> alreadyVisited = new HashSet<Vector2>();
        toBePropagated.Enqueue(map[x, z].GetComponent<Tile>());

        while(toBePropagated.Count > 0) {
            var current = toBePropagated.Dequeue();
            alreadyVisited.Add(new Vector2(current.x, current.z));

            if(current.x - 1 >= 0) {
                var nextTile = map[current.x - 1, current.z].GetComponent<Tile>();
                var nextCoord = new Vector2(nextTile.x, nextTile.z);
                if(nextTile.isSet) {
                    alreadyVisited.Add(nextCoord);
                } else {
                    propagateHelper(current, "left", ref nextTile);
                    if(!alreadyVisited.Contains(nextCoord)) {
                        toBePropagated.Enqueue(nextTile);
                    }
                }
            }

            if(current.x + 1 < width) {
                var nextTile = map[current.x + 1, current.z].GetComponent<Tile>();
                var nextCoord = new Vector2(nextTile.x, nextTile.z);
                if(nextTile.isSet) {
                    alreadyVisited.Add(nextCoord);
                } else {
                    propagateHelper(current, "right", ref nextTile);
                    if(!alreadyVisited.Contains(nextCoord)) {
                        toBePropagated.Enqueue(nextTile);
                    }
                }
            }

            if(current.z - 1 >= 0) {
                var nextTile = map[current.x, current.z - 1].GetComponent<Tile>();
                var nextCoord = new Vector2(nextTile.x, nextTile.z);
                if(nextTile.isSet) {
                    alreadyVisited.Add(nextCoord);
                } else {
                    propagateHelper(current, "down", ref nextTile);
                    if(!alreadyVisited.Contains(nextCoord)) {
                        toBePropagated.Enqueue(nextTile);
                    }
                }
            }

            if(current.z + 1 < height) {
                var nextTile = map[current.x, current.z + 1].GetComponent<Tile>();
                var nextCoord = new Vector2(nextTile.x, nextTile.z);
                if(nextTile.isSet) {
                    alreadyVisited.Add(nextCoord);
                } else {
                    propagateHelper(current, "up", ref nextTile);
                    if(!alreadyVisited.Contains(nextCoord)) {
                        toBePropagated.Enqueue(nextTile);
                    }
                }
            }
        }
    }

    void propagateHelper(Tile current, string direction, ref Tile next) {
        // Don't do anything if this tile has already been set
        if(next.isSet) {
            return;
        }

        // Collect the list of valid options for the next tile given the current tile's possible types and direction
        Dictionary<Tile.TileOption.TileId, HashSet<Tile.TileOption.TileId>> options;
        switch(direction) {
            case "up":
                options = neighbors.upOptions;
                break;
            case "right":
                options = neighbors.rightOptions;
                break;
            case "down":
                options = neighbors.downOptions;
                break;
            case "left":
                options = neighbors.leftOptions;
                break;
            default:
                options = new Dictionary<Tile.TileOption.TileId, HashSet<Tile.TileOption.TileId>>();
                Debug.LogError($"propagateHelper received invalid direction {direction}");
                break;
        }

        HashSet<Tile.TileOption.TileId> validList;
        if(current.isSet) {
            validList = options[current.id];
        } else {
            validList = new HashSet<Tile.TileOption.TileId>();
            foreach(var candidate in current.tileOptions) {
                foreach(var o in options[candidate.id]) {
                    validList.Add(o);
                }
            }
        }

        // validList is the list of ids that are valid options for the next tile
        var newOptionsList = new List<Tile.TileOption>();
        foreach(var o in next.tileOptions) {
            if(validList.Contains(o.id)) {
                newOptionsList.Add(o);
            }
        }
        if(newOptionsList.Count == 0) {
            var currentTile =  Enum.GetName(typeof(Tile.TileOption.TileId), current.id);
            var nextTileOptions = string.Join(", ", next.tileOptions.ConvertAll<string>(option => Enum.GetName(typeof(Tile.TileOption.TileId), option.id)));
            var validListOptions = string.Join(", ", validList.ToList().ConvertAll<string>(option => Enum.GetName(typeof(Tile.TileOption.TileId), option)));
            // Debug.LogWarning($"Propagating ({current.x}, {current.z}) to ({next.x}, {next.z}) would result in an empty tile.\nCurrent Tile: {currentTile} | Next Tile Options: {nextTileOptions} | Valid List: {validListOptions}");
        } else {
            next.tileOptions = newOptionsList;
        }
    }
}
