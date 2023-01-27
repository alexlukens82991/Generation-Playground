using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPixelController : MonoBehaviour
{
    [SerializeField] private LineRenderer m_LineRenderer;
    [SerializeField] private MeshRenderer m_Renderer;
    [SerializeField] private float m_MaxHeight;

    private void Start()
    {
        LukensUtils.LukensUtilities.DelayedFire(MakeConnections, Random.Range(2f, 7f));
    }

    private void MakeConnections()
    {
        Collider[] foundColliders = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (Collider collider in foundColliders)
        {
            AddConnection(collider.transform.position);
        }

        Color newGreen = Color.green;
        newGreen.a = transform.position.y / m_MaxHeight;

        m_Renderer.material.color = newGreen;
        m_LineRenderer.material.color = newGreen;
    }

    private void AddConnection(Vector3 position)
    {
        m_LineRenderer.positionCount += 2;
        m_LineRenderer.SetPosition(m_LineRenderer.positionCount - 2, transform.position);
        m_LineRenderer.SetPosition(m_LineRenderer.positionCount - 1, position);
    }
}
