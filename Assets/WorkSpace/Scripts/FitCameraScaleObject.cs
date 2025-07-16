using UnityEngine;

public class FitCameraToObject : MonoBehaviour
{
    public Camera cam;
    public Transform targetObject;
    public float padding = 1.1f;

    void Start()
    {
        if (cam == null) cam = Camera.main;
        Fit();
    }

    void Fit()
    {
        Bounds bounds = GetBounds(targetObject.gameObject);
        Vector3 size = bounds.size;

        float fov = cam.fieldOfView;
        float aspect = (float)Screen.width / Screen.height;
        float halfFOVRad = Mathf.Deg2Rad * fov * 0.5f;

        // Tính khoảng cách cần thiết để fit toàn bộ chiều cao hoặc chiều ngang
        float heightDistance = (size.z / 2f) / Mathf.Tan(halfFOVRad);     // chiều dài map
        float widthDistance = (size.x / 2f) / (Mathf.Tan(halfFOVRad) * aspect); // chiều rộng map

        float distance = Mathf.Max(heightDistance, widthDistance) * padding;

        // Camera sẽ từ trên nhìn xuống, góc nghiêng nhẹ
        Vector3 direction = new Vector3(0, 1, -1).normalized;
        cam.transform.position = bounds.center + direction * distance;

        cam.transform.LookAt(bounds.center);
    }

    Bounds GetBounds(GameObject go)
    {
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(go.transform.position, Vector3.zero);

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
            bounds.Encapsulate(r.bounds);
        return bounds;
    }
}
