using UnityEngine;
using System.Collections;

namespace FPS
{
	public class BulletScript : MonoBehaviour,IController
	{
		public int damage;
        //  IC.RegisterObjectPool("Bullet", "Object/BulletCasing/Bullet_Prefab");
        [Range(5, 100)]
		[Tooltip("子弹预制件应该在多长时间后销毁？")]
		public float destroyAfter;
		[Tooltip("如果启用，子弹会在撞击时销毁")]
		public bool destroyOnImpact = false;
		[Tooltip("撞击后子弹被摧毁的最短时间")]
		public float minDestroyTime;
		[Tooltip("撞击后子弹被摧毁的最长时间")]
		public float maxDestroyTime;

		[Header("击中不同物体的四个特效")]
		public Transform bloodImpactPrefabs;
		public Transform metalImpactPrefabs;
		public Transform dirtImpactPrefabs;
		public Transform concreteImpactPrefabs;

        IController IC;
        /*IC.RegisterObjectPool("Blood", "Object/Effect/Blood Impact Prefab");
            IC.RegisterObjectPool("Concrete", "Object/Effect/Concrete Impact Prefab");
            IC.RegisterObjectPool("Dirt", "Object/Effect/Dirt Impact Prefab");
            IC.RegisterObjectPool("Metal", "Object/Effect/Metal Impact Prefab");
            IC.RegisterObjectPool("Explosion", "Object/Effect/Explosion Prefab");*/
		public void InitDamage(int damage)
		{
			this.damage = damage;

        }
        private void Awake()
        {
            IC = this;
        }
        private void OnEnable()
		{
			
			/*   bloodImpactPrefabs = Resources.Load<Transform>("FVX/Blood Impact Prefab");
			   metalImpactPrefabs = Resources.Load<Transform>("FVX/Metal Impact Prefab");
			   dirtImpactPrefabs = Resources.Load<Transform>("FVX/Dirt Impact Prefab");
			   concreteImpactPrefabs = Resources.Load<Transform>("FVX/Concrete Impact Prefab");*/
			//Start destroy timer
			StartCoroutine(DestroyAfter());

			
        }
		
