using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{

    [DisallowMultipleComponent]
    public class MouseInputPreset : MonoBehaviour, ITick
    {
        [Header("Клавиатура + Мышь")]
        [SerializeField] KeyCode keyboardAcceleration;
        [SerializeField] KeyCode keyboardAcceleration2;
        [SerializeField] KeyCode keyboardAcceleration3;
        [SerializeField] KeyCode keyboardFire;
        [SerializeField] KeyCode keyboardFire2;

        GameManager gameManager;
        GameObject player;

        private Vector3 mousePosition;

        void Start()
        {
            CoreTools.GetManager<Updater>().AddTo(this);
            gameManager = CoreTools.GetManager<GameManager>();
            player = gameManager.GetPlayer();
        }

        public void Tick()
        {
            PlayerInput();
        }

        public void PlayerInput()
        {
            if (!isActiveAndEnabled) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameManager.GameMenu();
            }

            if (gameManager.GetGameState() != GameState.Process) return;

            if (Input.GetKey(keyboardAcceleration) || Input.GetKey(keyboardAcceleration2) || Input.GetKey(keyboardAcceleration3))
            {
                gameManager.PlayerInput(InputKeys.up);
            }
            if (Input.GetKeyDown(keyboardFire) || Input.GetKeyDown(keyboardFire2))
            {
                gameManager.PlayerInput(InputKeys.shoot);
            }

            gameManager.PlayerInput(CalculateAngle());
        }

        public float  CalculateAngle()
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float Angle = -Mathf.Atan2(mousePosition.z - player.transform.position.z, mousePosition.x - player.transform.position.x) / Mathf.PI * 180f;


            return Angle + 90; // +90 поправка с rihgt на forward
        }
    }
}
