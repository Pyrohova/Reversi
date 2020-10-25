using ReversiCore;
using ReversiRobot;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class ReversiModelHolder : MonoBehaviour
    {
        public ReversiModel reversiModel { get; private set; }
        public RandomUser robotPlayer { get; private set; }

        void Awake()
        {
            reversiModel = new ReversiModel();
            robotPlayer = new RandomUser(reversiModel);
        }
    }
}
