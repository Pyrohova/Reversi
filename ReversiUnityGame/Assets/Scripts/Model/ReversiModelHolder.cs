using ReversiCore;
using ReversiRobot;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class ReversiModelHolder : MonoBehaviour
    {
        public ReversiModel reversiModel { get; private set; }
        private RandomUser robotPlayer;

        void OnEnable()
        {
            reversiModel = new ReversiModel();
            robotPlayer = new RandomUser(reversiModel);
        }
    }
}
