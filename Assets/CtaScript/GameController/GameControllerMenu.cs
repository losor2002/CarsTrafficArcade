using System;
using System.Collections;
using CtaScript.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CtaScript.GameController
{
    public class GameControllerMenu : MonoBehaviour
    {
        public GameObject playButton;
        public GameObject zombieModeButton;
        public GameObject classicModeButton;
        public GameObject optionsButton;
        
        public GameObject controlSystemSelection;

        public GameObject arrowsSelection;
        public GameObject backButton;
        public GameObject carSelectionButton;
        public GameObject resetTutorialButton;
        public GameObject volumeButton;
        public GameObject muteButton;
        public GameObject rewardTable;
        public GameObject rewardQuad1;
        public GameObject rewardQuad2;
        public GameObject rewardQuad3;
        public GameObject rewardQuad4;
        public GameObject rewardQuad5;
        public GameObject rewardQuad6;
        public GameObject rewardQuad7;
        public GameObject quitSelection;

        public Button controlSystem0Button;
        public Button controlSystem1Button;
        public Button controlSystem2Button;
        public Button rightArrowsButton;
        public Button leftArrowsButton;
        public Button centerArrowsButton;
        public Text classicText;
        public Text zombieText;
        public Text crText;

        private AsyncOperation _asyncLoadPlayScene;
        private int _cr;
        private int _classicHighScore;
        private bool _isInMainMenu;
        private int _zombieHighScore;
        private int _zombieHighKills;
        private bool _stopRewardQuadBlinking;

        private void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            
            _cr = PlayerPrefs.GetInt("cr");
            _classicHighScore = PlayerPrefs.GetInt("HighScore");
            _zombieHighScore = PlayerPrefs.GetInt("ZombieHighScore");
            _zombieHighKills = PlayerPrefs.GetInt("ZombieHighKills");

            ShowMainMenu();
            RewardCheck();
            _asyncLoadPlayScene = Scenes.LoadSceneAsync(Scenes.PlayScene);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isInMainMenu)
                {
                    ShowQuitSelection();
                }
                else
                {
                    ShowMainMenu();
                }
            }
        }

        private void RewardCheck()
        {
            var firstTime = PlayerPrefs.GetInt("ft", 0);
            var stringDate = PlayerPrefs.GetString("PlayDate", Convert.ToString(DateTime.Now));
            var oldDate = Convert.ToDateTime(stringDate);
            var newDate = DateTime.Now;

            var difference = newDate.Subtract(oldDate);
            if (difference.Days >= 1 || (difference.Days == 0 && firstTime == 0))
            {
                var newStringDate = Convert.ToString(newDate);
                PlayerPrefs.SetString("PlayDate", newStringDate);
                GiveReward(difference, firstTime);
            }
        }

        private void GiveReward(TimeSpan difference, int firstTime)
        {
            HideMainMenu();
            _stopRewardQuadBlinking = false;
            rewardTable.SetActive(true);
            var day = PlayerPrefs.GetInt("day", 1);

            switch (difference.Days)
            {
                case 0 when firstTime == 0:
                case > 1:
                {
                    rewardQuad1.SetActive(true);
                    StartCoroutine(BlinkRewardQuad1());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 10);
                    PlayerPrefs.SetInt("ft", firstTime + 1);
                    PlayerPrefs.SetInt("day", 2);
                    break;
                }
                case 1:
                {
                    switch (day)
                    {
                        case 1:
                        {
                            PlayerPrefs.SetInt("day", 2);
                            rewardQuad1.SetActive(true);
                            StartCoroutine(BlinkRewardQuad1());
                            var c = PlayerPrefs.GetInt("cr", 0);
                            PlayerPrefs.SetInt("cr", c + 10);
                            break;
                        }
                        case 2:
                        {
                            PlayerPrefs.SetInt("day", 3);
                            rewardQuad2.SetActive(true);
                            StartCoroutine(BlinkRewardQuad2());
                            var c = PlayerPrefs.GetInt("cr", 0);
                            PlayerPrefs.SetInt("cr", c + 20);
                            break;
                        }
                        case 3:
                        {
                            PlayerPrefs.SetInt("day", 4);
                            rewardQuad3.SetActive(true);
                            StartCoroutine(BlinkRewardQuad3());
                            var c = PlayerPrefs.GetInt("cr", 0);
                            PlayerPrefs.SetInt("cr", c + 30);
                            break;
                        }
                        case 4:
                        {
                            PlayerPrefs.SetInt("day", 5);
                            rewardQuad4.SetActive(true);
                            StartCoroutine(BlinkRewardQuad4());
                            var c = PlayerPrefs.GetInt("cr", 0);
                            PlayerPrefs.SetInt("cr", c + 40);
                            break;
                        }
                        case 5:
                        {
                            PlayerPrefs.SetInt("day", 6);
                            rewardQuad5.SetActive(true);
                            StartCoroutine(BlinkRewardQuad5());
                            var c = PlayerPrefs.GetInt("cr", 0);
                            PlayerPrefs.SetInt("cr", c + 50);
                            break;
                        }
                        case 6:
                        {
                            PlayerPrefs.SetInt("day", 7);
                            rewardQuad6.SetActive(true);
                            StartCoroutine(BlinkRewardQuad6());
                            var c = PlayerPrefs.GetInt("cr", 0);
                            PlayerPrefs.SetInt("cr", c + 60);
                            break;
                        }
                        case 7:
                        {
                            PlayerPrefs.SetInt("day", 1);
                            rewardQuad7.SetActive(true);
                            StartCoroutine(BlinkRewardQuad7());
                            var c = PlayerPrefs.GetInt("cr", 0);
                            PlayerPrefs.SetInt("cr", c + 70);
                            break;
                        }
                    }

                    break;
                }
            }
        }

        private void HideRewardTable()
        {
            _stopRewardQuadBlinking = true;
            rewardTable.SetActive(false);
        }

        private IEnumerator BlinkRewardQuad1()
        {
            while (!_stopRewardQuadBlinking)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad1.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad1.SetActive(true);
            }
        }

        private IEnumerator BlinkRewardQuad2()
        {
            while (!_stopRewardQuadBlinking)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad2.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad2.SetActive(true);
            }
        }

        private IEnumerator BlinkRewardQuad3()
        {
            while (!_stopRewardQuadBlinking)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad3.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad3.SetActive(true);
            }
        }

        private IEnumerator BlinkRewardQuad4()
        {
            while (!_stopRewardQuadBlinking)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad4.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad4.SetActive(true);
            }
        }

        private IEnumerator BlinkRewardQuad5()
        {
            while (!_stopRewardQuadBlinking)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad5.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad5.SetActive(true);
            }
        }

        private IEnumerator BlinkRewardQuad6()
        {
            while (!_stopRewardQuadBlinking)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad6.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad6.SetActive(true);
            }
        }

        private IEnumerator BlinkRewardQuad7()
        {
            while (!_stopRewardQuadBlinking)
            {
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad7.SetActive(false);
                yield return new WaitForSecondsRealtime(0.75f);
                rewardQuad7.SetActive(true);
            }
        }

        public void StartClassicMode()
        {
            PlayerPrefs.SetInt("zombie", 0);
            Scenes.ActivateScene(_asyncLoadPlayScene);
        }

        public void StartZombieMode()
        {
            PlayerPrefs.SetInt("zombie", 1);
            Scenes.ActivateScene(_asyncLoadPlayScene);
        }

        public void SelectControlSystem0()
        {
            PlayerPrefs.SetInt("control", 0);
            UpdateControlSystemSelection();
        }

        public void SelectControlSystem1()
        {
            PlayerPrefs.SetInt("control", 1);
            UpdateControlSystemSelection();
        }

        public void SelectControlSystem2()
        {
            PlayerPrefs.SetInt("control", 2);
            UpdateControlSystemSelection();
        }

        public void SelectRightArrows()
        {
            PlayerPrefs.SetInt("frecce", 0);
            UpdateControlSystemSelection();
        }

        public void SelectLeftArrows()
        {
            PlayerPrefs.SetInt("frecce", 1);
            UpdateControlSystemSelection();
        }

        public void SelectCenterArrows()
        {
            PlayerPrefs.SetInt("frecce", 2);
            UpdateControlSystemSelection();
        }

        public void LoadCarSelection()
        {
            SceneManager.LoadScene(Scenes.CarSelection);
        }

        public void ResetTutorials()
        {
            PlayerPrefs.SetInt("tutorial1", 0);
            PlayerPrefs.SetInt("tutorial2", 0);
            PlayerPrefs.SetInt("tutorialz", 0);
        }

        public void Quit()
        {
            PlayerPrefs.Save();
            Application.Quit();
        }

        public void Mute()
        {
            PlayerPrefs.SetInt("audst", 0);
            AudioListener.volume = 0.0f;
            volumeButton.SetActive(false);
            muteButton.SetActive(true);
        }

        public void Unmute()
        {
            PlayerPrefs.SetInt("audst", 1);
            AudioListener.volume = 1.0f;
            volumeButton.SetActive(true);
            muteButton.SetActive(false);
        }

        private void ShowAudioButton()
        {
            var audioState = PlayerPrefs.GetInt("audst", 1);
            if (audioState == 1)
            {
                AudioListener.volume = 1.0f;
                volumeButton.SetActive(true);
                muteButton.SetActive(false);
            }
            else
            {
                AudioListener.volume = 0.0f;
                volumeButton.SetActive(false);
                muteButton.SetActive(true);
            }
        }

        private void HideAudioButton()
        {
            volumeButton.SetActive(false);
            muteButton.SetActive(false);
        }

        private void UpdateControlSystemSelection()
        {
            var controlSystem = PlayerPrefs.GetInt("control", 2);

            arrowsSelection.SetActive(controlSystem == 2);

            var transparentButtonColor = controlSystem0Button.GetComponent<Image>().color;
            transparentButtonColor.a = 0.4f;
            var opaqueButtonColor = transparentButtonColor;
            opaqueButtonColor.a = 1.0f;

            switch (controlSystem)
            {
                case 0:
                    controlSystem0Button.GetComponent<Image>().color = opaqueButtonColor;
                    controlSystem1Button.GetComponent<Image>().color = transparentButtonColor;
                    controlSystem2Button.GetComponent<Image>().color = transparentButtonColor;
                    break;
                case 1:
                    controlSystem0Button.GetComponent<Image>().color = transparentButtonColor;
                    controlSystem1Button.GetComponent<Image>().color = opaqueButtonColor;
                    controlSystem2Button.GetComponent<Image>().color = transparentButtonColor;
                    break;
                case 2:
                {
                    controlSystem0Button.GetComponent<Image>().color = transparentButtonColor;
                    controlSystem1Button.GetComponent<Image>().color = transparentButtonColor;
                    controlSystem2Button.GetComponent<Image>().color = opaqueButtonColor;

                    var arrows = PlayerPrefs.GetInt("frecce", 2);
                    
                    transparentButtonColor = rightArrowsButton.GetComponent<Image>().color;
                    transparentButtonColor.a = 0.4f;
                    opaqueButtonColor = transparentButtonColor;
                    opaqueButtonColor.a = 1.0f;
                    
                    switch (arrows)
                    {
                        case 0:
                            rightArrowsButton.GetComponent<Image>().color = opaqueButtonColor;
                            leftArrowsButton.GetComponent<Image>().color = transparentButtonColor;
                            centerArrowsButton.GetComponent<Image>().color = transparentButtonColor;
                            break;
                        case 1:
                            rightArrowsButton.GetComponent<Image>().color = transparentButtonColor;
                            leftArrowsButton.GetComponent<Image>().color = opaqueButtonColor;
                            centerArrowsButton.GetComponent<Image>().color = transparentButtonColor;
                            break;
                        case 2:
                            rightArrowsButton.GetComponent<Image>().color = transparentButtonColor;
                            leftArrowsButton.GetComponent<Image>().color = transparentButtonColor;
                            centerArrowsButton.GetComponent<Image>().color = opaqueButtonColor;
                            break;
                    }

                    break;
                }
            }
        }

        public void ShowOptions()
        {
            HideMainMenu();
            controlSystemSelection.SetActive(true);
            backButton.SetActive(true);
            resetTutorialButton.SetActive(true);
            UpdateControlSystemSelection();
        }

        private void HideOptions()
        {
            controlSystemSelection.SetActive(false);
            backButton.SetActive(false);
            resetTutorialButton.SetActive(false);
            arrowsSelection.SetActive(false);
        }

        public void ShowMainMenu()
        {
            HideOptions();
            HideGameModes();
            HideQuitSelection();
            HideRewardTable();
            playButton.SetActive(true);
            optionsButton.SetActive(true);
            carSelectionButton.SetActive(true);
            ShowAudioButton();
            _isInMainMenu = true;
            classicText.text = "Classic: " + _classicHighScore;
            zombieText.text = $"Zombie: {_zombieHighScore} + {_zombieHighKills}";
            crText.text = "CR: " + _cr;
        }

        private void HideMainMenu()
        {
            playButton.SetActive(false);
            optionsButton.SetActive(false);
            carSelectionButton.SetActive(false);
            _isInMainMenu = false;
            HideAudioButton();
            classicText.text = "";
            zombieText.text = "";
            crText.text = "";
        }

        public void ShowGameModes()
        {
            HideMainMenu();
            ShowAudioButton();
            zombieModeButton.SetActive(true);
            classicModeButton.SetActive(true);
            backButton.SetActive(true);
        }

        private void HideGameModes()
        {
            HideAudioButton();
            zombieModeButton.SetActive(false);
            classicModeButton.SetActive(false);
            backButton.SetActive(false);
        }

        private void ShowQuitSelection()
        {
            HideMainMenu();
            quitSelection.SetActive(true);
        }

        private void HideQuitSelection()
        {
            quitSelection.SetActive(false);
        }
    }
}