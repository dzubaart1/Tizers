using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestsScript
{
    public class TestScenCntrl: MonoBehaviour
    {
        public void RestartScene(int type)
        {
            switch (type)
            {
                case 1:
                    SceneManager.LoadScene("TestScene");
                    break; 
                case 2:
                    SceneManager.LoadScene("CyberCarMenu");
                    break;  
                case 3:
                    SceneManager.LoadScene("TestDrift");
                    break;
            }
        }
    }
}