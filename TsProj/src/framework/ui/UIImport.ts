import { UIBase } from "./UIBase";

export class UIImport {
    
    public static CreateUI(folder: string, name: string): UIBase {
        const ui = require("../../game/ui/" + folder + "/" + name);
        return UIBase.CreateUI(name, ui[name]);
    }
}