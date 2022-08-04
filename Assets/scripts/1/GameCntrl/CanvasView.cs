using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace _1.GameCntrl
{
    public class CanvasView : MonoBehaviour
    {
        public TMP_Text Score;
        public CanvasGroup WinPanel;
        public CanvasGroup GamePanel;

        private void Start()
        {
            CanvasGroupCntrl.ChangeStateCanvas(WinPanel, false);
            CanvasGroupCntrl.ChangeStateCanvas(GamePanel, true);
        }

        public void setScore(int points)
        {
            Score.text = points.ToString()+"/32";
        }

        public void SetWin()
        {
            CanvasGroupCntrl.ChangeStateCanvas(GamePanel, false);
            StartCoroutine(ShowWinPanel());

        }
        IEnumerator ShowWinPanel()
        {
            yield return new WaitForSeconds(2f);
            CanvasGroupCntrl.ChangeStateCanvas(WinPanel, true);
        }
    }
}