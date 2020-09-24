using ReversiCore;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class ReversiModelHolder : MonoBehaviour
    {
        public ReversiModel reversiModel { get; private set; }  = new ReversiModel();
    }
}
