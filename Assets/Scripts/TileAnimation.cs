using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimation : MonoBehaviour
{
    bool isVisible = false;
    GameObject player;
    float animationTime = 0.3f;
    Color invisibleColor = new Color(1, 1, 1, 0);
    Color visibleColor = new Color(1, 1, 1, 1);

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
            Debug.Log($"Making {gameObject.name} visible");
            isVisible = true;
            makeVisible();
        }
    }

    private void OnTriggerExit(Collider col) {
        if(col.gameObject.tag == "Player") {
            Debug.Log($"Making {gameObject.name} invisible");
            isVisible = false;
            makeInvisible();
        }
    }

    List<GameObject> findChildren() {
        List<GameObject> children = new List<GameObject>();
        for(int i = 0; i < transform.childCount; ++i) {
            children.Add(transform.GetChild(i).gameObject);
        }
        return children;
    }

    void makeVisible() {
        isVisible = true;
        var children = findChildren();
        StartCoroutine(animateVisible(children));
    }

    void makeInvisible() {
        isVisible = false;
        var children = findChildren();
        StartCoroutine(animateInvisible(children));
    }

    IEnumerator animateInvisible(List<GameObject> visuals) {
        float timer = 0;
        while(timer <= animationTime) {
            timer += Time.deltaTime;
            foreach(var go in visuals) {
                go.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.down, timer / animationTime);
                go.GetComponent<SpriteRenderer>().color = Color.Lerp(visibleColor, invisibleColor, timer / animationTime);
            }
            yield return null;
        }
        foreach(var go in visuals) {
            go.transform.localPosition = Vector3.down;
            go.GetComponent<SpriteRenderer>().color = invisibleColor;
        }
        yield return null;
    }

    IEnumerator animateVisible(List<GameObject> visuals) {
        float timer = 0;
        while(timer <= animationTime) {
            timer += Time.deltaTime;
            foreach(var go in visuals) {
                go.transform.localPosition = Vector3.Lerp(Vector3.down, Vector3.zero, timer / animationTime);
                go.GetComponent<SpriteRenderer>().color = Color.Lerp(invisibleColor, visibleColor, timer / animationTime);
            }
            yield return null;
        }
        foreach(var go in visuals) {
            go.transform.localPosition = Vector3.zero;
            go.GetComponent<SpriteRenderer>().color = visibleColor;
        }
        yield return null;
    }
}
