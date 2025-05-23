


using FPS;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;


namespace FPS_Manager
{
    public class EnemySystem :MonoBehaviour,ISystem
    {
        public bool hordeIsTrue;
        ISystem IC;
        public float currentSpawnRadius = 35f; // 生成半径（玩家视野外）
        public int maxZombies = 50;     // 最大僵尸数量
        public float spawnInterval = 5f; // 生成间隔
        

        public float normalSpawnRadius = 35f;
        public int normalMaxZombies = 50;
        public float normalSpawnInterval = 5f;


        public float hormalSpawnRadius = 20f;
        public int hordeMaxZombies = 150;
        public float hordeSpawnInterval = 0.1f;



        public float maxDistanceFromPlayer = 75f;
        public float checkInterval = 10f;

        private Transform player;
        private List<GameObject> activeZombies = new List<GameObject>();

      
        private void Awake()
        {
            IC = this;
            IC.RegisterObjectPool("Zombie", "Enemy/Zombie");
            IC.RegisterEvent("AppearEnemy");
            IC.RegisterEvent("HordeStart");
        //    IC.RegisterEvent("HordeEnd");

            IC.RegisterEvent("PlayerDead");
        }
        void Start()
        {
            IC.StartListening("AppearEnemy", TriggerEvent_EnemyStartedAppear);
            IC.StartListening("HordeStart", TriggerEvent_HordeTimeStarted);
           // IC.StartListening("HordeEnd", TriggerEvent_HordeTimeSOver);
            IC.StartListening("PlayerDead", TriggerEvent_PlayerDead);
        }
      
       
        private void TriggerEvent_PlayerDead(object data)
        {
            StopAllCoroutines(); CleanAllZombie();
        }

        private void  CleanAllZombie()
        {
            foreach (var zombie in activeZombies)
            {
                zombie.GetComponent<EnemyController>().PlayerDead();
               
            }
            activeZombies.Clear();
        }

