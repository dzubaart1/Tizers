using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Zenject;

namespace CyberCar.MenuCntrls
{
    public class ChalengePanelView : MonoBehaviour
    {
        public TMP_Text ChalengTitle;
        public List<RoadPatern> RoadPaterns;
        public Transform Content;
        public ChalengeItemCntrl ButtonPrefab;
        SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            RoadPaterns = Resources.LoadAll<RoadPatern>("RoadMissions").ToList();
            SetContent();
        }
        public void SetContent()
        {
            foreach (Transform child in Content)
            {
                Destroy(child.gameObject);
            }
            foreach (var car in RoadPaterns)
            {
                ChalengeItemCntrl item = Instantiate(ButtonPrefab, Content);
                item.SetData(car, this);
            }
        }

        public void SetPaternId(int id)
        {
            PlayerPrefs.SetInt("PaternGame", id);
            _signalBus.Fire<Signal_start_game>();
            
        }
    }
}