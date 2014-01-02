using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public float zoomOutSpeed = .5f;
	public float zoomInSpeed = 2f;
	
	public float zoomMarginPercent = 1.5f;
	
	public float panSpeed = .5f;

	private float nextZoom;
	private float zoomSpeed;
	private Vector3 nextPosition;
	private Bounds viewportTarget;
	
	private GravityController player;
	
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<GravityController>();
		viewportTarget = player.GetBounds();
		nextZoom = camera.orthographicSize;
		nextPosition = transform.position;
		zoomSpeed = zoomInSpeed;
	}
	
	void LateUpdate () {
		transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime / panSpeed);
		
		camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, nextZoom * zoomMarginPercent, Time.deltaTime / zoomSpeed);
	}
	
	public void UpdateViewport(Bounds b) {
		if (viewportTarget != b) {
			viewportTarget = b;

			nextZoom = Mathf.Max(viewportTarget.extents.x,viewportTarget.extents.y);
			zoomSpeed = nextZoom > camera.orthographicSize ? zoomOutSpeed : zoomInSpeed;

			nextPosition = viewportTarget.center - new Vector3(0,0,50f);
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(viewportTarget.center,viewportTarget.size * zoomMarginPercent);
	}
}
