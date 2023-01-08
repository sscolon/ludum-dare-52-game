using UnityEngine;

namespace Mechanizer
{
    public class GameScoreUI : MonoBehaviour
    {
        private int _lastScore;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private GameNumberUI _number;
        private void Start()
        {
            _gameManager.OnScoreChanged += UpdateUI;
            UpdateUI(_gameManager.Score);
        }

        private void OnDestroy()
        {
            _gameManager.OnScoreChanged -= UpdateUI;
        }

        private void UpdateUI(int score)
        {
            if (score > _lastScore)
            {
                _number.PopAnimation();
            }

            _lastScore = score;
            string text = score.ToString("0000000");
            _number.Text = text;
        }
    }
}