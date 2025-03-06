using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _script
{
    public class GameControllerCarSelection : MonoBehaviour
    {
        [FormerlySerializedAs("cr")] public Text crText;
        [FormerlySerializedAs("astontx")] public Text astonText;
        [FormerlySerializedAs("chevtx")] public Text chevText;
        [FormerlySerializedAs("f1tx")] public Text f1Text;
        [FormerlySerializedAs("sport1tx")] public Text sport1Text;
        [FormerlySerializedAs("muscletx")] public Text muscleText;

        private bool _astonPurchased;
        private AsyncOperation _asyncLoadMenu;
        private bool _chevPurchased;
        private int _cr;
        private bool _f1Purchased;
        private bool _musclePurchased;
        private bool _sport1Purchased;

        private void Start()
        {
            _cr = PlayerPrefs.GetInt("cr", 0);
            crText.text = "CR: " + _cr;

            _astonPurchased = PlayerPrefs.GetInt("aston", 0) != 0;
            _chevPurchased = PlayerPrefs.GetInt("chev", 0) != 0;
            _f1Purchased = PlayerPrefs.GetInt("f1", 0) != 0;
            _sport1Purchased = PlayerPrefs.GetInt("sport1", 0) != 0;
            _musclePurchased = PlayerPrefs.GetInt("muscle", 0) != 0;

            if (_astonPurchased)
            {
                astonText.text = "";
            }

            if (_chevPurchased)
            {
                chevText.text = "";
            }

            if (_f1Purchased)
            {
                f1Text.text = "";
            }

            if (_sport1Purchased)
            {
                sport1Text.text = "";
            }

            if (_musclePurchased)
            {
                muscleText.text = "";
            }

            _asyncLoadMenu = Scenes.LoadSceneAsync(Scenes.Menu);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }

        public void Back()
        {
            Scenes.ActivateScene(_asyncLoadMenu);
        }

        public void Lambo1()
        {
            PlayerPrefs.SetInt("car", 0);
            Back();
        }

        public void Aston()
        {
            switch (_astonPurchased)
            {
                case false when _cr >= 50:
                    PlayerPrefs.SetInt("cr", _cr - 50);
                    PlayerPrefs.SetInt("aston", 1);
                    PlayerPrefs.SetInt("car", 1);
                    PlayerPrefs.Save();
                    Back();
                    break;
                case true:
                    PlayerPrefs.SetInt("car", 1);
                    Back();
                    break;
            }
        }

        public void Chev()
        {
            switch (_chevPurchased)
            {
                case false when _cr >= 400:
                    PlayerPrefs.SetInt("cr", _cr - 400);
                    PlayerPrefs.SetInt("chev", 1);
                    PlayerPrefs.SetInt("car", 2);
                    PlayerPrefs.Save();
                    Back();
                    break;
                case true:
                    PlayerPrefs.SetInt("car", 2);
                    Back();
                    break;
            }
        }

        public void F1()
        {
            switch (_f1Purchased)
            {
                case false when _cr >= 1000:
                    PlayerPrefs.SetInt("cr", _cr - 1000);
                    PlayerPrefs.SetInt("f1", 1);
                    PlayerPrefs.SetInt("car", 3);
                    PlayerPrefs.Save();
                    Back();
                    break;
                case true:
                    PlayerPrefs.SetInt("car", 3);
                    Back();
                    break;
            }
        }

        public void Sport1()
        {
            switch (_sport1Purchased)
            {
                case false when _cr >= 425:
                    PlayerPrefs.SetInt("cr", _cr - 425);
                    PlayerPrefs.SetInt("sport1", 1);
                    PlayerPrefs.SetInt("car", 4);
                    PlayerPrefs.Save();
                    Back();
                    break;
                case true:
                    PlayerPrefs.SetInt("car", 4);
                    Back();
                    break;
            }
        }

        public void Muscle()
        {
            switch (_musclePurchased)
            {
                case false when _cr >= 100:
                    PlayerPrefs.SetInt("cr", _cr - 100);
                    PlayerPrefs.SetInt("muscle", 1);
                    PlayerPrefs.SetInt("car", 6);
                    PlayerPrefs.Save();
                    Back();
                    break;
                case true:
                    PlayerPrefs.SetInt("car", 6);
                    Back();
                    break;
            }
        }
    }
}