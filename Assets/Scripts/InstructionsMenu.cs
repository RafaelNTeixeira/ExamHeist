using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Class responsible for the Instructions screen
public class InstructionsMenu : MonoBehaviour
{
    public Button backButton;
    
    private Button[] buttons;
    private int selectedIndex = 0;

    private void Start()
    {
        buttons = new Button[] { backButton };
        UpdateSelection();

        backButton.onClick.AddListener(GoBack);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (selectedIndex)
            {
                case 0:
                    backButton.onClick.AddListener(GoBack);
                    break;
            }
        }
    }

    private void UpdateSelection()
    {
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }

    public void GoBack()
    {
        GameManager.instance.SetGameState(GameManager.GameState.MainMenu);
    }
}
