using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Core;

public sealed class GameController : MonoBehaviour
{
    private Camera gameCamera;

    private GameManager gameManager;
    private RegulatoryPools pool;
    private TimerManager timerManager;
    private InputManager inputManager;

    private GameObject gameCanvas;
    private GameUI gameUi;

    private GameObject lobbyCanvas;
    private LobbyMenu lobbyMenu;

    private Ship ship;
    private UFO ufo;

    private int score;
    private int lifeCount;
    private int asteroidsCount;
    private int startAsteroidCount;

    private GameObject[] asteroids;

    public GameState state { get; private set; }

    public GameObject player { get; private set; }
    public GameObject Ufo { get; private set; }
    
    public float sceneRightEdge { get; private set; }
    public float sceneLeftEdge { get; private set; }
    public float sceneTopEdge { get; private set; }
    public float sceneBottomEdge { get; private set; }
    public float sceneWidth { get; private set; }
    public float sceneHeight { get; private set; }

    public int Level { get; private set; }


    public void Init()
    {
        gameManager = CoreTools.GetManager<GameManager>();
        pool = CoreTools.GetManager<RegulatoryPools>();
        timerManager = CoreTools.GetManager<TimerManager>();
        inputManager = CoreTools.GetManager<InputManager>();

        FindCamera();
        DefiningScreenBoundaries();
        AddEventSystem();
        CreatePools();

        state = GameState.Start;

        gameCanvas = GameObject.Instantiate(gameManager.GameCanvasPrefab);
        gameUi = gameCanvas.GetComponent<GameUI>();
        gameUi.Init();
        gameCanvas.SetActive(false);
        
        lobbyCanvas = GameObject.Instantiate(gameManager.LobbyCanvasPrefab);
        lobbyMenu = lobbyCanvas.GetComponent<LobbyMenu>();
        lobbyMenu.Init();
        lobbyCanvas.SetActive(false);

        SetInput(inputManager.CurrentType);
        inputManager.GetInput().SetActive(false);

        gameManager.sceneRightEdge = sceneRightEdge;
        gameManager.sceneLeftEdge = sceneLeftEdge;
        gameManager.sceneTopEdge = sceneTopEdge;
        gameManager.sceneBottomEdge = sceneBottomEdge;
        gameManager.sceneWidth = sceneWidth;
        gameManager.sceneHeight = sceneHeight;

        startAsteroidCount = gameManager.StartAsteroidsCount;

        player = GameObject.Instantiate(gameManager.PlayerPrefab);
        ship = player.GetComponent<Ship>();
        ship.Init();
        player.SetActive(false);

        Ufo = GameObject.Instantiate(gameManager.UfoPrefab);
        ufo = Ufo.GetComponent<UFO>();
        ufo.Init();
        Ufo.SetActive(false);

        EnteringGameMenu();
    }

    public void EnteringGameMenu()
    {
        RemoveAllAsteroids();
        state = GameState.Start;
        lobbyCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        SpawnAsteroids(2, gameManager.BigAsteroidPrefab, GetRandomAsteroidPosition());
        SpawnAsteroids(3, gameManager.MidSateroidPrefab, GetRandomAsteroidPosition());
    }

    public void NewGame()
    {
        RemoveAllAsteroids();
        Play();
        state = GameState.Process;
        gameCanvas.SetActive(true);
        lobbyCanvas.SetActive(false);

        startAsteroidCount = gameManager.StartAsteroidsCount;
        score = 0;
        UpdateScore(score);
        lifeCount = gameManager.StartLifeCount;
        gameUi.UpdateLifeCount(lifeCount);

        SpawnAsteroids(startAsteroidCount, gameManager.BigAsteroidPrefab, GetRandomAsteroidPosition());

        inputManager.GetInput().SetActive(true);

        DeadUfo(0);

        player.SetActive(true);
        ship.ResetObject();
        ship.Rotation = 0;
        ship.Blink();
    }

    private void CreatePools()
    {
        pool.AddPool(PoolType.Ateroids);
        pool.AddPool(PoolType.Bullet);
    }

    private void RemoveAllAsteroids()
    {
        asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        for (int i = 0; i < asteroids.Length; i++)
        {
            pool.Despawn(PoolType.Ateroids, asteroids[i]);
        }
        asteroidsCount = 0;
    }

