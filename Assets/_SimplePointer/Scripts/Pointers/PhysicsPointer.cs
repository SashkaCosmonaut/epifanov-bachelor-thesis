using UnityEngine;

public class PhysicsPointer : MonoBehaviour
{
    [SerializeField] protected float defaultLength = 3.0f;

    private LineRenderer lineRenderer = null;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLength();
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, CalculateEnd());
    }

    protected virtual Vector3 CalculateEnd()
    {
        // Отлавливаем попадание лазера на объект
        var hit = CreateRaycast();

        var endPosition = DefaultEnd(defaultLength);

        if (hit.collider != null)       // Координата конца лазера на объекте, если он есть
            endPosition = hit.point;

        return endPosition;
    }

    protected Vector3 DefaultEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }

    /// <summary>
    /// Определить попадание луча в объект.
    /// </summary>
    /// <param name="length">Длина луча лазера.</param>
    /// <returns>Информация о попадании линии указателя в какой-либо объект.</returns>
    private RaycastHit CreateRaycast()
    {
        var ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out RaycastHit hit, defaultLength);

        return hit;
    }
}
