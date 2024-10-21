using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMovePlayer : MonoBehaviour
{
    public InputAction moveDown;
    public InputAction moveUp;
    public InputAction moveForward;
    public InputAction moveBackward;
    public InputAction moveLeft;
    public InputAction moveRight;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        moveDown.Enable();
        moveUp.Enable();
        moveLeft.Enable();
        moveRight.Enable();
        moveForward.Enable();
        moveBackward.Enable();

    }

    private void Update()
    {
        if (moveDown.IsInProgress())
        {
            MoveDown();
        }
        if (moveUp.IsInProgress())
        {
            MoveUp();
        }
        if (moveLeft.IsInProgress())
        {
            MoveLeft();
        }
        if (moveRight.IsInProgress())
        {
            MoveRight();
        }
        if (moveForward.IsInProgress())
        {
            MoveForward();
        }
        if (moveBackward.IsInProgress())
        {
            MoveBackward();
        }
    }

    void MoveDown()
    {
        transform.position -= transform.up * speed;
        Debug.Log("a");
    }
    void MoveUp()
    {
        transform.position += transform.up * speed;
        Debug.Log("b");
    }
    void MoveLeft()
    {
        transform.position -= transform.right * speed;
        Debug.Log("c");
    }
    void MoveRight()
    {
        transform.position += transform.right * speed;
        Debug.Log("ds");
    }
    void MoveBackward()
    {
        transform.position -= transform.forward * speed;
        Debug.Log("ds");
    }
    void MoveForward()
    {
        transform.position += transform.forward * speed;
        Debug.Log("c");
    }
}
