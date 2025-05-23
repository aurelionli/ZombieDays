using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

//public enum Scene { MainMenu=0,GameMap=1,Range=2} //主界面，游戏，靶场
namespace FPS_Manager
{
    public class ObjectSpawnerSystem : MonoBehaviour, ISystem
    {
        private ISystem IC;
        //todo:协程检测，而且协程生成，对象池回收

        [Header("生成设置")]
        public GameObject objectPrefab; // 要生成的预制体
        [Tooltip("最大同时存在数量")] public int maxObjects = 1000;   // 最大同时存在数量
        [Tooltip("每批生成的数量")]public int batchSize = 100;     // 每批生成的数量
        [Tooltip("生成半径（相对于建筑物）")] public float spawnRadius = 50f; // 生成半径（相对于建筑物）
        [Tooltip("检查物资数量的间隔（秒）")] public float checkInterval = 120f; // 检查物资数量的间隔（秒）

        [Header("导航检测")]
      
        public float checkHeight = 10f;  // 检测高度
        public string spawnAreaName = "ObjectSpawn"; // 生成区域的导航层名称

        public Transform spawnerGroup;
        private Transform buildingsParent; // 建筑物父物体（Built）
        private List<GameObject> spawnedObjects = new List<GameObject>();

        private List<string> objectsName = new List<string>();


        private void Awake()
        {
            IC = this;
        }
        private void Start()
        {
            buildingsParent = transform;
            //InvokeRepeating("CheckObjectCount", 0f, checkInterval);
            InitObjectName();
            StartCoroutine(SpawnObjectsRoutine()); // 启动生成协程
            StartCoroutine(CheckObjectCountRoutine()); // 启动检查协程
        }

        private void InitObjectName()
        {
            Debug.Log("初始化");
            //弹药箱
            IC.RegisterObjectPool("ArmsAmmoBox", "Interact/ArmsAmmoBox");
            IC.RegisterObjectPool("HandGunAmmoBox", "Interact/HandGunAmmoBox");
            IC.RegisterObjectPool("ShotGunAmmoBox", "Interact/ShotGunAmmoBox");
            IC.RegisterObjectPool("SMGAmmoBox", "Interact/SMGAmmoBox");
            IC.RegisterObjectPool("SniperAmmoBox", "Interact/SniperAmmoBox");
            IC.RegisterObjectPool("Grenade", "Interact/Grenade"); 
            IC.RegisterObjectPool("SM_Item_Can_07", "Interact/SM_Item_Can_07");

            objectsName.Add("ArmsAmmoBox");
            objectsName.Add("HandGunAmmoBox");
            objectsName.Add("ShotGunAmmoBox");
            objectsName.Add("SMGAmmoBox");
            objectsName.Add("SniperAmmoBox");
            objectsName.Add("Grenade");
            objectsName.Add("SM_Item_Can_07");
        }

        
       private IEnumerator asdasdas()
        {
            while (true) 
            {
                spawnedObjects.RemoveAll(item => item == null || item.activeSelf == false);

                if (spawnedObjects.Count < maxObjects)
                {
                    int objectsToSpawn = Mathf.Min(batchSize, maxObjects - spawnedObjects.Count);
                    for (int i = 0; i < objectsToSpawn; i++)
                    {
                        SpawnNewObject();
                    }
                }
                yield return new WaitForSeconds(1f);
            } 
                
        }
        private IEnumerator SpawnObjectsRoutine()
        {
            while (true)
            {
                // 如果当前物体数量不足，补充一批
                if (spawnedObjects.Count < maxObjects)
                {
                    int objectsToSpawn = Mathf.Min(batchSize, maxObjects - spawnedObjects.Count);
                    for (int i = 0; i < objectsToSpawn; i++)
                    {
                        SpawnNewObject();
                    }
                }

                // 等待一段时间再生成下一批
                yield return new WaitForSeconds(1f); // 每批生成间隔1秒
            }
        }
        private IEnumerator CheckObjectCountRoutine()
        {
            int ia = 0;
            while (true)
            {
                // 清理已销毁的物体
                //  spawnedObjects.RemoveAll(item => item == null);
                spawnedObjects.RemoveAll(item => item==null|| item.activeSelf == false);
                // 补充缺失的物体
                while (spawnedObjects.Count < maxObjects)
                {
                    if(ia ==1000)
                    {
                        Debug.Log("c出现了无限循环");
                        break;
                    }
                    int objectsToSpawn = Mathf.Min(batchSize, maxObjects - spawnedObjects.Count);
                    for (int i = 0; i < objectsToSpawn; i++)
                    {
                        SpawnNewObject();
                    }
                    yield return new WaitForSeconds(1f); // 每批生成间隔1秒
                }

                // 等待指定时间再检查
                yield return new WaitForSeconds(checkInterval);
            }
        }

