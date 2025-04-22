using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen; 

    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(GoToScreneRoutine(sceneIndex));
    }
    IEnumerator GoToScreneRoutine(int sceneIndex)
    {
        fadeScreen.FadeOut(); 
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

        //Launch the new scene
        SceneManager.LoadScene(sceneIndex);
    }
}
