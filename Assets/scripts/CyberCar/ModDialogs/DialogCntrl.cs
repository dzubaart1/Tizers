using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        [SerializeField] private int CurReplica;

        private DialogsData _dialogsData;
        public Button nexBtn;
        SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<SignalVipeData>(VipeData);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0f);
            Debug.Log(Application.persistentDataPath );
            List<DialogParams> dialogParamsList = Resources.LoadAll<DialogParams>("DialogsData").ToList();
            _dialogsData = LoadDialogs();
            if (_dialogsData != null)
            {
                foreach (var VARIABLE in dialogParamsList)
                {
                    if (VARIABLE.DialogId == _dialogsData.LastShowedDialog + 1)
                    {
                        Curparams = VARIABLE;
                    }
                }
            }
            else
            {
                foreach (var VARIABLE in dialogParamsList)
                {
                    if (VARIABLE.DialogId ==0)
                    {
                        Curparams = VARIABLE;
                    }
                }
            }

            if (Curparams != null)
            {
                _DialogData = JsonUtility.FromJson<DialogData>(Curparams.dialogFile.text);
                ShowReplica();
                nexBtn.onClick.AddListener(NextReplica);
            }
            else
            {
                Debug.Log("try skip");
                ShowMenuPanle panel = new ShowMenuPanle();
                panel.idPanel = 1;
                _signalBus.Fire(panel);
            }
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
            SaveDialogData(Curparams.DialogId);
        }

        void VipeData()
        {
            SaveDialogData(-1);
        }

        void SaveDialogData(int dId)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/DialogsData.dat");
            DialogsData data = new DialogsData();
            data.LastShowedDialog = dId;
            bf.Serialize(file, data);
            file.Close();
        }

        public DialogsData LoadDialogs()
        {
            if (File.Exists(Application.persistentDataPath + "/DialogsData.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file =
                    File.Open(Application.persistentDataPath
                              + "/DialogsData.dat", FileMode.Open);
                DialogsData data = (DialogsData) bf.Deserialize(file);
                file.Close();
                return data;
            }
            else
                return null;
        }
    }
}