import { EventManager } from "../event/EventManager";
import { Timer } from "./Timer";

export class G {
    public static EventManager = EventManager.Instance(EventManager);
    public static Timer = Timer.Instance(Timer);
}