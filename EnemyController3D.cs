using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Библиотека для AI, позволяет ИИ ходить по NavMesh и т.д.
using UnityEngine.UI;

public class EnemyController3D : MonoBehaviour
{
    //public GameObject Enemy;
    public GameObject target; //default target= player
    public float stopRadius; //радиус в котором остановится, желательно, чтобы позволял мобу бить хитбокосм!
    public float distanceToTarget;
    public float enemyViewDistance; //Радиус, на котором враг увидит игрока, добавил для будущих возможностей
    public float enemyHp = 1f;
    public float speed = 50f;
    public Image hpBar;
    public GameObject bloodParcicleEffect; //ссылка на объект с эффектом брызг кровушки 
    private float stunTime; // время, которое враг будет на месте после получения удара
    public float startStunTime;
    private bool faceToRight = false; //изначально npc смотрит влево
    private NavMeshAgent navMeshAgent;
    //private Rigidbody rb;
    private Animator animatorController;
    private CharacterController _moveControl;
    private EnemyMeleeAttackA meleeAttack1; //1ая базисная атака
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        meleeAttack1 = GetComponent<EnemyMeleeAttackA>();
        target = GameObject.FindWithTag("Player"); //теперь при инициализации моба он ищет игрока и ставит его в цель(сделано для того, чтобы заспавненные мобы выбирали цель, если они создаются из префаба)
        navMeshAgent = GetComponent<NavMeshAgent>();
        animatorController = GetComponent<Animator>(); //В аниматоре для содействия с навмешем врубить Apply Root Motion
        this._moveControl = this.gameObject.GetComponent<CharacterController>(); //пример стиля C# (выглядит отвратно) зачем явно указывать this? и this.gameObject?
    }
    public void enemyTakeDamage(float amountDamage)
    {
        stunTime = startStunTime; //получили стан
        Instantiate(bloodParcicleEffect, transform.position, Quaternion.identity); //каждый раз копирует объект эффекта крови и создает его на месте нпц при получении им урона 
        enemyHp -= amountDamage;
        Debug.Log("Enemy take + " + amountDamage + " Damage");
    }

    void reverseFace() //функция смены взгляда в другую сторону  
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z); //Vector из 3 х элементов(хоть и 2д элемент) но плэйр использует так же z, смена х на -1 поворачивает спрайт !
        //transform.Rotate(0f, 180f, 0f);
    }

    void FixedUpdate()
    {
        
        distanceToTarget = Vector3.Distance(target.transform.position, this.transform.position); //This можно убрать, для будущего понимания
        if(target.transform.position.x < transform.position.x && faceToRight) //Для разворота лица врага
        {
            reverseFace();
            faceToRight = !faceToRight;
        }else if (target.transform.position.x > transform.position.x && !faceToRight)
        {
            reverseFace();
            faceToRight = !faceToRight;
        }
        if (stunTime <= 0) //двигаемся,если нету стана
        {
            if (distanceToTarget > enemyViewDistance) //Если игрок вне зоны видимости врага
            {
                meleeAttack1.canIattack(false);
                animatorController.SetTrigger("enemyIdle");
                navMeshAgent.enabled = false; //вырубаем за ненадобностью
            }
            if (distanceToTarget < enemyViewDistance && distanceToTarget > stopRadius)
            {
                meleeAttack1.canIattack(false);
                animatorController.SetTrigger("enemyRun");
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(target.transform.position);
                _moveControl.Move(navMeshAgent.velocity.normalized * speed * Time.deltaTime); //Передвигаемся с помощью charaqcterController, velocity это ускорение, берется из параметров навмешагента,выставляемых в unity inspector, могу выставить их навмешагенту сам через код
            }
            if (distanceToTarget < stopRadius) // у NavMESH необходимо указать  Stopping Distance на пару единиц меньше , чтобы не дергался у края  радиуса атаки !!
            {
                animatorController.SetTrigger("enemyIdle");
                meleeAttack1.canIattack(true);
                //Теперь не можем указать навмешу , насколько близко подойти к наМ, так как его StoppingDistance не влияет на движение characterControll!
                //Придумать, как заводить чуть глубже StopRadius
                //_moveControl.Move(navMeshAgent.velocity.normalized * Time.deltaTime); //нормальзуем вектор(делаем длиной 1) и на него немного заводим вглубь атаки
                //Врубил, чтобы заводил его чуть чуть глубже радиуса атаки //navMeshAgent.enabled = false; //вырубаем за ненадобностью// Вырубается и врубается туда сюда слишком часто 
            }
        }
        else
        {
            stunTime -= Time.deltaTime;
        }
        if(enemyHp <= 0f)  //Уничтожаем объект, если он умер!
        {
            animatorController.SetTrigger("enemyDie");
            Destroy(gameObject , 0.6f); //Задерживаем уничтожение объекта на время анимации смерти (время подобрал вручную, можно через код, но длинно и ужасно)
        }

        hpBar.fillAmount = enemyHp;
    }
}
