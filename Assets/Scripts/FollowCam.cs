using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public float speed = 3.0f;
    public float zDistance = 10.0f;
    public float allowableOffset = 3.0f;
    public GameObject player;

    // Improvements to consider:
    // - Easing movement at start and end
    // - Catching up more quickly if player is too far from center


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position + Vector3.back * zDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position + Vector3.back * zDistance) > allowableOffset)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position + Vector3.back * zDistance, speed * Time.deltaTime);
        }
    }
}
