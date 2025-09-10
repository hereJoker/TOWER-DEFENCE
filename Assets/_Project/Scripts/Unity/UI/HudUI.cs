using System;
using _Project.Scripts.Gameplay;
using _Project.Scripts.SpawnSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.Unity.UI
{
    public class HudUI : MonoBehaviour, IWinLoseListener
    {
        [SerializeField] private Text _totalLives;
        [SerializeField] private GameObject _root;
        [SerializeField] private GameObject _lose;
        [SerializeField] private GameObject _win;

        private void Awake()
        {
            _root.SetActive(true);
            _lose.SetActive(false);
            _win.SetActive(false);
        }

        public void SetLose()
        {
            _root.SetActive(true);
            _lose.SetActive(true);
            _win.SetActive(false);
        }

        public void SetWin()
        {
            _root.SetActive(true);
            _win.SetActive(true);
            _lose.SetActive(false);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnWin()
        {
            SetWin();
        }

        public void OnLose()
        {
            SetLose();
        }

        public void OnUpdateLives(in int lives)
        {
            _totalLives.text = lives.ToString();
        }
    }
}
