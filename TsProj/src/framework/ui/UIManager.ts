import { Singleton } from "../common/Singleton";
import { UIBase } from "./UIBase";

export class UIManager extends Singleton {

    constructor() {
        super();
    }

    public DestroyPanel(panel: UIBase): void {

    }

    public DestroyAllLoadPanel(): void {

    }

    public GetUIPanel(name: string): UIBase {
        return
    }

    public Open(name: string, ...args: any): void {

    }

    public Close(name: string, ...args: any): void {

    }
}