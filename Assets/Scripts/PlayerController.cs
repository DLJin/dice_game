using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    bool canMove = true;
    public GameObject visuals;
    public float moveDuration = 0.5f;
    public float jumpHeight = 0.3f;
    Keyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        keyboard = Keyboard.current;
        if(canMove) {
            if (keyboard.upArrowKey.wasPressedThisFrame) {
                movePlayer(0, 1);
            } else if (keyboard.downArrowKey.wasPressedThisFrame) {
                movePlayer(0, -1);
            } else if (keyboard.rightArrowKey.wasPressedThisFrame) {
                movePlayer(1, 0);
            } else if (keyboard.leftArrowKey.wasPressedThisFrame) {
                movePlayer(-1, 0);
            }
        }
    }

    void movePlayer(int h, int v) {
        canMove = false;
        StartCoroutine(animateMovement(h, v));
        transform.position = new Vector3(transform.position.x + h, 0, transform.position.z + v);
    }

    IEnumerator animateMovement(int h, int v) {
        float moveTimer = 0;
        var currPos = transform.position;
        var finalPos = new Vector3(transform.position.x + h, 0, transform.position.z + v);
        while (moveTimer < moveDuration) {
            moveTimer += Time.deltaTime;
            var newPos = Vector3.Lerp(currPos, finalPos, moveTimer/moveDuration);
            newPos.y = Mathf.Sin(Mathf.PI * moveTimer / moveDuration) * jumpHeight;
            transform.position = newPos;
            yield return null;
        }
        transform.position = finalPos;
        canMove = true;
        yield return null;
    }
}
