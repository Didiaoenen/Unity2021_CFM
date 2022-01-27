export class Singleton {

    private static instance: any;

    public static Instance<T>(obj: { new(): T }): T {
        if (this.instance == null) {
            this.instance = new obj();
        }
        return this.instance;
    }
}