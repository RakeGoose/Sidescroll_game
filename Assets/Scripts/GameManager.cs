using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject playerPrefab;
    private GameObject playerInstance;

    void Start()
    {
        SpawnPlayer();
    }

    
    void SpawnPlayer()
    {
        Transform spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

        if(spawnPoint != null && playerPrefab != null)
        {
            playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Error: SpawnPoint or PlayerPrefab doesn't exists");
        }
    }
}
