import { UnityEngine } from "csharp";
import { G } from "../common/GlobalManager";
import { UIEventListener } from "../event/UIEventListener";
import { UIComponent } from "./UIComponent";
import { UITransBinder } from "./UITransBinder";

export abstract class UIBase {

    public name: string;
    public rootGO: UnityEngine.GameObject;
    public transBinder: UITransBinder;

    private _compMap: Map<string, UIComponent>;
    private _timerMap: Map<string, number>;
    private _uiEventListener: UIEventListener;

    constructor(name: string) {
        this.name = name;
        this._timerMap = new Map<string, number>();
        this._uiEventListener = new UIEventListener(G.EventManager);
        this.transBinder = new UITransBinder();
    }

    public get compMap() {
        if (this._compMap == null) {
            this._compMap = new Map<string, UIComponent>();
        }
        return this._compMap;
    }

    public static GetComponent(obj: UIBase, name: string, ...args: any): UIComponent {
        let uiComp = new UIComponent(obj.rootGO.transform, name, args);
        obj.compMap.set(name, uiComp);
        return uiComp;
    }

    public OnCreate(): void {

    }

    public OnDestroy(): void {
        this.StopAllTimer();
        for (let key in this.compMap) {
            this.compMap.get(key).Destroy();
        }
    }

    public OnShow(): void {

    }

    public OnHide(): void {

    }

    public OnAddEvent(): void {

    }

    public OnRemoveEvent(): void {
        this._uiEventListener.RemoveAllListener();
    }

    public OnAddUIListener(): void {

    }

    public OnRemoveUIListener(): void {

    }

    public OnRefUI(): void {

    }

    //Common Function
    public static CreateUI(name: string, obj: { new(name: string): UIBase }): UIBase {
        return new obj(name);
    }

    public AddEvent(message: string, obj: UIBase, listener: Function): void {
        this._uiEventListener.AddListener(message, obj, listener);
    }

    public StartTimer(name: string): void {
        if (this._timerMap.get(name) == null) {
            let time = G.Timer.StartTimer(name);
            this._timerMap.set(name, time);
        }
    }

    public StopTimer(name: string): void {
        G.Timer.StopTimer(this.GetTimer(name));
        this._timerMap.delete(name);
    }

    public GetTimer(name: string): number {
        return this._timerMap.get(name);
    }

    private StopAllTimer(): void {
        for (let value in this._timerMap) {
            G.Timer.StopTimer(this._timerMap.get(value));
        }
        this._timerMap.clear();
    }
}