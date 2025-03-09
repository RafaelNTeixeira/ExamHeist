using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTimer : MonoBehaviour
{
    [SerializeField] private float changeTime;

    private void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0) 
        {
            SceneManager.LoadScene("GameScene");
        } 
    }
}
