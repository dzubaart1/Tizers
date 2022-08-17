using Obstacles;
using UnityEngine;

namespace CyberCar.Bonuses
{
    [CreateAssetMenu(fileName = "Effect", menuName = "Effects/effect", order = 1)]
    public class EffectParams : ScriptableObject
    {
        public Material Material;
        public Mesh mesh;
       
    }
}