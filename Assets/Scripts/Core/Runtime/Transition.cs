using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanizer
{
    public class Transition : MonoBehaviour
    {
        private static Sprite _screenOverlaySprite;
        private static Transition _transition;
        private Image _blackScreen;
        private const float TRANSITION_SPEED = 2f;
        public static Transition Main
        {
            get
            {
                if (_transition == null)
                {
                    Texture2D texture = new Texture2D(16, 16);
                    for (int x = 0; x < 16; x++)
                    {
                        for (int y = 0; y < 16; y++)
                        {
                            texture.SetPixel(x, y, Color.white);
                        }
                    }

                    texture.Apply();
                    _screenOverlaySprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 16f);

                    var gameObject = new GameObject();
                    gameObject.name = "manager_transition";
                    _transition = gameObject.AddComponent<Transition>();
                    _transition.hideFlags = HideFlags.HideInHierarchy;
                    DontDestroyOnLoad(_transition);
                }

                return _transition;
            }
        }

        private Image GetBlackScreen()
        {
            GameObject screen = new GameObject();
            GameObject.DontDestroyOnLoad(screen);
            Canvas canvas = screen.AddComponent<Canvas>();
            canvas.sortingOrder = 9999;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            GameObject imageObject = new GameObject();
            imageObject.transform.SetParent(screen.transform);

            Image b = imageObject.AddComponent<Image>();
            b.transform.localScale = new Vector3(9999, 9999, 1);
            b.sprite = _screenOverlaySprite;
            b.color = Color.clear;
            return b;
        }

        public IEnumerator TransitionIn(float speed = TRANSITION_SPEED)
        {
            if (_blackScreen == null)
            {
                _blackScreen = GetBlackScreen();
            }
            else
            {
                yield break;
            }

            float a = 0.0f;
            Color color = _blackScreen.color;
            color.a = a;
            _blackScreen.color = color;
            while (a < 1.0f)
            {
                a += Time.unscaledDeltaTime * speed;
                Color b = _blackScreen.color;
                b.a = a;
                _blackScreen.color = b;
                yield return null;
            }
        }

        public IEnumerator TransitionOut(float speed = TRANSITION_SPEED)
        {
            if (_blackScreen == null)
            {
                yield break;
            }

            float a = 1.0f;
            while (a > 0f)
            {
                a -= Time.unscaledDeltaTime * speed;
                if (_blackScreen == null)
                {
                    yield break;
                }

                Color b = _blackScreen.color;
                b.a = a;
                _blackScreen.color = b;
                yield return null;
            }

            //Destroy the object so we aren't losing frames for no reason.
            Destroy(_blackScreen.gameObject);
        }
    }
}
