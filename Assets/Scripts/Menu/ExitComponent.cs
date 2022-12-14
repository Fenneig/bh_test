using UnityEngine;

namespace Menu
{
    public class ExitComponent : MonoBehaviour
    {
        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}