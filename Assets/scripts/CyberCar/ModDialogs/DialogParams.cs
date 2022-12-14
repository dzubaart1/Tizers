using System.Collections.Generic;
using UnityEngine;

namespace CyberCar.ModDialogs
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Dialogs/DialogParams", order = 1)]
    public class DialogParams : ScriptableObject
    {
        public int DialogId;
        public TextAsset dialogFile;
        public List<Sprite> CharSprite;
        public string EndDialogPrefsName;
    }
}