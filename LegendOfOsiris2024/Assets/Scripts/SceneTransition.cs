using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    public Vector3 newPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadNewScene());
        }
    }

    private IEnumerator LoadNewScene()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);


        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SetPlayerPosition();
    }

    private void SetPlayerPosition()
    {
           
         GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = newPosition;  
            }
            else
            {
                Debug.LogWarning("Player object not found in the scene.");
            }
    }
    
}
