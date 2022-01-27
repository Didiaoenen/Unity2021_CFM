import { UnityEngine } from "csharp";
import { Timer } from "../common/Timer";
import { UIEventListener } from "../event/UIEventListener";
import { UIComponent } from "./UIComponent";
import { UITransBinder } from "./UITransBinder";

export class UIBase {

    public name: string;
    public rootGO: UnityEngine.GameObject;
    public transBinder: UITransBinder;

    private _timerMap: Map<string, Timer>;
    private _uiEventListener: UIEventListener;

    constructor(name: string) {
        this._timerMap = new Map<string, Timer>();
        this.transBinder = new UITransBinder();
    }

    public static GetComponent(obj: UIBase, name: string, ...args: any): UIComponent {
        return new UIComponent(obj.rootGO.transform, name, args);
    }

    public OnCreate(): void {

    }

    public OnDestroy(): void {

    }

    public OnShow(): void {

    }

    public OnHide(): void {

    }

    public OnAddEvent(): void {

    }

    public OnRemoveEvent(): void {

    }

    public OnAddUIListener(): void {

    }

    public OnRemoveUIListener(): void {

    }

    public OnRefUI(): void {

    }

    //Common Function
    public StartTimer(name: string): void {

    }

    public StopTimer(name: string): void {

    }

    private StopAllTimer(): void {
        
    }
}