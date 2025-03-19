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
        // Check if the player is pressing the return key to go back to the main menu
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

    // Function to update the selected button
    private void UpdateSelection()
    {
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }

    // Function to go back to the main menu
    public void GoBack()
    {
        GameManager.instance.SetGameState(GameManager.GameState.MainMenu);
    }
}
