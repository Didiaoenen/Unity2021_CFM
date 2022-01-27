import { UIBase } from "./UIBase";

export function binder(name: string, ...args: any) {
    return function(target: any, key: string | symbol) {
        target[key] = UIBase.GetComponent(target as UIBase, name, args);
    }
}