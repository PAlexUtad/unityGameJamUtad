using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAndGameOver : MonoBehaviour
{
    public enum CollisionType { Win, Lose }
    public CollisionType collisionOption;
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collisionOption == CollisionType.Win)
        {
            print("W");
        }
        else if (collisionOption == CollisionType.Lose)
        {
            print("L");
        }
    }

}
