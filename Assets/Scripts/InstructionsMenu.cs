using UnityEngine;
using UnityEngine.UI;

// Class responsible for the Instructions screen
public class InstructionsMenu : MonoBehaviour
{
    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(GoBack);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            backButton.onClick.AddListener(GoBack);
        }
    }

    public void GoBack()
    {
        GameManager.instance.SetGameState(GameManager.GameState.MainMenu);
    }
}
