using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LukensUtils
{
    public class LukensUtilsRuntime : MonoBehaviour
    {
        private Camera m_MainCam;
        public void DelayedFire(Action action, float time)
        {
            StartCoroutine(DelayedFireRoutine(action, time));
        }

        private IEnumerator DelayedFireRoutine(Action action, float time)
        {
            yield return new WaitForSeconds(time);

            action.Invoke();
        }
        
        public GameObject WorldSpaceMouseRaycast(float distance = 100)
        {
            GameObject foundObject = null;

            if (m_MainCam == null)
                m_MainCam = Camera.main;

            Ray ray = m_MainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, distance))
                foundObject = hit.collider.gameObject;

            return foundObject;
        }

        public GameObject RaycastFromPosition(Vector3 position, Vector3 direction, float distance = 100f)
        {
            if (Physics.Raycast(position, direction, out RaycastHit hit, distance))
                return hit.collider.gameObject;

            Debug.Log("Raycast did not hit anything!");
            return null;
        }
    }
}