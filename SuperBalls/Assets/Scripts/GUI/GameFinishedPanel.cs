using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUI {

    public class GameFinishedPanel : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI _finalPoints;

        public void Show(int finalPoints) {
            gameObject.SetActive(true);
            _finalPoints.text = $"Final Score: {finalPoints}";
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
    
        public void OnRestartClick() {
            SceneManager.LoadScene(0);
        }

        public void OnExitClick() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    
    }

}