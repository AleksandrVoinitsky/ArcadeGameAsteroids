using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class GameUI : MonoBehaviour 
{
    private int LifeCountUi = 3;
    public GameObject ImageParent;
    public Text ScoreText;
    public int AsteroidsAmount;
    public GameObject LifeImage;
    private List<GameObject> ImageLifeTimeList;
    public void Init()
    {
        ImageLifeTimeList = new List<GameObject>();
        for (int i = 0; i < LifeCountUi; i++)
        {
            ImageLifeTimeList.Add( GameObject.Instantiate(LifeImage, ImageParent.transform));
        }
    }

    public void UpdateScoreUI(int Score)
    {
        ScoreText.text = Score.ToString() + "                                                         " +
            "asteroids amount - " + AsteroidsAmount;
    }

    public void UpdateLifeCount(int lifeCount)
    {
        LifeCountUi = lifeCount;

        for (int i = 0; i < ImageLifeTimeList.Count; i++)
        {
            ImageLifeTimeList[i].SetActive(false);
        }

        for (int i = 0; i < LifeCountUi; i++)
        {
            ImageLifeTimeList[i].SetActive(true);
        }
    }

}
