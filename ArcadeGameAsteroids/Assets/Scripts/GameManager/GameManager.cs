using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Core
{
    [CreateAssetMenu(fileName = "GameManager", menuName = "Managers/GameManager")]
    public class GameManager : ManagerBase, IAwake
    {
        public string MainObjectName = "[Sys_Core]";

        [Space(10)]
        [Header("UI и камера")]
        public Camera GameCameraPrefab;
        public GameObject GameCanvasPrefab;
        public GameObject LobbyCanvasPrefab;

        [Space(10)]
        [Header("Префаб игрока")]
        public GameObject PlayerPrefab;
        public GameObject PlayerBulletPrefab;
        public int StartLifeCount = 3;

        [Space(10)]
        [Header("Префабы астероидов")]
        public int StartAsteroidsCount = 2;
        public GameObject BigAsteroidPrefab;
        public GameObject MidSateroidPrefab;
        public GameObject SmallAteroidPrefab;

        [Space(10)]
        [Header("UFO префаб")]
        public GameObject UfoPrefab;
        public GameObject UfoBulletPrefab;
        [Range(0, 40)] public float MinAppearanceUFO = 20;
        [Range(0, 100)] public float MaxAppearanceUFO = 40;

        [Space(10)]
        private GameObject main;
        private GameController controller;


        [HideInInspector] public float sceneRightEdge;
        [HideInInspector] public float sceneLeftEdge;
        [HideInInspector] public float sceneTopEdge;
        [HideInInspector] public float sceneBottomEdge;
        [HideInInspector] public float sceneWidth;
        [HideInInspector] public float sceneHeight;

        public void OnAwake()
        {
            Debug.Log("GameManager active");
            main = GameObject.Find("[Sys_Core]") ?? new GameObject("[Sys_Core]");
            controller = main.AddComponent<GameController>();
            controller.Init();

        }

        public void StartGame()
        {
            controller.NewGame();
        }

       public GameObject GetPlayer()
       {
            return controller.player;
       }

        public void CreateAsteroid(int num, GameObject type, Vector3 pos)
        {
            controller.SpawnAsteroids(num ,type ,pos);
        }

        public GameObject CreateAsteroid(GameObject type, Vector3 pos)
        {
            return controller.CreateAsteroid(type, pos);
        }

        public GameState GetGameState()
        {
            return controller.state;
        }

        public void PlayerInput(InputKeys key)
        {
            controller.PlayerInput(key);
        }

        public void PlayerInput(float angle)
        {
            controller.PlayerInput(angle);
        }

        public void GameMenu()
        {
            controller.GameMenu();
        }

        public void Quit()
        {
            controller.Quit();
        }

        public void RemoveAsteroid(int score, GameObject obj)
        {
            controller.RemoveAsteroid(score, obj);
        }

        public void NewGame()
        {
            controller.NewGame();
        }

        public void TakeLife()
        {
            controller.TakeLife();
        }

        public void DeadUfo(int score)
        {
            controller.DeadUfo(score);
        }

        public void SetInput(InputType type)
        {
            controller.SetInput(type);
        }

        public void AddAsteroid(int count)
        {
            controller.AddAsteroid(count);
        }
    }
}
