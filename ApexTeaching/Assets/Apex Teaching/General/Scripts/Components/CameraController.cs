﻿namespace Apex.AI.Teaching
{
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public sealed class CameraController : MonoBehaviour
    {
        public float mouseMoveMargin = 20f;
        public float moveSpeed = 20f;
        public float scrollSpeed = 1f;

        public Vector2 constraintsY = new Vector2(10f, 60f);

        private void FixedUpdate()
        {
            MoveOnKeys();
            ZoomOnScroll();
        }

        private void ZoomOnScroll()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll == 0f)
            {
                return;
            }

            var y = Mathf.Clamp(this.transform.position.y + (-Mathf.Sign(scroll) * scrollSpeed), this.constraintsY.x, this.constraintsY.y);
            this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);
        }

        private void MoveOnKeys()
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                MoveCamera(Vector3.forward);
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                MoveCamera(Vector3.back);
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                MoveCamera(Vector3.right);
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                MoveCamera(Vector3.left);
            }
        }

        private void MoveCamera(Vector3 direction)
        {
            this.transform.position += direction.normalized * this.moveSpeed * Time.fixedDeltaTime;
        }
    }
}