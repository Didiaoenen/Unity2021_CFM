class EventNum {
    private static _num = 1;

    public static get num() {
        return EventNum._num++;
    }
}

function Num(): number {
    return EventNum.num;
}

export enum EventDefine {
    TEST1 = Num(),
    TEST2 = Num(),
}