using System.Collections;
using System.Collections.Generic;
using _1.GameCntrl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    public BoxCntrl BoxPrefab;
    public BoxCntrl CurCube;
    public Transform ClawPoint;
    public int MaxScore;
    public List<Color> Colors;
    public Dictionary<int, Color> ColorsDictionary;
    public CanvasView cView;
    public Animator BadfGuy;
    public int winScore=32;
    public AudioSource audio;
    void Start()
    {
        ColorsDictionary = new Dictionary<int, Color>();
        for (int i = 0; i < Colors.Count; i++)
        {
            ColorsDictionary.Add(i, Colors[i]);
        }

        CurCube = Instantiate(BoxPrefab, ClawPoint);
        CurCube.transform.position = ClawPoint.position;
    }
    
    public void DropCube()
    {
        if (!CurCube) return;
        CurCube.dropBox();
        CurCube = null;
        StartCoroutine(CreateCube());
    }

    public IEnumerator CreateCube()
    {
        yield return new WaitForSeconds(1f);
        CurCube = Instantiate(BoxPrefab, ClawPoint);
        CurCube.transform.position = ClawPoint.position;
    }

    public void setScore(int points)
    {
        if (points > MaxScore)
        {
            MaxScore = points;
            cView.setScore(MaxScore);
        }

        if (MaxScore >= winScore)
        {
            BadfGuy.SetTrigger("Die");
            cView.SetWin();
        }
    }

    public void restartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("UiMenu");
    }
}