import { Singleton } from "../common/Singleton";

class MessageObjet {
    public obj: any;
    public listeners: Array<Function>;

    constructor(obj: any, listeners: Array<Function>) {
        this.obj = obj;
        this.listeners = listeners;
    }

    public AddListener(listener: Function) {
        this.listeners.push(listener);
    }
}

export class EventManager extends Singleton {

    private _messageMap: Map<string, MessageObjet>;

    constructor() {
        super();
        this._messageMap = new Map<string, MessageObjet>();
    }

    public AddListener(message: string, obj: any, listener: Function): void {
        let mapValue = this._messageMap.get(message);
        if (mapValue == null) {
            mapValue = new MessageObjet(obj, new Array<Function>());
            this._messageMap.set(message, mapValue);
        }
        mapValue.AddListener(listener);
    }

    public RemoveListener(message: string, obj: any, listener: Function): void {
        let mapValue = this._messageMap.get(message);
        if (mapValue && mapValue.obj == obj) {
            let delArray = new Array<Function>();
            for (let value of mapValue.listeners) {
                if (value == listener) {
                    delArray.push(value);
                }
            }
            for (let value of delArray) {
                mapValue.listeners.splice(mapValue.listeners.indexOf(value), 1);
            }
        }
    }

    public RemoveAllListener(): void {
        this._messageMap.clear();
    }

    public DispatchListener(message: string, ...args: any) {
        let mapValue = this._messageMap.get(message);
        if (message) {
            for (let value of mapValue.listeners) {
                value.call(mapValue.obj, args);
            }
        }
    }
}