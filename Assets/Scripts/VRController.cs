using UnityEngine;
using Valve.VR;

namespace Assets.Scripts
{
    /// <summary>
    /// Класс для управления передвижением игрока с помощью тачпада.
    /// youtube.com/watch?v=QREKO1sf8b8
    /// </summary>
    public class VRController : MonoBehaviour
    {
        /// <summary>
        /// Насколько значение силы нажатия на тачпад будет увеличивать скорость игрока [-1..1].
        /// </summary>
        public float Sensitivity = 0.1f;

        /// <summary>
        /// Максимальная скорость игрока.
        /// </summary>
        public float MaxSpeed = 1.0f;

        /// <summary>
        /// Событие нажатия на тачпад (кнопку движения).
        /// </summary>
        public SteamVR_Action_Boolean MovePress = null;

        /// <summary>
        /// Направление движения игрока, задаваемое тачпадом.
        /// </summary>
        public SteamVR_Action_Vector2 MoveValue = null;

        /// <summary>
        /// Скорость передвижения игрока.
        /// </summary>
        private float speed = 0.0f;

        /// <summary>
        /// Объект управления игроком.
        /// </summary>
        private CharacterController characterController = null;

        /// <summary>
        /// Объект VR-камеры игрока.
        /// </summary>
        private Transform cameraRig = null;

        /// <summary>
        /// Объект головы игрока в VR.
        /// </summary>
        private Transform head = null;

        /// <summary>
        /// Метод, вызываемый до Start.
        /// </summary>
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Start is called before the first frame update.
        /// </summary>
        void Start()
        {
            cameraRig = SteamVR_Render.Top().origin;
            head = SteamVR_Render.Top().head;
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        void Update()
        {
            HandleHead();
            HandleHeight();
            CalculateMovement();
        }

        /// <summary>
        /// Обработка поворота головы игроком.
        /// </summary>
        private void HandleHead()
        {
            // Сохранить текущее положение камеры игрока
            var oldPosition = cameraRig.position;
            var oldRotation = cameraRig.rotation;

            // Поворачиваем объект игрока туда, куда повёрнута камеры
            transform.eulerAngles = new Vector3(0.0f, head.rotation.eulerAngles.y, 0.0f);

            // Восстанавливаем положение камеры, т.к. она тоже двигается с объектом игрока
            cameraRig.position = oldPosition;
            cameraRig.rotation = oldRotation;
        }

        /// <summary>
        /// Приведение объекта игрока в движение.
        /// </summary>
        private void CalculateMovement()
        {
            // Определяем направление движения, поскольку объект игрока вращается вместе с камерой
            var orientationEuler = new Vector3(0, transform.eulerAngles.y, 0);
            var orientation = Quaternion.Euler(orientationEuler);
            var movement = Vector3.zero;

            // Если не двигаемся, обнуляем скорость (можно замедлять, чтобы убрать инерцию)
            // Если наше событие перешло из состояния true в состояние false
            if (MovePress.GetStateUp(SteamVR_Input_Sources.Any))
                speed = 0;

            // Если тачпад нажат, событие перешло в состояние true
            if (MovePress.state)
            {
                // Увеличиваем скорость игрока, проверяя, что она не выходит за границы
                speed += MoveValue.axis.y * Sensitivity;
                speed = Mathf.Clamp(speed, -MaxSpeed, MaxSpeed);

                // Указываем направление движения с заданной скоростью
                // Идём в ту сторону, в которую смотрим, а скорость растёт постепенно
                movement += orientation * (speed * Vector3.forward) * Time.deltaTime;
            }

            // Увеличить скорость передвижения
            characterController.Move(movement);
        }

        /// <summary>
        /// Изменить высоту объекта игрока в соответствии с высотой расположения шлема.
        /// </summary>
        private void HandleHeight()
        {
            // Задать положение головы в локальном пространстве по высоте от 1 до 2 метров
            var headHeight = Mathf.Clamp(head.position.y, 1, 2);
            characterController.height = headHeight;

            // Задать значение центра объекта игрока, чтобы ось повторота игрока была в центре
            var newCenter = Vector3.zero;
            newCenter.y = characterController.height / 2;
            newCenter.y += characterController.skinWidth; // Если не добавить, будет небольшая тряска

            // Движение капсулы в локальном пространстве
            newCenter.x = head.localPosition.x;
            newCenter.z = head.localPosition.z;

            // Поворот капсулы объекта игрока
            newCenter = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * newCenter;

            // Применить
            characterController.center = newCenter;
        }
    }
}
