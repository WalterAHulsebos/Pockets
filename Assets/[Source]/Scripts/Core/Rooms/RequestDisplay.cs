using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RequestDisplay : MonoBehaviour
{
	//private GameObject m_GameObject = null;

	[SerializeField] LayerMask m_PreviewLayer;
	[SerializeField] Camera m_Camera = null;
	[SerializeField] RenderTexture m_RenderTexture = null;
	[SerializeField] Renderer m_DisplayScreenRenderer = null;
	[SerializeField] float m_RotationSpeed = 5.0f;
	[SerializeField] TextMeshPro m_ItemNameLabel = null;
	[SerializeField] TextMeshPro m_QuantityLabel = null;
	[SerializeField] MeshFilter m_PreviewItemMeshFilter = null;

	public void SetUp(string name, int quantity, Mesh mesh)
    {
		//m_NameLabel.text = name;
		//m_QuantityLabel.text = quantity.ToString();

		//if (m_GameObject != null)
		//Destroy(m_GameObject);

		m_PreviewItemMeshFilter.mesh = mesh;

		//m_GameObject = Instantiate(prefab, m_Camera.transform.position, Quaternion.identity);

		//m_GameObject.transform.position = m_Camera.transform.position + m_Camera.transform.forward * 1.0f;

		//SetLayerRecursively(m_GameObject, LayerMask.NameToLayer("RequestPreview"));
		//SetKinematicRecursively(m_GameObject);

		m_ItemNameLabel.text = name;
		m_QuantityLabel.text = quantity.ToString();

		//Destroy(item);
	}

	private void Awake()
	{
		m_RenderTexture = new RenderTexture(256, 256, 16);
		m_RenderTexture.Create();

		m_Camera.targetTexture = m_RenderTexture;


		m_DisplayScreenRenderer.material.mainTexture = m_RenderTexture;

		m_Camera.transform.position = new Vector3(Random.Range(500, 900), Random.Range(500, 900), Random.Range(500, 900));

		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (m_RenderTexture != null)
		{
			m_RenderTexture.Release();
			m_RenderTexture = null;
		}

		if (m_DisplayScreenRenderer.material != null)
		{
			Destroy(m_DisplayScreenRenderer.material);
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Rotate the m_PreviewItemMeshFilter
		m_PreviewItemMeshFilter.transform.Rotate(Vector3.one * Time.deltaTime * m_RotationSpeed);

		m_Camera.Render();
	}

	void SetLayerRecursively(GameObject o, int layer)
	{
		foreach (Transform t in o.GetComponentsInChildren<Transform>(true))
			t.gameObject.layer = layer;
	}

	void SetKinematicRecursively(GameObject o)
	{
		foreach (Rigidbody r in o.GetComponentsInChildren<Rigidbody>(true))
			r.isKinematic = true;
	}
}
