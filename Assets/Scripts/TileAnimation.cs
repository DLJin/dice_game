using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimation : MonoBehaviour
{
    GameObject player;
    float animationTime = 0.3f;
    Color invisibleColor = new Color(1, 1, 1, 0);
    Color visibleColor = new Color(1, 1, 1, 1);
    GameObject floor;
    List<GameObject> decor;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Player") {
            // Debug.Log($"Making {gameObject.name} visible");
            makeVisible();
        }
    }

    private void OnTriggerExit(Collider col) {
        if(col.gameObject.tag == "Player") {
            // Debug.Log($"Making {gameObject.name} invisible");
            makeInvisible();
        }
    }

    public void findChildren() {
        floor = transform.GetChild(0).gameObject;
        decor = new List<GameObject>();
        for(int i = 0; i < floor.transform.childCount; ++i) {
            decor.Add(floor.transform.GetChild(i).gameObject);
        }
    }

    void makeVisible() {
        // findChildren();
        StartCoroutine(animateVisible());
    }

    void makeInvisible() {
        // findChildren();
        StartCoroutine(animateInvisible());
    }

    IEnumerator animateInvisible() {
        float timer = 0;
        while(timer <= animationTime) {
            timer += Time.deltaTime;
            floor.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.down, timer / animationTime);
            floor.GetComponent<SpriteRenderer>().color = Color.Lerp(visibleColor, invisibleColor, timer / animationTime);
            foreach(var go in decor) {
                go.GetComponent<SpriteRenderer>().color = Color.Lerp(visibleColor, invisibleColor, timer / animationTime);
            }
            yield return null;
        }
        floor.transform.localPosition = Vector3.down;
        floor.GetComponent<SpriteRenderer>().color = invisibleColor;
        foreach(var go in decor) {
            go.GetComponent<SpriteRenderer>().color = invisibleColor;
        }
        yield return null;
    }

    IEnumerator animateVisible() {
        float timer = 0;
        while(timer <= animationTime) {
            timer += Time.deltaTime;
            floor.transform.localPosition = Vector3.Lerp(Vector3.down, Vector3.zero, timer / animationTime);
            floor.GetComponent<SpriteRenderer>().color = Color.Lerp(invisibleColor, visibleColor, timer / animationTime);
            foreach(var go in decor) {
                go.GetComponent<SpriteRenderer>().color = Color.Lerp(invisibleColor, visibleColor, timer / animationTime);
            }
            yield return null;
        }
        floor.transform.localPosition = Vector3.zero;
        floor.GetComponent<SpriteRenderer>().color = visibleColor;
        foreach(var go in decor) {
            go.GetComponent<SpriteRenderer>().color = visibleColor;
        }
        yield return null;
    }
}
