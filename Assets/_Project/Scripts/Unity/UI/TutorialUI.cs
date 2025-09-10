using System;
using UnityEngine;

namespace _Project.Scripts.Unity.UI
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        private bool _isFinished;
        private void Awake()
        {
            _isFinished = false;
            _root.gameObject.SetActive(true);
        }

        public void Finish()
        {
            if (_isFinished)
                return;

            _isFinished = true;
            _root.gameObject.SetActive(false);
        }
    }
}
