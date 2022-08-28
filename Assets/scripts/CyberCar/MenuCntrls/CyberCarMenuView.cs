using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Zenject;

namespace CyberCar.MenuCntrls
{
    public class CyberCarMenuView : MonoBehaviour
    {
        public CyberCarMenuCntrl MenuCntrl = new CyberCarMenuCntrl();
        public TMP_Text CoinScore;
        SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<Signal_start_game>(StartGame);
        }

        IEnumerator Start()
        {
            if (PlayerPrefs.GetInt("FirstIn") == 1)
            {
                Camera.main.GetComponent<Animator>().enabled = false;
                yield return new WaitForSeconds(0f);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }


            //PlayerPrefs.SetInt("CoinScore",0);
            CoinScore.text = PlayerPrefs.GetInt("CoinScore").ToString();
            PlayerPrefs.SetInt("FirstIn", 1);
            Camera.main.GetComponent<Animator>().enabled = false;
            ShowPanel(0);
        }

        public void StartGame()
        {
            MenuCntrl.RunGame();
        }

        public void ShowPanel(int id)
        {
            ShowMenuPanle panel = new ShowMenuPanle();
            panel.idPanel = id;
            _signalBus.Fire(panel);
        }

        public void openModal(int ModalId)
        {
            _signalBus.Fire(new ShowMenuModalPlane() {idPanel = ModalId});
        }

        public void ClearPrefs()
        {
            PlayerPrefs.SetInt("FirstIn", 0);
            _signalBus.Fire<SignalVipeData>();
        }
    }
}