using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mechanizer
{
    public class GameSceneManager : MonoBehaviour
    {
        private Transition _transition;
        public static GameSceneManager Main { get; private set; }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var gameObject = new GameObject();
            Main = gameObject.AddComponent<GameSceneManager>();
            Main._transition = Transition.Main;
            Main.hideFlags = HideFlags.HideInHierarchy;
            DontDestroyOnLoad(Main);
        }

        public void LoadScene(string scene)
        {
            StartCoroutine(LoadSceneRoutine(scene));
        }

        public IEnumerator LoadSceneRoutine(string scene)
        {
            yield return StartCoroutine(_transition.TransitionIn());
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            while (!operation.isDone)
            {
                yield return null;
            }
            yield return StartCoroutine(_transition.TransitionOut());
        }
    }
}