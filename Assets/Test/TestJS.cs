using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Puerts;
using System.IO;

public class TestJS : MonoBehaviour
{
    public GameObject uiRoot;

    JsEnv jsEnv;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        var dataPath = UnityEngine.Application.dataPath;
        var path = Path.Combine(UnityEngine.Application.dataPath, "../TsProj/output/");
        jsEnv = new JsEnv(new DefaultLoader(path), 9229);
        jsEnv.Eval("require('GameMain')");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jsEnv != null)
        {
            jsEnv.Tick();
        }
    }
}
