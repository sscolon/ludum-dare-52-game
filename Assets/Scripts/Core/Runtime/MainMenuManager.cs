using System.Collections;
using UnityEngine;

namespace Mechanizer
{
    public class MainMenuManager : MonoBehaviour
    {
        public void Play()
        {
            GameSceneManager.Main.LoadScene("main");
        }
    }
}