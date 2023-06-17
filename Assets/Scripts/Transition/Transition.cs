using UnityEngine.SceneManagement;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public Animator anim;
    private string sceneName;

    public void PlayOutTransition(string _sceneName)
    {
        //Play Off transition and set the scene name
        anim.Play("Out");
        sceneName = _sceneName;
    }

    public void ChangeSceneTo_EVENT()
    {
        //Change scene (in event annimation)
        SceneManager.LoadScene(sceneName);
    }
}