using UnityEngine;

namespace Assets.Resources.Scripts.Components
{
    public class SinusComponent : MonoBehaviour
    {
        public Vector3 start;
        public Vector3 magnitude;
        public Vector3 direction;

        public void OnEnable()
        {
        }

        private void Update()
        {
            var x = Mathf.Sin(direction.x * Time.time + start.x) * magnitude.x;
            var y = Mathf.Sin(direction.y * Time.time + start.y) * magnitude.y;
            var z = Mathf.Sin(direction.z * Time.time + start.z) * magnitude.z;

            var v = new Vector3(x, y, z);

            //this.transform.position += transform.TransformDirection(v);
            this.transform.localPosition += v;
        }
    }
}