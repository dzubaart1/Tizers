using Obstacles;
using UnityEngine;

namespace CyberCar.Bonuses
{
    [CreateAssetMenu(fileName = "Effect", menuName = "Effects/effect", order = 1)]
    public class EffectParams : ScriptableObject
    {
        public int effectId;
        public Material Material;
        public Mesh mesh;
        public Sprite DefenceIcon;
        public float EffectTime;
        public float showBtnTime;
        public ObstacleCntrl.EfectType myType;
        public GameObject EfectObj;

    }
}