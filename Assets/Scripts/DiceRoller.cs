using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    static Rigidbody rb;
    static GameObject gO;
    static Vector3 diceSpeed;

    static DiceState diceState;
    enum DiceState {
        moving,
        stopped,
        standby
    }

    static Vector3 STARTING_POSITION = new Vector3 (0, 3, 0);
    static float STOPPED_SPEED = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gO = gameObject;
        // this is to turn off gravity for the dice in the standby position
        rb.isKinematic = true;
        gO.transform.position = STARTING_POSITION;
        diceState = DiceState.standby;
    }

    // Update is called once per frame
    void Update()
    {
        diceSpeed = rb.velocity;

        // based on the velocity and position of the dice, we can define 3 unique states
        // STANDBY, MOVING, STOPPED
        if (gO.transform.position == STARTING_POSITION && diceSpeed.magnitude <= STOPPED_SPEED) {
            diceState = DiceState.standby;
        }
        else if (diceSpeed.magnitude > STOPPED_SPEED || gO.transform.position.y > STARTING_POSITION.y) {
            diceState = DiceState.moving;
        }
        else {
            diceState = DiceState.stopped;
        }

        if (Input.GetKeyDown (KeyCode.Space)) {
            // if the dice is MOVING or STOPPED, pressing the space button should reset the dice to the STANBY state
            if (diceState == DiceState.moving || diceState == DiceState.stopped) {
                gO.transform.position = STARTING_POSITION;
                rb.velocity = new Vector3 (0, 0, 0);
                rb.isKinematic = true;
                diceState = DiceState.standby;
            }
            // if the dice is in STANDBY, pressing the space button initiate a dice roll
            else {
                gO.transform.position = STARTING_POSITION;
                rb.velocity = new Vector3 (0, 0, 0);
                // turn gravity on
                rb.isKinematic = false;

                float dirX = Random.Range (0, 500);
                float dirY = Random.Range (0, 500);
                float dirZ = Random.Range (0, 500);
                transform.rotation = Quaternion.identity;
                rb.AddForce(transform.up * 300);
                rb.AddTorque(dirX, dirY, dirZ);
            }
        }

        if (diceState == DiceState.stopped) {
            Ray ray = new Ray(gO.transform.position, Vector3.up);
            RaycastHit hitData;
            Physics.Raycast(ray, out hitData);
            Debug.Log(hitData.collider.gameObject.name + " was hit with an upwards raycast!");
        }
    }
}
