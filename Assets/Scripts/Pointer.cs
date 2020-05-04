using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Класс указателя на объекты на сцене.
    /// </summary>
    public class Pointer : MonoBehaviour
    {
        /// <summary>
        /// Длина указателя по умолчанию.
        /// </summary>
        public float DefaultLength = 5.0f;

        /// <summary>
        /// Объект точки на конце указателя.
        /// </summary>
        public GameObject Dot;

        /// <summary>
        /// Модель геймпада, из которой будет исходить луч.
        /// </summary>
        public GameObject GamepadModel;

        /// <summary>
        /// Модуль ввода для обработки событий взаимодействия с окружением.
        /// </summary>
        public VRInputModule InputModule;

        /// <summary>
        /// Линия указателя.
        /// </summary>
        private LineRenderer LineRenderer = null;

        /// <summary>
        /// Настройка указаеля.
        /// </summary>
        private void Awake()
        {
            LineRenderer = GetComponent<LineRenderer>();
        }

        /// <summary>
        /// Обновление указателя.
        /// </summary>
        private void Update()
        {
            UpdateLine();
        }

        /// <summary>
        /// Обновить линию указателя.
        /// </summary>
        private void UpdateLine()
        {
            // Позиция объекта - источника линии указателя
            var lineSource = GamepadModel.transform;

            // Задать длину указателя по умолчанию или по расстоянию до объекта
            var targetLength = DefaultLength;

            // Отлавливаем попадание лазера на объект
            var hit = CreateRaycast(lineSource, targetLength);

            // Координата конца лазера по умолчанию
            var endPosition = lineSource.position + lineSource.forward * targetLength;

            if (hit.collider != null)       // Координата конца лазера на объекте, если он есть
                endPosition = hit.point;

            // Размещаем точку на конце лазера
            Dot.transform.position = endPosition;

            // Отрисовываем линию лазера
            LineRenderer.SetPosition(0, lineSource.position);
            LineRenderer.SetPosition(1, endPosition);
        }

        /// <summary>
        /// Определить попадание луча в объект.
        /// </summary>
        /// <param name="source">Источник луча.</param>
        /// <param name="length">Длина луча лазера.</param>
        /// <returns>Информация о попадании линии указателя в какой-либо объект.</returns>
        private RaycastHit CreateRaycast(Transform source, float length)
        {
            var ray = new Ray(source.position, source.forward);

            Physics.Raycast(ray, out RaycastHit hit, length);

            return hit;
        }
    }
}
