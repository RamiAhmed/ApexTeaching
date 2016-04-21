namespace Apex.AI.Teaching
{
    using UnityEngine;

    public class SinusComponent : MonoBehaviour
    {
        public Vector3 start;
        public Vector3 magnitude;
        public Vector3 direction;

        private UnitBase _unit;
        private float _lastStop;
        private Vector3 _startPos;

        public void OnEnable()
        {
            _unit = this.transform.GetComponentInParent<UnitBase>();
            _startPos = this.transform.localPosition;
        }

        private void FixedUpdate()
        {
            var time = Time.time;
            if (_unit != null && !_unit.isMoving)
            {
                _lastStop = time;
                this.transform.localPosition = _startPos;
                return;
            }

            var t = time - _lastStop;
            var x = Mathf.Sin(direction.x * t + start.x) * magnitude.x;
            var y = Mathf.Sin(direction.y * t + start.y) * magnitude.y;
            var z = Mathf.Sin(direction.z * t + start.z) * magnitude.z;
            this.transform.localPosition += new Vector3(x, y, z);
        }
    }
}