        private void SpawnNewObject()
        {
            // 随机选择一个建筑物
            Transform randomBuilding = GetRandomBuilding();
            if (randomBuilding == null) return;

            // 在建筑物周围生成随机点
            Vector3 randomPosition = GetRandomPositionAroundBuilding(randomBuilding);

            // 导航网格检测
            if(IsValidNavMeshPosition(randomPosition,out Vector3 surfacePosition))
            {
              //  GameObject newObj = Instantiate(objectPrefab, surfacePosition, Quaternion.identity, spawnerGroup);
                GameObject newObj = GetSelectInstantiateObject(surfacePosition, Quaternion.identity);
                spawnedObjects.Add(newObj);
            }
                
            
        }
        private GameObject GetSelectInstantiateObject(Vector3 pos,Quaternion quaternion)
        {
            int i = Random.Range(0, objectsName.Count);
           // Debug.Log(i + "    " + objectsName[i]);
            return IC.GetObject(objectsName[i], pos, quaternion);

        }
        private Transform GetRandomBuilding()
        {
            if (buildingsParent.childCount == 0) return null;
            return buildingsParent.GetChild(Random.Range(0, buildingsParent.childCount));
        }

        private Vector3 GetRandomPositionAroundBuilding(Transform building)
        {
            Bounds bounds = GetBuildingBounds(building);
            //min坐下后，max右上前
            return new Vector3(
                Random.Range(bounds.min.x + spawnRadius, bounds.max.x - spawnRadius),
                bounds.max.y,
                Random.Range(bounds.min.z + spawnRadius, bounds.max.z - spawnRadius)
            );
        }

        private Bounds GetBuildingBounds(Transform building)
        {
            Collider collider = building.GetComponent<Collider>();
            return collider != null ? collider.bounds : new Bounds(building.position, Vector3.zero);
        }

        private bool IsValidNavMeshPosition(Vector3 position,out Vector3 surfacePosition)//启用，因为通过刚体自然下落就不用检查可达性。
        {
            // 检测导航网格可达性
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, checkHeight, NavMesh.AllAreas))
            {
                // 检查是否在指定的生成区域
                int spawnAreaMask = 1 << NavMesh.GetAreaFromName(spawnAreaName);
              //  return (hit.mask & spawnAreaMask) != 0;
                if((hit.mask & spawnAreaMask) != 0)
                {
                    surfacePosition = hit.position;
                    return true;
                    //射线检测无用，因为我会禁用场景
                    /* if(Physics.Raycast(hit.position+Vector3.up,Vector3.down,out RaycastHit hitt,10f,mask))
                     {
                         surfacePosition = hitt.point;
                         return true; 
                     }*/
                }
            }
            surfacePosition = position;
            return false;
        }

        // 可视化生成范围（调试用）
       private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (buildingsParent != null)
            {
                foreach (Transform child in buildingsParent)
                {
                    Bounds bounds = GetBuildingBounds(child);
                    Gizmos.DrawWireCube(bounds.center, bounds.size + Vector3.one * spawnRadius * 2);
                }
            }
        }
    }
}



