using UnityEngine;
using UnityEngine.SceneManagement;

// Class to change the cutscene after a certain time
public class ChangeSceneOnTimer : MonoBehaviour
{
    [SerializeField] private float changeTime;
    [SerializeField] private bool isFinalCutscene;

    private void Update()
    {
        changeTime -= Time.deltaTime;

        // Press the space key to skip the cutscene
        if (Input.GetKeyUp(KeyCode.Space)) {
            GameManager.instance.canPauseGame = true && !isFinalCutscene; // Can only pause the game after initial cutscene
            SceneManager.LoadScene(isFinalCutscene ? "MainMenu" : "GameScene");
        }
        
        if (changeTime <= 0) 
        {
            GameManager.instance.canPauseGame = true && !isFinalCutscene; // Can only pause the game after initial cutscene
            SceneManager.LoadScene(isFinalCutscene ? "MainMenu" : "GameScene");
        } 
    }
}
