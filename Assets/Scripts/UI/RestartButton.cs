using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Cards
{
    [RequireComponent(typeof(Button))]
    public class RestartButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        }
    }
}