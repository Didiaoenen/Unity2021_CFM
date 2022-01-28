import { UnityEngine, TestJS } from "csharp";
import { $typeof } from 'puerts'
import { EventDefine } from "./framework/event/EventDefine";
import { UIImport } from "./framework/ui/UIImport";

class GameMain {

    public name: string;
    
    constructor() {
        
    }

    public Start() {
        try {

            UnityEngine.Debug.Log("GameMain");

            let mainCamera = UnityEngine.GameObject.Find("Main Camera");
            let testJs = mainCamera.GetComponent($typeof(TestJS)) as TestJS;
            let button = testJs.uiRoot.transform.Find("Button").GetComponent($typeof(UnityEngine.UI.Button)) as UnityEngine.UI.Button;
            button.onClick.AddListener(() => {
                this.eventListener();
            });

        } catch (ex) {
            console.log(ex);
        }
    }

    public eventListener() {
        console.log("button pressed..., input is: ");
        console.log(EventDefine.TEST1, EventDefine.TEST2, EventDefine.TEST1 == EventDefine.TEST2);
        console.log(UIImport.CreateUI("hall", "UIHall").name);
        console.log(UIImport.CreateUI("login", "UILogin").name);
    }
}

new GameMain().Start();