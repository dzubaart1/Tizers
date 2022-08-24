using System;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CyberCar.ModDialogs
{
    public class DialogCntrl : MonoBehaviour
    {
        public Image Char1;
        public Image Char2;
        public TMP_Text CharName;
        public TMP_Text ReplicaText;
        public List<DialogReplica> ReplicsList;
        public DialogParams Curparams;
        private DialogData _DialogData;
        [SerializeField]
        private int CurReplica;

        public Button nexBtn;
        SignalBus _signalBus;
        [Inject]
        public void Construct( SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Start()
        {
             _DialogData = JsonUtility.FromJson<DialogData>(Curparams.dialogFile.text);
            ShowReplica();
            nexBtn.onClick.AddListener(NextReplica);
        }

        void ShowReplica()
        {
            DialogReplica replica = _DialogData.data[CurReplica];
            controlImage(replica.CharId);
            CharName.text = replica.CharName;
            ReplicaText.text = replica.Replica;
        }

        public void NextReplica()
        {
            CurReplica++;
            if (CurReplica + 1 == _DialogData.data.Length)
            {
                ShowReplica();
                nexBtn.transform.Find("Text").GetComponent<TMP_Text>().text = "Завершить";
                nexBtn.onClick.RemoveAllListeners();
                nexBtn.onClick.AddListener(FinalReplica);
                return;
            }
                ShowReplica();
        }

        public void ScipAll()
        {
            FinalReplica();
        }

        void controlImage(int id)
        {
            if (id == 0)
            {
                Char1.gameObject.SetActive(true);
                Char1.sprite = Curparams.CharSprite[id];
                Char2.gameObject.SetActive(false);
            }
            else
            {
                Char1.gameObject.SetActive(false);
                Char2.sprite = Curparams.CharSprite[id];
                Char2.gameObject.SetActive(true);
            }
        }

        void FinalReplica()
        {
            ShowMenuPanle panel = new ShowMenuPanle();
            panel.idPanel = 1;
            _signalBus.Fire(panel);
        }
    }
}