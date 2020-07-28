using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Core 
{
    [DisallowMultipleComponent]
    public class InputPreset : MonoBehaviour, ITick
    {
        [Header("Клавиатура")]
        [SerializeField] KeyCode keyboardAcceleration;
        [SerializeField] KeyCode keyboardRight;
        [SerializeField] KeyCode keyboardLeft;
        [SerializeField] KeyCode keyboardFire;

        GameManager gameManager;
        
        void Start()
        {
            CoreTools.GetManager<Updater>().AddTo(this);
            gameManager = CoreTools.GetManager<GameManager>();
        }

        public void Tick()
        {
            PlayerInput();
        }

        public void PlayerInput()
        {
            if (!isActiveAndEnabled) return;

            if (Input.GetKeyDown(KeyCode.Escape) )
            {
                gameManager.GameMenu();
            }

            if (gameManager.GetGameState() != GameState.Process) return;

            if (Input.GetKey(keyboardAcceleration))
            {
                gameManager.PlayerInput(InputKeys.up);
            }
            if (Input.GetKey(keyboardRight))
            {
                gameManager.PlayerInput(InputKeys.right);
            }
            if (Input.GetKey(keyboardLeft))
            {
                gameManager.PlayerInput(InputKeys.left);
            }
            if (Input.GetKeyDown(keyboardFire))
            {
                gameManager.PlayerInput(InputKeys.shoot);
            }
        }
    }
}
