using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunTimeData : MonoSingleton<RunTimeData>
{
    public List<int> DeckList = new List<int>();
    public int MaxHP = 50;
    public int Defence = 10;
    public int Speed = 5;
    public int DrawCount = 3;
    public int HP = 50;

    public int Gold = 0;
    public int Exp = 0;

    public int LayerIndex = 0;
    public int CurrentNodeID = 0;

    public int Level = 1;
    public int StatPoint = 0;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            HP = MaxHP;
            StatPoint += (Exp / 20) * 2;
            Level += Exp / 20;
            Exp = 0;
        }
    }
}