        private void OnDestroy()
        {
            IC.StopListening("AppearEnemy", TriggerEvent_EnemyStartedAppear);
            IC.StopListening("HordeStart", TriggerEvent_HordeTimeStarted);
           // IC.StopListening("HoodeEnd", TriggerEvent_HordeTimeSOver);
            IC.StopListening("PlayerDead", TriggerEvent_PlayerDead);

            IC.UnRegisterEvent("AppearEnemy");
            IC.UnRegisterEvent("HordeStart");
          //  IC.UnRegisterEvent("HordeEnd");

            IC.UnRegisterEvent("PlayerDead");
        }
        private void TriggerEvent_EnemyStartedAppear(object data)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Log("开始出现僵尸协程执行");
            StartCoroutine(SpawnZombies());
            StartCoroutine(CheckZombiesDistance());
  
        }
        private void TriggerEvent_HordeTimeStarted(object data)
        {
            IC.PlayerBGMusic("Combat");
            IC.PlaySFX("HordeStarted",null);
            hordeIsTrue = true;
            maxZombies = hordeMaxZombies;
            spawnInterval = hordeSpawnInterval;
            foreach(GameObject item in activeZombies)
            {
                SetAngry(item);
              
            }
        }
        private void TriggerEvent_HordeTimeSOver(object data)
        {
            IC.PlayerBGMusic("Background");
            hordeIsTrue = false;
            maxZombies = hordeMaxZombies;
            spawnInterval = hordeSpawnInterval;
            foreach (GameObject item in activeZombies)
            {
                SetAngryOver(item);
  
            }
        }
        void Update()
        {
           /* foreach (var zombie in activeZombies)
            {
                if (zombie != null)
                {
                    float distance = Vector3.Distance(zombie.transform.position, player.position);
                    if (distance > maxDistanceFromPlayer)
                    {
                        // 将僵尸传送到玩家附近
                        Vector3 newPos = GetRandomSpawnPosition();
                        zombie.transform.position = newPos;
                    }
                }
            }*/
        }
        IEnumerator CheckZombiesDistance()
        {
            while (true)
            {
                // 每隔10秒检测一次
                yield return new WaitForSeconds(checkInterval);

                // 遍历所有僵尸
                for (int i = activeZombies.Count - 1; i >= 0; i--)
                {
                    GameObject zombie = activeZombies[i];
                    if (zombie != null)
                    {
                        float distance = Vector3.Distance(zombie.transform.position, player.position);
                        if (distance > maxDistanceFromPlayer)
                        {
                            zombie.gameObject.SetActive(false);

                            // 将僵尸重新定位到玩家附近
                            Vector3 newPos = GetRandomSpawnPosition();
                            if (newPos != Vector3.zero)
                            {
                                zombie.gameObject.SetActive(true);
                                
                                zombie.transform.position = newPos;
                                Debug.Log("僵尸重新定位: " + zombie.name);
                            }
                        }
                    }
                    else
                    {
                        // 如果僵尸已被销毁，从列表中移除
                        activeZombies.RemoveAt(i);
                    }
                }
            }
        }
        IEnumerator SpawnZombies()
        {
            while (true)
            {
                while(activeZombies.Count < maxZombies)
                {
                    // 在玩家视野外生成
                    Vector3 spawnPos = GetRandomSpawnPosition();
                    if (spawnPos != Vector3.zero)
                    {
                        GameObject zombie = IC.GetObject("Zombie", spawnPos, Quaternion.identity);
                        int i = Random.Range(1, 50);
                        zombie.transform.GetChild(i).gameObject.SetActive(true);
                        if (hordeIsTrue)
                        {
                            SetAngry(zombie);
                        }
                        else
                        {
                            SetAngryOver(zombie);
                        }
                        //GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
                        //GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
                        activeZombies.Add(zombie);
                    }
                }
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        private void SetAngry(GameObject zombie)
        {
            zombie.GetComponent<EnemyController>().SetAngry();
            zombie.GetComponent<EnemyController>().health = 25;
        }
        private void SetAngryOver(GameObject zombie)
        {
            zombie.GetComponent<EnemyController>().AngryOver();
            zombie.GetComponent<EnemyController>().health = 50;
        }
        Vector3 GetRandomSpawnPosition()
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * currentSpawnRadius;
            Vector3 spawnDir = new Vector3(randomCircle.x, 0, randomCircle.y);
            Vector3 spawnPos = player.position + spawnDir;

            // 确保生成点在NavMesh上
            
            if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return Vector3.zero; // 无效位置需重新计算
        }
    }
}
/*喵~NavMesh.SamplePosition 是 Unity 中用于在 NavMesh（导航网格）上查找最近可行走位置的方法。它可以帮助你在生成僵尸或其他 AI 实体时，确保生成点位于可行走的区域，避免生成在墙壁、悬崖或其他不可行走的地方。

1. NavMesh.SamplePosition 的作用
功能：

在 NavMesh 上查找距离给定位置最近的可行走点。

如果找到有效位置，返回 true，并将结果存储在 NavMeshHit 中。

如果未找到有效位置，返回 false。

典型用途：

动态生成 AI 实体时，确保生成点位于可行走区域。

检测某个位置是否在 NavMesh 上。

2. 方法签名
csharp
复制
public static bool SamplePosition(
    Vector3 sourcePosition, // 输入位置
    out NavMeshHit hit,     // 输出结果
    float maxDistance,      // 最大搜索距离
    int areaMask            // 允许的区域掩码
);
3. 参数说明
参数	说明
sourcePosition	输入位置，表示你想要查找的起点。
hit	输出结果，包含找到的最近可行走点的信息（位置、法线等）。
maxDistance	最大搜索距离，表示从 sourcePosition 开始搜索的范围。
areaMask	区域掩码，用于指定允许的区域类型（如 Walkable、Jump 等）。
4. 返回值
true：找到有效位置，结果存储在 hit 中。

false：未找到有效位置。

*/