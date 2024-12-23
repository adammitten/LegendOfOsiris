using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public string[] spawnPointNames;
 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadSceneAndSpawnPlayer());
        }
    }

    private IEnumerator LoadSceneAndSpawnPlayer()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }


        bool spawnPointFound = false;
        foreach (string spawnPointName in spawnPointNames)
        {
            GameObject spawnPoint = GameObject.Find(spawnPointName);

            if (spawnPoint != null)
            {
                GameObject player = GameObject.FindWithTag("Player");

                if (player != null)
                {
                    player.transform.position = spawnPoint.transform.position;
                }

                spawnPointFound = true;
                break;
            }



            else
            {
                Debug.LogError("SpawnPoint1 not found in scene!");
            }
        }
    }
}
