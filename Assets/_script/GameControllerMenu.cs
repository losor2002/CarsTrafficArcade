using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _script
{
    public class GameControllerMenu : MonoBehaviour
    {
        [FormerlySerializedAs("PlayButt")] public GameObject playButton;
        [FormerlySerializedAs("zombiebutt")] public GameObject zombieModeButton;
        [FormerlySerializedAs("ClassicButt")] public GameObject classicModeButton;
        [FormerlySerializedAs("menubutton")] public GameObject optionsButton;

        [FormerlySerializedAs("controlsysimg")]
        public GameObject controlSystemSelection;

        [FormerlySerializedAs("sceltafrecce")] public GameObject arrowsSelection;
        [FormerlySerializedAs("backbutt")] public GameObject backButton;
        [FormerlySerializedAs("carsbutt")] public GameObject carSelectionButton;
        [FormerlySerializedAs("tutorialButt")] public GameObject resetTutorialButton;
        [FormerlySerializedAs("volumeButt")] public GameObject volumeButton;
        [FormerlySerializedAs("mutoButt")] public GameObject muteButton;
        [FormerlySerializedAs("tabella")] public GameObject rewardTable;
        [FormerlySerializedAs("quad1")] public GameObject rewardQuad1;
        [FormerlySerializedAs("quad2")] public GameObject rewardQuad2;
        [FormerlySerializedAs("quad3")] public GameObject rewardQuad3;
        [FormerlySerializedAs("quad4")] public GameObject rewardQuad4;
        [FormerlySerializedAs("quad5")] public GameObject rewardQuad5;
        [FormerlySerializedAs("quad6")] public GameObject rewardQuad6;
        [FormerlySerializedAs("quad7")] public GameObject rewardQuad7;
        [FormerlySerializedAs("uscita")] public GameObject quitSelection;

        [FormerlySerializedAs("controlsys")] public Button controlSystem0Button;
        [FormerlySerializedAs("controlsys1")] public Button controlSystem1Button;
        [FormerlySerializedAs("controlsys2")] public Button controlSystem2Button;
        [FormerlySerializedAs("frdestra")] public Button rightArrowsButton;
        [FormerlySerializedAs("frsinistra")] public Button leftArrowsButton;
        [FormerlySerializedAs("frcentro")] public Button centerArrowsButton;
        [FormerlySerializedAs("highscoretx")] public Text highScoreText;
        [FormerlySerializedAs("scoretx")] public Text scoreText;
        [FormerlySerializedAs("crtx")] public Text crText;

        private AsyncOperation _asyncLoad;
        private bool _audioButtonVisibility = true;
        private int _cr;
        private int _highScore;
        private bool _isInMainMenu = true;
        private bool _isInOptions;
        private int _score;
        private bool _stopRewardQuadBlinking;

        private void Start()
        {
            Application.targetFrameRate = -1;

            RewardCheck();

            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            _cr = PlayerPrefs.GetInt("cr", 0);
            _highScore = PlayerPrefs.GetInt("HighScore", 0);
            _score = PlayerPrefs.GetInt("CurrentScore", 0);
            PlayerPrefs.SetInt("kills", 0);
            AsyncLoadPlayScene();
        }

        private void Update()
        {
            if (_isInMainMenu)
            {
                highScoreText.text = "HighScore: " + _highScore;
                scoreText.text = "Score: " + _score;
                crText.text = "CR: " + _cr;


                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    quitSelection.SetActive(true);
                    HideMainMenu();
                }
            }
            else
            {
                highScoreText.text = "";
                scoreText.text = "";
                crText.text = "";

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ShowMainMenu();
                }
            }

            if (_audioButtonVisibility)
            {
                var audst = PlayerPrefs.GetInt("audst", 1);
                if (audst == 1)
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
            else
            {
                volumeButton.SetActive(false);
                muteButton.SetActive(false);
            }

            if (_isInOptions)
            {
                var control = PlayerPrefs.GetInt("control", 2);

                if (control == 2)
                {
                    arrowsSelection.SetActive(true);
                }
                else
                {
                    arrowsSelection.SetActive(false);
                }


                if (control == 0)
                {
                    controlSystem0Button.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                    controlSystem1Button.GetComponent<Image>().color = default;
                    controlSystem2Button.GetComponent<Image>().color = default;
                }
                else if (control == 1)
                {
                    controlSystem0Button.GetComponent<Image>().color = default;
                    controlSystem1Button.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                    controlSystem2Button.GetComponent<Image>().color = default;
                }
                else if (control == 2)
                {
                    controlSystem0Button.GetComponent<Image>().color = default;
                    controlSystem1Button.GetComponent<Image>().color = default;
                    controlSystem2Button.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                    var frecce = PlayerPrefs.GetInt("frecce", 2);
                    if (frecce == 0)
                    {
                        rightArrowsButton.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                        leftArrowsButton.GetComponent<Image>().color = default;
                        centerArrowsButton.GetComponent<Image>().color = default;
                    }
                    else if (frecce == 1)
                    {
                        rightArrowsButton.GetComponent<Image>().color = default;
                        leftArrowsButton.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                        centerArrowsButton.GetComponent<Image>().color = default;
                    }
                    else if (frecce == 2)
                    {
                        rightArrowsButton.GetComponent<Image>().color = default;
                        leftArrowsButton.GetComponent<Image>().color = default;
                        centerArrowsButton.GetComponent<Image>().color = new Color32(156, 156, 156, 255);
                    }
                }
            }
        }

        private void AsyncLoadPlayScene()
        {
            _asyncLoad = SceneManager.LoadSceneAsync("scena1");
            _asyncLoad.allowSceneActivation = false;
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
            rewardTable.SetActive(true);
            HideMainMenu();
            var day = PlayerPrefs.GetInt("day", 1);

            if ((difference.Days == 0 && firstTime == 0) || difference.Days > 1)
            {
                rewardQuad1.SetActive(true);
                StartCoroutine(BlinkRewardQuad1());
                var c = PlayerPrefs.GetInt("cr", 0);
                PlayerPrefs.SetInt("cr", c + 10);
                PlayerPrefs.SetInt("ft", firstTime + 1);
                PlayerPrefs.SetInt("day", 2);
            }

            if (difference.Days == 1)
            {
                if (day == 1)
                {
                    PlayerPrefs.SetInt("day", 2);
                    rewardQuad1.SetActive(true);
                    StartCoroutine(BlinkRewardQuad1());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 10);
                }

                if (day == 2)
                {
                    PlayerPrefs.SetInt("day", 3);
                    rewardQuad2.SetActive(true);
                    StartCoroutine(BlinkRewardQuad2());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 20);
                }

                if (day == 3)
                {
                    PlayerPrefs.SetInt("day", 4);
                    rewardQuad3.SetActive(true);
                    StartCoroutine(BlinkRewardQuad3());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 30);
                }

                if (day == 4)
                {
                    PlayerPrefs.SetInt("day", 5);
                    rewardQuad4.SetActive(true);
                    StartCoroutine(BlinkRewardQuad4());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 40);
                }

                if (day == 5)
                {
                    PlayerPrefs.SetInt("day", 6);
                    rewardQuad5.SetActive(true);
                    StartCoroutine(BlinkRewardQuad5());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 50);
                }

                if (day == 6)
                {
                    PlayerPrefs.SetInt("day", 7);
                    rewardQuad6.SetActive(true);
                    StartCoroutine(BlinkRewardQuad6());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 60);
                }

                if (day == 7)
                {
                    PlayerPrefs.SetInt("day", 1);
                    rewardQuad7.SetActive(true);
                    StartCoroutine(BlinkRewardQuad7());
                    var c = PlayerPrefs.GetInt("cr", 0);
                    PlayerPrefs.SetInt("cr", c + 70);
                }
            }
        }

        public void CloseRewardTable()
        {
            _stopRewardQuadBlinking = true;
            rewardTable.SetActive(false);
            ShowMainMenu();
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
            _asyncLoad.allowSceneActivation = true;
        }

        public void StartZombieMode()
        {
            PlayerPrefs.SetInt("zombie", 1);
            _asyncLoad.allowSceneActivation = true;
        }

        public void ShowOptions()
        {
            playButton.SetActive(false);
            optionsButton.SetActive(false);
            carSelectionButton.SetActive(false);
            _audioButtonVisibility = false;
            controlSystemSelection.SetActive(true);
            backButton.SetActive(true);
            resetTutorialButton.SetActive(true);
            _isInMainMenu = false;
            _isInOptions = true;
        }

        public void SelectControlSystem0()
        {
            PlayerPrefs.SetInt("control", 0);
        }

        public void SelectControlSystem1()
        {
            PlayerPrefs.SetInt("control", 1);
        }

        public void SelectControlSystem2()
        {
            PlayerPrefs.SetInt("control", 2);
        }

        public void SelectRightArrows()
        {
            PlayerPrefs.SetInt("frecce", 0);
        }

        public void SelectLeftArrows()
        {
            PlayerPrefs.SetInt("frecce", 1);
        }

        public void SelectCenterArrows()
        {
            PlayerPrefs.SetInt("frecce", 2);
        }

        public void ShowMainMenu()
        {
            playButton.SetActive(true);
            optionsButton.SetActive(true);
            carSelectionButton.SetActive(true);
            controlSystemSelection.SetActive(false);
            backButton.SetActive(false);
            resetTutorialButton.SetActive(false);
            classicModeButton.SetActive(false);
            zombieModeButton.SetActive(false);
            quitSelection.SetActive(false);
            arrowsSelection.SetActive(false);
            _isInOptions = false;
            _audioButtonVisibility = true;
            _isInMainMenu = true;
        }

        public void LoadCarSelection()
        {
            SceneManager.LoadScene("cars");
        }

        public void ResetTutorials()
        {
            PlayerPrefs.SetInt("tutorial1", 0);
            PlayerPrefs.SetInt("tutorial2", 0);
            PlayerPrefs.SetInt("tutorialz", 0);
        }

        public void Mute()
        {
            PlayerPrefs.SetInt("audst", 0);
        }

        public void Unmute()
        {
            PlayerPrefs.SetInt("audst", 1);
        }

        public void ShowGameModes()
        {
            zombieModeButton.SetActive(true);
            classicModeButton.SetActive(true);
            backButton.SetActive(true);
            optionsButton.SetActive(false);
            carSelectionButton.SetActive(false);
            playButton.SetActive(false);
            _isInMainMenu = false;
        }

        public void Quit()
        {
            PlayerPrefs.Save();
            Application.Quit();
        }

        private void HideMainMenu()
        {
            _isInMainMenu = false;
            playButton.SetActive(false);
            optionsButton.SetActive(false);
            carSelectionButton.SetActive(false);
            _audioButtonVisibility = false;
        }
    }
}