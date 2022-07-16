using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public GameObject blankTile;

    GameObject[,] map;
    List<GameObject> toBeSelected;

    void Start() {
        // Instantiate the grid
        map = new GameObject[width,height];
        toBeSelected = new List<GameObject>();
        for(int z = 0; z < height; ++z) {
            for(int x = 0; x < width; ++x) {
                // Create a new blank tile copy and position it
                var newTile = Instantiate(blankTile, transform);
                newTile.transform.localPosition = new Vector3(
                    (2f * x + 1f - width) / 2f,
                    0,
                    (2f * z + 1f - height) / 2f
                );
                // Add the new tile to the map
                map[x,z] = newTile;
                newTile.GetComponent<Tile>().setLocation(x, z);
                // Prepare a list of tiles that still need to be collapsed
                toBeSelected.Add(newTile);
            }
        }

        // Start collapsing the tiles
        while(toBeSelected.Count > 0) {
            collapse();
        }
    }

    public void collapse() {
        // Pick a random un-collapsed tile
        var current = toBeSelected[Random.Range(0, toBeSelected.Count)];

        // Pick a random valid option
        var currentTile = current.GetComponent<Tile>();
        currentTile.selectOption();
        propagateSelection(currentTile.x, currentTile.z);

        // Remove this tile from the working list
        toBeSelected.Remove(current);
    }

    public void propagateSelection(int x, int z) {

    }
}
