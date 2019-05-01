using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// делаю стрельбу через Raycast  //Вешаю на отдельный объект представляющий собой оружие НО САМО НАРИСОВАННОЕ ОРУЖИМЕ У МЕНЯ БУДЕТ У СПРАЙТА МОДЕЛИ ИГРОКА В АНИМАЦИИ
/// ИХ ПОЗИЦИИ ДОЛЖНЫ СОВПАДАТЬ НА ГЛАЗ ИГРОКА!!
/// </summary>
public class PlayerShootA : MonoBehaviour
{
    public float shootDamage;
    public float startTimeBetweenShoot; // в секундах по ходу
    private float curTimeBetweenShoot;
    public float shootRange;
    public Transform gunEnd;//К нему цыпляем пустой GameObject обозначающий позицию оружия  //указываем ствол, с которого будем стрелять, это нужно для дальнейшей прописовки вектора , по которому пустим спрайт стрельбы
    //public LayerMask layerMaskOfEnemy; //Указываем слой layer mask врагов

    private bool buttonClicked = false;
    private WaitForSeconds howLongShootVisible = new WaitForSeconds(0.1f); //сколько в секундах будет виден сам спрайт выстрела
    private AudioSource shootAudio;
    private LineRenderer shootLine;//В ИНСПЕКТОРЕ ОН ДОЛЖЕН БЫТЬ DISABLED! //рисуем спрайт(линию) выстрела Делает линию между 2мя точками в 3D пространстве
    private Animator animatorController;
    // Start is called before the first frame update
    void Start()
    {
        shootLine = GetComponent<LineRenderer>();
        shootAudio = GetComponent<AudioSource>();
        animatorController = GetComponentInParent<Animator>();
    }

    public void onClickShootButton()
    {
        if (curTimeBetweenShoot <= 0)
        {
            curTimeBetweenShoot = startTimeBetweenShoot;
            animatorController.SetTrigger("playerAttack");
            buttonClicked = true;
        }
        /*
        else
        {
            curTimeBetweenShoot -= Time.deltaTime;
        }*/
    }

    void FixedUpdate()
    {
        Vector3 debugLineOrigin = gunEnd.transform.position; //Для дебагинга линии выстрела
        curTimeBetweenShoot -= Time.deltaTime;
        if (buttonClicked) //TODO поменять на глобальное время Time.time добавляемое к текущему кулдауну и к его обновлению с общим
        {
            //curTimeBetweenShoot = startTimeBetweenShoot;

            StartCoroutine(shootEffect()); //Запускаем корутину для звуков и прорисовки выстрела

            Vector3 raycastOrigin = gunEnd.transform.position;  //Начальная точка нашего рэйкаста ,у меня 3D имитирует 2D , так что спрайтовая линия и сам Рэйкаст будут абсолютно одинаковыми по координатам
            // но в 3d играх от первого лица спрайт выстрела идет от конца оружия до центра экрана на дальней плоскости камеры обзора, а сам рейкаст идет от центра ближней плоскости до центра дальней
            RaycastHit hit; //Будет держать в себе инфу о том, попал ли в гэймобъекты с коллайдерами 

            //теперь объявляю начальную и конечные позиции линии выстрела
            shootLine.SetPosition(0, gunEnd.position);
            //Далее используем Physics.raycast (return true ,если куда то попали) 
            if (Physics.Raycast(raycastOrigin, gunEnd.transform.right,out hit, shootRange)) //если рэйкаст куда то попал (выстрел) 1ый параметр -точка начала рэйкаста, 2ой это вектор направления
            { /// out hit позволяет методу Raycast возвращать помимо true false попадания так же и доп данное hit , которое можно использовать далее
                shootLine.SetPosition(1, hit.point); //2 точкой нашего спрайта линии выстрела будет место попадания
                EnemyController3D enemyOnTarget = hit.collider.GetComponent<EnemyController3D>();
                if(enemyOnTarget != null)
                {
                    enemyOnTarget.enemyTakeDamage(shootDamage);
                }
                //так же можно обратиться к rigitbody цели и добавить силу (если есть ригидбоди)
            }
            else //если не попал
            {//линию выстрела все равно надо закончить, закончим дальней точкой выстрела
                shootLine.SetPosition(1, raycastOrigin + (gunEnd.transform.right * shootRange)); 
            }
            buttonClicked = false;
        }
        /*else //кулдаун стрельбы, уменьшаем
        {
            curTimeBetweenShoot -= Time.deltaTime;
        }*/
        Debug.DrawRay(debugLineOrigin, gunEnd.transform.right * shootRange, Color.green); //Для отображения линии выстрела в дебагинге
    }

    private IEnumerator shootEffect() //рисуем эффект выстрела исользуя КОРУТИН Coroutine
    {
        shootAudio.Play();
        shootLine.enabled = true;
        yield return howLongShootVisible; //тут говорим нашему корутину подождать и вернуться через new WaitForSeconds() значение ( у нас это уже забито)
        //таким образом наш shootLine просуществует отрезок времени
        shootLine.enabled = false;
    }
}
