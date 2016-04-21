namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class TimeControlComponent : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Plus) || Input.GetKeyUp(KeyCode.KeypadPlus))
            {
                if (Time.timeScale < 64f)
                {
                    Time.timeScale *= 2f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp(KeyCode.KeypadMinus))
            {
                if (Time.timeScale > 0.0625f)
                {
                    Time.timeScale *= 0.5f;
                }
            }
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(Screen.width * 0.5f - 70f, 5f, 140f, 25f), string.Concat("Time Scale: ", Time.timeScale.ToString("F2"), "x"));
        }
    }
}