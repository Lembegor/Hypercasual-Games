using Game.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class ResultScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Text _title;
        [SerializeField] private Button _restartButton;
        [SerializeField] private string _wonText = "You Win!";
        [SerializeField] private string _lostText = "You Lose";

        private GameController _game;

        [Inject]
        private void Construct(GameController game)
        {
            _game = game;
        }

        private void Start()
        {
            _panel.SetActive(false);
            _game.Finished += Show;
            _restartButton.onClick.AddListener(Restart);
        }

        private void OnDestroy()
        {
            _game.Finished -= Show;
            _restartButton.onClick.RemoveListener(Restart);
        }

        private void Show(GameState state)
        {
            _title.text = state == GameState.Won ? _wonText : _lostText;
            _panel.SetActive(true);
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
