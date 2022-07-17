using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceFaceChanger : MonoBehaviour
{
    public GameObject Dice;
    public Sprite[] spriteList;
    private Keyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // this feature just cycles all of the sprites of the dice faces when the up arrow is pressed
        // it's just a proof of concept for changing the faces of the die individually
        keyboard = Keyboard.current;
        if (keyboard.upArrowKey.wasPressedThisFrame) {
            foreach (Transform child in Dice.transform) {
                SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
                for (int i = 0; i < spriteList.Length; i++) {
                    if (spriteList[i].name == spriteRenderer.sprite.name) {
                        spriteRenderer.sprite = spriteList[(i + 1) % spriteList.Length];
                        break;
                    }
                }
            }
        }
    }
}
