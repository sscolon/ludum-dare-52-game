using System.Collections;
using TMPro;
using UnityEngine;

namespace Mechanizer
{
    public class DeathManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _deathParent;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private TMP_Text _summary;
        [SerializeField] private string[] _tips;
        private void Start()
        {
            _deathParent.transform.localScale = Vector3.zero;
        }

        public void Activate(GameManager context)
        {
            Cursor.visible = true;
            StartCoroutine(SpawnRoutine(context));
        }

        private IEnumerator SpawnRoutine(GameManager context)
        {
            float time = 0f;
            while (time < 1.0f)
            {
                time += Time.deltaTime*3f;
                _deathParent.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
                yield return null;
            }
            int score = context.Score;
            string text = score.ToString("0000000");

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                _score.text += c.ToString();
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(1f);
            WriteSummary(score);
        }

        private void WriteSummary(int score)
        {
            if (score == 0)
            {
                _summary.text = "What? How?";
            }
            else
            {
                if (_tips.Length == 0)
                {
                    _summary.text = "What? How?";
                    return;
                }
             
                int tipIndex = Random.Range(0, _tips.Length);
                string tip = _tips[tipIndex];
                _summary.text = tip;
            }
        }

        public void Retry()
        {
            GameSceneManager.Main.LoadScene("main");
        }

        public void MainMenu()
        {
            GameSceneManager.Main.LoadScene("menu");
        }
    }
}