
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ABAsset : MonoBehaviour
{
    //AB包管理器让外部更方便的进行资源加载
    private Dictionary<string, AssetBundle> abDir = new Dictionary<string, AssetBundle>();
    //主包,一个平台对应一个主包
    private AssetBundle mainAB = null;
    //依赖信息
    private AssetBundleManifest manifest = null;


    //因为加载路径不一定是stringmeaning，所以路径定义成一个属性方便更改
    private string PathUrl
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }
    //主包名也可能
    private string MainABName
    {
        get
        {
            return "主包名";
        }
    }

    //同步加载方法
    //加载AB包
    private void LoadDependencies(string abName)
    {
        //加载AB包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //获取依赖包信息
        AssetBundle ab;
        string[] strs = manifest.GetAllDependencies(abName);
        foreach (string str in strs)
        {
            //判断包是否加载过
            if (!abDir.ContainsKey(str))
            {
                ab = AssetBundle.LoadFromFile(PathUrl + str);
                abDir.Add(str, ab);
            }
        }
        //加载资源来源包
        if (abDir.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(PathUrl + abName);
            abDir.Add(abName, ab);
        }
    }
    //不指定类型
    public object LoadRes(string abName, string resName)
    {

        LoadDependencies(abName);
        //加载资源
        return abDir[abName].LoadAsset(resName);//根据名字加载 返回的是object

    }
    //根据type指定类型
    public object LoadRes(string abName, string resName, System.Type type)
    {
        LoadDependencies(abName);
        return abDir[abName].LoadAsset(resName, type);
    }
    //根据泛型加载  where T:Object是因为LoadAsset<T> 的T要求继承自Object
    public T LoadRes<T>(string abName, string resName) where T : Object
    {
        LoadDependencies(abName);
        return abDir[abName].LoadAsset<T>(resName);
    }
    //异步加载方法
    //加载AB包
    private IEnumerator LoadDependenciesAsync(string abName)
    {
        //主包为空，则异步加载并且回调
        if (mainAB == null)
        {
            yield return LoadAssetBundleAsync(MainABName, ab =>
            {
                mainAB = ab;
                manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            });
        }
        //获得包的依赖包信息,并且加载放入字典
        string[] dependencies = manifest.GetAllDependencies(abName);
        foreach (string str in dependencies)
        {
            if (!abDir.ContainsKey(str))
            {
                yield return LoadAssetBundleAsync(str, ab => abDir.Add(str, ab));
            }
        }
        //如果目标包没有加载
        if (!abDir.ContainsKey(abName))
        {
            yield return LoadAssetBundleAsync(abName, ab => abDir.Add(abName, ab));
        }
    }
    //异步加载单个包。
    private IEnumerator LoadAssetBundleAsync(string abName, System.Action<AssetBundle> onLoadComplete)
    {
        AssetBundleCreateRequest a = AssetBundle.LoadFromFileAsync(PathUrl + abName);

        yield return a;

        onLoadComplete(a.assetBundle);
    }

    //这里AB包没有异步加载，只是从AB包中加载资源是。
    //不指定类型
    public void LoadResAsync(string abNanme, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadRes(abNanme, resName, callBack));
    }
    private IEnumerator ReallyLoadRes(string abName, string resName, UnityAction<Object> callBack)
    {
        yield return LoadDependenciesAsync(abName);
        if (!abDir.ContainsKey(abName))
        {
            Debug.LogError("异步加载AB包失败");
            yield break;
        }
        AssetBundleRequest abr = abDir[abName].LoadAssetAsync(resName);
        yield return abr;
        if (abr.asset == null)
        {
            Debug.LogError($"资源加载为空:{abName}");
        }
        callBack(abr.asset);
    }
    //根据Type异步加载
    public void LoadResAsync(string abNanme, string resName, System.Type type, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadRes(abNanme, resName, type, callBack));
    }
    private IEnumerator ReallyLoadRes(string abName, string resName, System.Type type, UnityAction<Object> callBack)
    {
        LoadDependencies(abName);
        AssetBundleRequest abr = abDir[abName].LoadAssetAsync(resName, type);
        yield return abr;
        if (abr.asset == null)
        {
            Debug.LogError($"资源加载为空:{abName}");
        }
        callBack(abr.asset);
    }
    //根据泛型指定
    public void LoadResAsync<T>(string abNanme, string resName, UnityAction<T> callBack) where T : Object
    {
        StartCoroutine(ReallyLoadRes<T>(abNanme, resName, callBack));
    }
    private IEnumerator ReallyLoadRes<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        LoadDependencies(abName);
        AssetBundleRequest abr = abDir[abName].LoadAssetAsync<T>(resName);
        yield return abr;
        if (abr.asset == null)
        {
            Debug.LogError($"资源加载为空:{abName}");
        }
        callBack(abr.asset as T);
    }
    //单个包卸载
    public void UnLoad(string abName)
    {
        if (abDir.ContainsKey(abName))
        {
            abDir[abName].Unload(false);
            abDir.Remove(abName);
        }
    }
    //所有包卸载
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDir.Clear();
        mainAB = null;
        manifest = null;
    }
















    //不能重复加载AB包
    private void Start()
    {
        //1.加载AB包   //Application.streamingAssetsPath 就是copy的文件夹
        AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "目标预制体包名");
        //2.加载AB包中的资源
        //只使用名字加载，会出现同名不同类型资源分不清的问题
        //建议 泛型加载，or type指定
        GameObject obj = ab.LoadAsset<GameObject>("MINGZI");
        GameObject obj2 = ab.LoadAsset("s", typeof(GameObject)) as GameObject;

        //卸载所有AB包，True的意思是顺便把场景上AB包的资源删除了。
        //注意，预制体的话是赋值一份到场景，但是贴图那些都是引用AB包，
        //所以true删除的话会出现紫色物体，而不是i整个删除
        AssetBundle.UnloadAllAssetBundles(false);
        //还有删除自己的
        ab.Unload(false);




        //AB包依赖，举个例子一个预制体材质球，是正方形预制体的材质
        //AB包中的资源使用了非包内的资源，他会默认的把它打包到同一个包中
        //如果它是在另一个包中，会丢失，。
        //AB包以来-一个资源身上用到了别的AB包中的资源，这个时候会出现资源丢失
        //这个时候需要把依赖包一起加载，但不用加载资源。
        //依赖包的关键知识点--获取主包 获得以来信息

        //1.加载主包
        AssetBundle main = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "主包名字就是Assbundle/主包名/资源包名/资源名");
        //2.加载主包的固定文件   //固定句式和string
        AssetBundleManifest abManifest = main.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        //3.从固定文件中得到依赖信息  //就是这个包的所有依赖包的名字
        string[] strs = abManifest.GetDirectDependencies(Application.streamingAssetsPath + "/" + "资源包名");
        //得到了依赖包的名字
        foreach (string str in strs)
        {
            //加载依赖包
            AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + str);
        }




    }

    private Image sl;
    //异步加载
    IEnumerator LoadABRes(string name, string resName)
    {
        //1.加载AB包
        AssetBundleCreateRequest a = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + "目标预制体包名");

        yield return a;//等待加载完
        //2.加载AB包中的资源
        AssetBundleRequest b = a.assetBundle.LoadAssetAsync(resName, typeof(Sprite));
        yield return b;//等待加载完
        sl = b.asset as Image;

    }

}
