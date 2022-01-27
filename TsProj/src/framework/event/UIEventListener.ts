import { EventManager } from "./EventManager";

class EventObject {
    public message: string;
    public obj: any;
    public listener: Function;

    constructor(message: string, obj: any, listener: Function) {
        this.message = message;
        this.obj = obj;
        this.listener = listener;
    }
}

export class UIEventListener {

    private _manager: EventManager;
    private _eventArray: Array<EventObject>;

    constructor(manager: EventManager) {
        this._manager = manager;
        this._eventArray = new Array<EventObject>();
    }

    public AddListener(message: string, obj: any, listener: Function): void {
        this._manager.AddListener(message, obj, listener);
        this._eventArray.push(new EventObject(message, obj, listener));
    }

    public RemoveListener(message: string, obj: any, listener: Function): void {
        this._manager.RemoveListener(message, obj, listener);
        this.RemoveEvent(message, obj, listener);
    }

    public RemoveAllListener(): void {
        for (let value of this._eventArray) {
            this._manager.RemoveListener(value.message, value.obj, value.listener);
        }
    }

    private RemoveEvent(message: string, obj: any, listener: Function): void {
        let delArray = new Array<EventObject>();
        for (let value of this._eventArray) {
            if (value.message == message && value.obj == obj && value.listener == listener) {
                delArray.push(value);
            }
        }
        for (let value of delArray) {
            let index = this._eventArray.indexOf(value);
            this._eventArray.splice(index, 1);
        }
    }
}