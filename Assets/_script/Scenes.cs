using UnityEngine;
using UnityEngine.SceneManagement;

namespace _script
{
    public static class Scenes
    {
        public const string Menu = "menu";
        public const string CarSelection = "cars";
        public const string PlayScene = "scena1";

        public static AsyncOperation LoadSceneAsync(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            return asyncOperation;
        }

        public static void ActivateScene(AsyncOperation asyncOperation)
        {
            asyncOperation.allowSceneActivation = true;
        }
    }
}