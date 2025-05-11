using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BaseBallScene
{
    public class ScoreSystem : MonoBehaviour
    {
        /* 
         * 현재는 공이 땅에 닿는지 배트에 닿는지에 따라 hit/miss가 체크 되지만
         * 해당 판정은 리듬 시스템 도입 후 수정되어야 함.
        */



        public TMPro.TextMeshProUGUI scoreText;
        int hit;
        int miss;

        public static ScoreSystem instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                hit = 0;
                miss = 0;
            }
            else if (instance != this)
            {
                Destroy(this);
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this);
        }

        public void Hit()
        {
            hit++;
            UpdateUI();
        }
        public void Miss()
        {
            miss++;
            UpdateUI();
        }

        void UpdateUI()
        {
            scoreText.text = $"{hit} / {miss}";
        }
    }
}