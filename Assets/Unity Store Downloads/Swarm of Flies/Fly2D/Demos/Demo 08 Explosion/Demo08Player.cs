using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptBoy.Fly2D.Demo
{
    public class Demo08Player : MonoBehaviour
    {
        public GameObject explosionPrefab;

        private void Update()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(explosionPrefab, mousePosition, Quaternion.identity);
            }
        }
    }
}