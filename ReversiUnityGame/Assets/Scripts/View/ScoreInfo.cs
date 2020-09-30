using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View
{

    public class ScoreInfo : MonoBehaviour
    {
        [SerializeField] 
        Text WhiteScore;

        [SerializeField] 
        Text BlackScore;

        public void UpdateScore(int CountWhite, int CountBlack)
        {
            WhiteScore.text = CountWhite.ToString();
            BlackScore.text = CountBlack.ToString();
        }

        public void ClearAll()
        {
            WhiteScore.text = "0";
            BlackScore.text = "0";
        }

        void Start()
        {
            ClearAll();
        }
    }
}
