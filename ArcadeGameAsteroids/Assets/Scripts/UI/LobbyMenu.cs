using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class LobbyMenu : MonoBehaviour
{
    public GameObject ButtonContinue;
    public GameObject ButtonStartGame;
    public GameObject ChangeInput;
    public GameObject Exit;


    private Text changeButtonText;
    private GameManager gameManager;

    private string InputTextKeyboard = "Управление (Клавиатура)" ;
    private string InputTextKeyboardAndMouse = "Управление (Клавиатура + мышь)";

    private bool KeyboardOnly = false;

    public void Init()
    {
        gameManager = CoreTools.GetManager<GameManager>();
        changeButtonText = ChangeInput.GetComponentInChildren<Text>();
    }

    public void OnEnable()
    {
        if(gameManager != null)
        {
            if (gameManager.GetGameState() == GameState.Pause)
            {
                ButtonContinue.SetActive(true);
            }
            else
            {
                ButtonContinue.SetActive(false);
            }
        }

    }

    public void ContinueButtonDown()
    {
        gameManager.GameMenu();
    }

    public void StartGameButtonDown()
    {
        gameManager.StartGame();
    }

    public void ChangeInputButtonDown()
    {
        if (KeyboardOnly)
        {
            changeButtonText.text = InputTextKeyboard;
            KeyboardOnly = false;
            gameManager.SetInput(InputType.keyboard);
        }
        else
        {
            changeButtonText.text = InputTextKeyboardAndMouse;
            KeyboardOnly = true;
            gameManager.SetInput(InputType.keyboardAndMouse);
        }
    }

    public void ExitButtonDown()
    {
        gameManager.Quit();
    }
}
