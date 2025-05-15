using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BaseBallScene
{
    public class ScoreSystem : MonoBehaviour
    {
        /* 
         * ����� ���� ���� ����� ��Ʈ�� ������� ���� hit/miss�� üũ ������
         * �ش� ������ ���� �ý��� ���� �� �����Ǿ�� ��.
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