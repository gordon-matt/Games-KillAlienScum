using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnEnter : MonoBehaviour
{
    public string SceneToLoad = "Play";

    private void Update()
    {
        //Debug.Log("Update...");
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //Debug.Log("Trying to load scene...");
            FindObjectOfType<ProgressSceneLoader>().LoadScene(SceneToLoad);
        }
    }
}