using UnityEngine;
using UnityEngine.SceneManagement;

// Class to change the cutscene after a certain time
public class ChangeSceneOnTimer : MonoBehaviour
{
    [SerializeField] private float changeTime;

    private void Update()
    {
        changeTime -= Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Space)) {
            GameManager.instance.canPauseGame = true;
            SceneManager.LoadScene("GameScene");
        }
        
        if (changeTime <= 0) 
        {
            GameManager.instance.canPauseGame = true;
            SceneManager.LoadScene("GameScene");
        } 
    }
}
