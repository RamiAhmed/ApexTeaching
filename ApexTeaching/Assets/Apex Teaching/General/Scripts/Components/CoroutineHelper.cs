namespace Apex.AI.Teaching
{
    using UnityEngine;

    public sealed class CoroutineHelper : MonoBehaviour
    {
        public static CoroutineHelper instance;

        private void OnEnable()
        {
            if (instance != null)
            {
                Destroy(this, 0.1f);
                return;
            }

            instance = this;
        }
    }
}