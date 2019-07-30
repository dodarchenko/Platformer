using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingPlayer : MonoBehaviour { 

    public float distance;

    // Start is called before the first frame update
    void Start()
    {
    Physics2D.queriesStartInColliders = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance);
        Debug.DrawLine(transform.position, transform.position + transform.right * distance, Color.red);
    }
}
