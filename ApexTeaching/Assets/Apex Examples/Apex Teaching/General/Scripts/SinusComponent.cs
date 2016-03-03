namespace Apex.AI.Teaching
{
    using UnityEngine;

    public class SinusComponent : MonoBehaviour
    {
        public Vector3 start;
        public Vector3 magnitude;
        public Vector3 direction;

        private UnitBase _unit;

        public void OnEnable()
        {
            _unit = this.transform.GetComponentInParent<UnitBase>();
        }

        private void Update()
        {
            if (_unit != null && !_unit.isMoving)
            {
                this.transform.localPosition = Vector3.zero;
                return;
            }

            var time = Time.time;
            var x = Mathf.Sin(direction.x * time + start.x) * magnitude.x;
            var y = Mathf.Sin(direction.y * time + start.y) * magnitude.y;
            var z = Mathf.Sin(direction.z * time + start.z) * magnitude.z;
            this.transform.localPosition += new Vector3(x, y, z);
        }
    }
}