using System;

namespace CyberCar.ModDialogs
{
    [Serializable]
    public class DialogReplica
    {
        public string CharName;
        public string Replica;
        public int CharId;
    }

    [Serializable]
    public class DialogData
    {
        public DialogReplica[] data;
    }
}