		//If the bullet collides with anything
		private void OnCollisionEnter(Collision collision)
		{
			//If destroy on impact is false, start 
			//coroutine with random destroy timer
			if (!destroyOnImpact)
			{
				StartCoroutine(DestroyTimer());
			}
			//Otherwise, destroy bullet on impact
			else
			{
                // Destroy(gameObject);
                IC.ReturnObject("Bullet", gameObject);
            }


			if (collision.gameObject.tag == "Player")
			{
				Debug.Log("傻逼，生成位置在自己的碰撞体内啊！");


				Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());

			}
			//碰撞的第一个接触点
			//它用于确保实例化的血液效果预制体正确地朝向碰撞面的法线方向
			if (collision.transform.tag == "Blood")
			{
				IC.GetObject("Blood", transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
				//	Instantiate(bloodImpactPrefabs, transform.position,Quaternion.LookRotation(collision.contacts[0].normal));

                //	Destroy(gameObject);
                IC.ReturnObject("Bullet", gameObject);
            }
			
            if ( collision.transform.tag == "Enemy")
            {
                IC.GetObject("Blood", transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
				/*Instantiate(bloodImpactPrefabs, transform.position,
                    Quaternion.LookRotation(collision.contacts[0].normal));*/
				//IC.TriggerEvent("HitEnemy");
				Debug.Log(collision.collider.gameObject.name);
                collision.collider.gameObject.GetComponent<IDamage>().Damage(damage);
                IC.ReturnObject("Bullet", gameObject);// Destroy(gameObject);
            }

            if (collision.transform.tag == "Metal" )
			{
                IC.GetObject("Metal", transform.position, Quaternion.LookRotation(collision.contacts[0].normal));

                //金属
               /* Instantiate(metalImpactPrefabs
					, transform.position,
					Quaternion.LookRotation(collision.contacts[0].normal));*/

                IC.ReturnObject("Bullet", gameObject); //Destroy(gameObject);
			}


			if (collision.transform.tag == "Dirt" )
			{
                IC.GetObject("Dirt", transform.position, Quaternion.LookRotation(collision.contacts[0].normal));

                //泥土
                /*Instantiate(dirtImpactPrefabs, transform.position,
					Quaternion.LookRotation(collision.contacts[0].normal));*/

                IC.ReturnObject("Bullet", gameObject); //Destroy(gameObject);
			}


			if (collision.transform.tag == "Concrete" )
			{
                IC.GetObject("Concrete", transform.position, Quaternion.LookRotation(collision.contacts[0].normal));

                //混凝土
             /*   Instantiate(concreteImpactPrefabs, transform.position,
					Quaternion.LookRotation(collision.contacts[0].normal));*/

                IC.ReturnObject("Bullet", gameObject); //Destroy(gameObject);
			}

			Transform parent = collision.transform.parent;
            if(parent != null)
            {
                if(parent.tag == "Concrete")
				{
                    IC.GetObject("Concrete", transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
                }
                if (parent.tag == "Dirt")
                {
                    IC.GetObject("Dirt", transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
                }
                if (parent.tag == "Metal")
                {
                    IC.GetObject("Metal", transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
                }
                if (parent.tag == "Blood")
                {
                    IC.GetObject("Blood", transform.position, Quaternion.LookRotation(collision.contacts[0].normal));
                }
                IC.ReturnObject("Bullet", gameObject);
            }


            if (collision.transform.tag == "Target")
			{
				//靶子
				collision.transform.gameObject.GetComponent
					<TargetController>().isHit = true;

                IC.ReturnObject("Bullet", gameObject); //Destroy(gameObject);
			}


			if (collision.transform.tag == "ExplosiveBarrel")
			{
				//炸药桶
				collision.transform.gameObject.GetComponent
					<ExplosiveBarrelScript>().explode = true;

                IC.ReturnObject("Bullet", gameObject); //Destroy(gameObject);
			}


			if (collision.transform.tag == "GasTank")
			{
				//气罐
				collision.transform.gameObject.GetComponent
					<GasTankScript>().isHit = true;

                IC.ReturnObject("Bullet", gameObject); //Destroy(gameObject);
			}
		}

		
		private IEnumerator DestroyTimer()
		{

			yield return new WaitForSeconds
				(Random.Range(minDestroyTime, maxDestroyTime));

            IC.ReturnObject("Bullet", gameObject); //Destroy(gameObject);
		}

		private IEnumerator DestroyAfter()
		{

			yield return new WaitForSeconds(destroyAfter);

            IC.ReturnObject("Bullet", gameObject); //Destroy(gameObject);
		}
        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
/*喵~主人想要平衡枪械的伤害和子弹数量，这是一个非常有趣的任务！我们可以以手枪为基础，逐步设计其他枪械的属性。以下是详细的平衡方案喵~

设计原则
以手枪为基准：

手枪：4枪打死一个僵尸（僵尸100血），单发伤害25-33。

备弹：100发。

其他枪械的伤害和备弹：

伤害越高，备弹越少。

伤害越低，备弹越多。

每种枪械的伤害和备弹需要有一定的差异，以增加游戏的多样性。

僵尸血量：

僵尸血量固定为100。

枪械属性设计
1. 手枪（4把）
单发伤害：25-33（4枪打死一个僵尸）。

备弹：100发。

差异：

手枪A：伤害25，备弹100。

手枪B：伤害28，备弹100。

手枪C：伤害30，备弹100。

手枪D：伤害33，备弹100。

2. 冲锋枪（5把）
单发伤害：15-20（整体比手枪低，但射速快）。

备弹：200发。

差异：

冲锋枪A：伤害15，备弹200。

冲锋枪B：伤害16，备弹200。

冲锋枪C：伤害18，备弹200。

冲锋枪D：伤害19，备弹200。

冲锋枪E：伤害20，备弹200。

3. 步枪（3把）
单发伤害：35-45（比手枪高，适合中距离战斗）。

备弹：150发。

差异：

步枪A：伤害35，备弹150。

步枪B：伤害40，备弹150。

步枪C：伤害45，备弹150。

4. 霰弹枪（1把）
单发伤害：50-60（近距离高伤害，但射速慢）。

备弹：50发。

差异：

霰弹枪A：伤害50，备弹50。

5. 狙击枪（3把）
单发伤害：80-100（远距离高伤害，但射速慢）。

备弹：30发。

差异：

狙击枪A：伤害80，备弹30。

狙击枪B：伤害90，备弹30。

狙击枪C：伤害100，备弹30。

枪械属性表
枪械类型	枪械名称	单发伤害	备弹	击杀僵尸所需枪数
手枪	手枪A	25	100	4
手枪	手枪B	28	100	4
手枪	手枪C	30	100	4
手枪	手枪D	33	100	4
冲锋枪	冲锋枪A	15	200	7
冲锋枪	冲锋枪B	16	200	7
冲锋枪	冲锋枪C	18	200	6
冲锋枪	冲锋枪D	19	200	6
冲锋枪	冲锋枪E	20	200	5
步枪	步枪A	35	150	3
步枪	步枪B	40	150	3
步枪	步枪C	45	150	3
霰弹枪	霰弹枪A	50	50	2
狙击枪	狙击枪A	80	30	2
狙击枪	狙击枪B	90	30	2
狙击枪	狙击枪C	100	30	1
设计思路
手枪：

作为基础枪械，伤害适中，备弹充足。

适合新手玩家或备用武器。

冲锋枪：

伤害较低，但备弹多，射速快。

适合清理大量低血量敌人。

步枪：

伤害较高，备弹适中。

适合中距离战斗，平衡输出和续航。

霰弹枪：

高伤害，但备弹少，射速慢。

适合近距离爆发输出。

狙击枪：

超高伤害，但备弹极少，射速慢。

适合远距离精准击杀。

进一步优化
射速和换弹时间：

可以为每种枪械设计不同的射速和换弹时间，进一步增加多样性。

例如：

冲锋枪：射速快，换弹时间短。

狙击枪：射速慢，换弹时间长。

特殊效果：

可以为某些枪械添加特殊效果，如穿透、爆炸、减速等。

配件系统：

允许玩家通过配件（如扩容弹匣、消音器、瞄准镜等）调整枪械属性。

希望这个平衡方案能帮助主人设计出有趣的枪械系统喵~如果还有其他需求，请随时告诉我哦！(=^･ω･^=)*/