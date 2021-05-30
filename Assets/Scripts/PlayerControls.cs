using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(ActorActions))]
public class PlayerControls : MonoBehaviour
{
    private ActorActions actions;

    // Start is called before the first frame update
    void Start()
    {
        actions = GetComponent<ActorActions>();
    }

    // Update is called once per frame
    void Update()
    {

        if (actions.CanMove)
        {
            if (Input.GetKey(KeyCode.D))
            {
                actions.Move(Direction.Right);
            }
            if (Input.GetKey(KeyCode.W))
            {
                actions.Move(Direction.Up);
            }
            if (Input.GetKey(KeyCode.A))
            {
                actions.Move(Direction.Left);
            }
            if (Input.GetKey(KeyCode.S))
            {
                actions.Move(Direction.Down);
            }
        }
    }

}
