using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTimer : MonoBehaviour
{
    [SerializeField] private float changeTime;

    private void Update()
    {
        changeTime -= Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Space)) {
            SceneManager.LoadScene("GameScene");
            GameManager.instance.canPauseGame = true;
        }
        
        if (changeTime <= 0) 
        {
            SceneManager.LoadScene("GameScene");
            GameManager.instance.canPauseGame = true;
        } 
    }
}
