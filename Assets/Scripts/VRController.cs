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
        private const float Speed = 0.0f;

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
            CalculateMovement();
            HandleHeight();
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

        private void CalculateMovement()
        {

        }

        private void HandleHeight()
        {

        }
    }
}
