using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] GameObject _titleScreenPanel;
        [SerializeField] OptionsMenu _optionsMenu;

        void Awake()
        {
            _optionsMenu.UpdateUI();
        }

        public void Play()
        {
            Manager.Instance.GoToGame();
        }

        public void OpenOptionsPanel()
        {
            _optionsMenu.gameObject.SetActive(true);
            _titleScreenPanel.SetActive(false);
        }
        public void CloseOptionsPanel()
        {
            _optionsMenu.gameObject.SetActive(false);
            _titleScreenPanel.SetActive(true);
        }

 

    }
}
