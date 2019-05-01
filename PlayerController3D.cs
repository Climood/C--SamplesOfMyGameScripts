using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerController3D : MonoBehaviour
{
    enum Statements
    {
        attack,jump,run,idle
    }
    public float playerHealth = 1f;
    public Transform transformingBar; //MAX SCALES бара хп следующие : х =4 , y = 0.21, z=1(z и y  не меняем) , соотсетственно, если хп упало до половины => x = 2
    public GameObject bloodParcicleEffect; //ссылка на объект с эффектом брызг кровушки 
    public static bool playerIsDead = false;

    //public float speed = 50f;

    public float speed = 80F; //Скорость передвижения
    public float jumpSpeed = 50F; //Начальная скорость прыжка
    public float gravity = 30F; //Сила гравитации
    //нужна для нормального переключения анимаций
    Statements curStatement;
    /*
    //переменные для границ мира
    private float upRoadBorderY = -61.6f; //Проинициализировал на случай, если объект WordlBordersManager не создасться и не запустит свой Start(), если же создастся, его значения обновтят эти
    private float downRoadBorderY = -85.6f;
    private float upRoadBorderZ = 14.9f;
    private float downRoadBorderZ = -21.5f;
    private float rightRoadBorderX = 347f;
    private float leftRoadBorderX = -374f;
    */
    //public static bool canMoveToRight = true; 
    //public static bool canMoveToleft = true;
    
    private Rigidbody rb;
    private Animator animatorController;
    private CharacterController moveController;
    private bool faceToRight = true; //Изначально смотрим вправо
    private bool isJumped = false; //Чтобы отслеживтаь прыжки
    // Start is called before the first frame update
    private Vector3 movePosition;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animatorController = GetComponent<Animator>();
        moveController = GetComponent<CharacterController>();
    }
    /*
    public void setBordersOfWorld(float upRoadBorderY ,float downRoadBorderY, float upRoadBorderZ, float downRoadBorderZ,float rightRoadBorderX,float leftRoadBorderX)
    {
        this.upRoadBorderY = upRoadBorderY;
        this.downRoadBorderY = downRoadBorderY;
        this.upRoadBorderZ = upRoadBorderZ;
        this.downRoadBorderZ = downRoadBorderZ;
        this.rightRoadBorderX = rightRoadBorderX;
        this.leftRoadBorderX = leftRoadBorderX;
    } 
    */
    public void playerTakeDamage(float amountDamage)
    {
        Instantiate(bloodParcicleEffect, transform.position, Quaternion.identity); //каждый раз копирует объект эффекта крови и создает его на месте нпц при получении им урона 
        playerHealth -= amountDamage;
    }

    void reverseFace() //функция смены взгляда в другую сторону  
    {
        faceToRight = !faceToRight;
        //Поворачивает не корректно, скорее перерастягивает//
        //transform.localScale = new Vector3(transform.localScale.x * -1,transform.localScale.y,transform.localScale.z); //Vector из 3 х элементов(хоть и 2д элемент) но плэйр использует так же z, смена х на -1 поворачивает спрайт !
        //для полноценного разворота объекта и всего дочернего use transform.Rotate( 0f, 180f, 0f);
        transform.Rotate(0f, 180f, 0f);// НО ОН РАЗВОРАЧИВАЕТ ВООБЩЕ ВСЕ, ВКЛЮЧАЯ ПРИЯЗЯННЫЕ ОБЪЕКТЫ (КАМЕРА и т.д.) и перемещение по векторам с ним надо изменять
        //придется переделать перемещение с учетом этого разворота, т.к. он нужен для корректной стрельбы в 3d
    }

   
    /// <summary>
    /// /////////// Методы для управления кнопками на андроиде УБРАЛ L R ТАК КАК ОНИ ПРЕДНАЗНАЧЕНЫ ДЛЯ 2 Д ПО ПРЯМОЙ
    /// </summary>

    
    public void onClickJumpButton()
    {
        //if(isGrounded) rb.AddForce(new Vector3(0, jumpForce, 0 ), ForceMode.Impulse); 
        //if (isGrounded) rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse); //В режиме импульса рабоает как реальный прыжок, падает быстрее и т.д.
        if (moveController.isGrounded) isJumped = true;
       
    }
    /*public void onClickFireButton() //Переношу атаки в отдельные скрипты
    {
        animatorController.SetTrigger("playerAttack");  //Теперь на атакует как FixedUpdate врубает Idle  анимацию если персонаж стоит
    }*/

    private void FixedUpdate()
    {

        
        //Забиваем старую позицию для возвращения при выходе за границы в прыжке
        //prevPosition = rb.transform.position;  БУДЕТ БАГ ЧТО ЗАВИСНЕТ НА ГРАНИЦЕ В ВОЗДУХЕ
        float horizontalAxis = SimpleInput.GetAxisRaw("Horizontal");
        float verticalAxis = SimpleInput.GetAxisRaw("Vertical");
        //Debug.Log("is grounded = " + isGrounded);
        if (moveController.isGrounded) //TODO заменить на CharacterControlle.isGrounded, он сам определяет находится ли контроллер на земле(статическом коллайдере)
        { //чтобы управление работало только на земле)
          
            if (verticalAxis > 0) // сделано, чтобы при перемещении джойстика вниз, колайдер игрока не падал отскакивая, а сразу прижимался к поверхности земли благодаря равенству отклонения по z
                movePosition = new Vector3(horizontalAxis, 0f, verticalAxis); //Изменил чтобы было передвижение по x z для 3д перемещения игрока по плэйну
            else movePosition = new Vector3(horizontalAxis, verticalAxis, verticalAxis);
            //rb.isKinematic = false; //отрубаем кинематику, чтобы персонаж поднимался, упираясь в наклоненную землю
            //transform.position += movePosition * speed * Time.deltaTime;  //Можно использовать нормаль вектора перемещения , а затем умножать ее на скорость  и передавать ее в Transform.transformdirection
            //rb.isKinematic = true;

            ///////////Переделка управление #4  для учета развернутого игрока через Rotate (для поддержки выстрелов)
            ///
            if (horizontalAxis > 0)
            {
                if (!faceToRight) reverseFace();
                if (verticalAxis > 0)
                {
                    movePosition = new Vector3(horizontalAxis, 0f, verticalAxis);
                }
                else
                {
                    movePosition = new Vector3(horizontalAxis, verticalAxis, verticalAxis);
                }
            }
            else if (horizontalAxis < 0)//если горизонтально джойстик смещен влево
            {
                if(faceToRight) reverseFace();
                if (verticalAxis > 0)
                {
                    movePosition = new Vector3(-horizontalAxis, 0f, -verticalAxis);
                }
                else
                {
                    movePosition = new Vector3(-horizontalAxis, verticalAxis, -verticalAxis); //Игрался с настройками минусов вживую, с учетом разворота :)
                }
            }


            
            //Управление через физику (работает значительно быстрее , чем через transform по координатам)
            //rb.AddForce(transform.forward * horizontalAxis * Speed, ForceMode.Force);
            movePosition = transform.TransformDirection(movePosition); //Переводим локальные координаты в соответствующие глобальные
            movePosition *= speed; //movePosition - это коэффициент скорости. Поэтому мы умножаем его на максимальную скорость, чтобы получить текущую
            if (isJumped)
            { //Если мы прыгаем
                movePosition.y = jumpSpeed; //Мы придаем скорости прыжка
                isJumped = false; //обнуляем прыжок
            }
        }
        movePosition.y -= gravity * Time.deltaTime; //Это действие гравитации
        moveController.Move(movePosition * Time.deltaTime); //получив все данные в конце-концов мы вызываем переменную Move и даем ей наш вектор направления. И она сама будет двигать нашего персонажа как надо. Без прохождений сквозь стены, без addForce и прочей ереси.

        //if (horizontalAxis > 0)
        //  rb.AddForce(transform.right * Speed, ForceMode.VelocityChange);




       
        ////Обработка, чтобы на месте врубал анимацию стоя, а в беге бегал 
        if (movePosition.x == 0 && movePosition.z == 0) //не проверяем y, так как на y действует имитированная гравитация
        {
            animatorController.SetTrigger("playerIdle");
        }
        else
        {
            animatorController.SetTrigger("playerRun");
        }

        if (playerHealth >= 1f) playerHealth = 1f;
        if (playerHealth <= 0f)
        {
            playerHealth = 0f;
            playerIsDead = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Перезапускает уровень, на котором умер игрок
            // SceneManager.LoadScene(1); //ПереЗагружает 1 уровень во время смерти //в качестве параметра так же можно использовать индекс, полученный с инспектора уровней, а так же по названию(стринг)
        }   //Для перезагрузки текущего уровня можно ввести SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        transformingBar.localScale = new Vector3((float)playerHealth / 0.25f, 0.21f, 1f); //делим хп на 25, чтобы максимум был 4, минимум 0 и приводим к дробным, чтобы не округляло

    }

}