    private void NextLevel()
    {
        startAsteroidCount++;
        Timer nextLevelTimer = timerManager.CreateTimer(2f, () =>
        {
            SpawnAsteroids(startAsteroidCount, gameManager.BigAsteroidPrefab, GetRandomAsteroidPosition());
        });
        Level++;
    }

    public void TakeLife()
    {
        lifeCount--;
        gameUi.UpdateLifeCount(lifeCount);
       
        if (CheckLife())
        {
            DeadShip();
        }
    }

    private bool CheckLife()
    {
        if (lifeCount == 0)
        {
            GameOver();
            return false;
        }
        return true;
    }

    private void GameOver()
    {
        state = GameState.Score;
        gameCanvas.SetActive(false);
        lobbyCanvas.SetActive(true);
        player.SetActive(false);
        DeadUfo(0);
        EnteringGameMenu();
    }

    public void SpawnAsteroids(int asteroidsCount, GameObject type, Vector3 pos)
    {
        for (int i = 0; i < asteroidsCount; i++)
        {
            CreateAsteroid(type, pos);
        }
    }

    private GameObject CreateAsteroid(GameObject type, Vector3 pos)
    {
        GameObject tempAteroid = pool.Spawn(PoolType.Ateroids, type, pos,
                new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 0));
        asteroidsCount++;

        return tempAteroid;
    }

    public void RemoveAsteroid(int score) 
    {
        asteroidsCount--;
        UpdateScore(score);

        if (asteroidsCount == 0)
        {
            NextLevel();
        }
    }

    public void UpdateScore(int count)
    {
        score += count;
        gameUi.UpdateScoreUI(score);
    }

    private Vector3 GetRandomAsteroidPosition()
    {
        return new Vector3(Random.Range(0, gameManager.sceneWidth * 2), 0, Random.Range(0, gameManager.sceneHeight * 2));
    }

    public void PlayerInput(InputKeys key)
    {
        ship.ShipControl(key);
    }

    public void PlayerInput(float angle)
    {
        ship.ShipControl(InputKeys.nan, angle);
    }

    public void GameMenu()
    {
        if (state == GameState.Process)
        {
            Pause();
            lobbyCanvas.SetActive(true);
        }
        else if (state == GameState.Pause)
        {
            Play();
            lobbyCanvas.SetActive(false);
        }
    }

    private void Pause()
    {
        if (state == GameState.Process)
            Time.timeScale = 0;
        state = GameState.Pause;
    }

    private void Play()
    {
        if (state == GameState.Pause)
            Time.timeScale = 1;
        state = GameState.Process;
    }

    private void FindCamera()
    {
        if (gameManager.GameCameraPrefab == null)
        {
            gameCamera = Camera.main;
        }
        else
        {
            gameCamera = gameManager.GameCameraPrefab;
        }
    }

    private void AddEventSystem()
    {
        GameObject mainObj = GameObject.Find("[Sys_Core]");
        mainObj.AddComponent<EventSystem>();
        mainObj.AddComponent<StandaloneInputModule>();
    }

    private void DefiningScreenBoundaries()
    {
        sceneWidth = gameCamera.orthographicSize * 2 * gameCamera.aspect;
        sceneHeight = gameCamera.orthographicSize * 2;
        sceneRightEdge = sceneWidth / 2;
        sceneLeftEdge = sceneRightEdge * -1;
        sceneTopEdge = sceneHeight / 2;
        sceneBottomEdge = sceneTopEdge * -1;
    }

    public void DeadShip()
    {
        ship.Blink();
    }

    public void DeadUfo(int count)
    {
        UpdateScore(count);
        ufo.ResetObject();
        Ufo.SetActive(false);
        ufo.isDead = true;
        AttackUfo(); 
    }

    public void AttackUfo() 
    {
        StartCoroutine(Attack());
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetInput(InputType type)
    {
            inputManager.SetInputType(type);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(Random.Range(gameManager.MinAppearanceUFO,gameManager.MaxAppearanceUFO));
        if(state == GameState.Process)
        {
            ufo.StartTimer(2f);
            Ufo.SetActive(true);
            ufo.SetUfoPosition();
            ufo.SummonsUfo();
        }
        
    } 
}
