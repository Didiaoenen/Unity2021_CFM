local TableExtention: TableExtentionType = {}

function TableExtention.Foreach(t: {any}, callback: function)
    for i, v in ipairs(t) do
        if callback then
            callback(i, v)
        end
    end
end

function TableExtention.FindLastIndex(t: {any}, callback: function): number
    local tt = {}
    for i, v in ipairs(t) do
        if callback and callback(i, v) then
            table.insert(tt, i)
        end
    end
    return tt[#(t)]
end

function TableExtention.FindIndex(t: {any}, callback: function): number
    local tt = {}
    for i, v in ipairs(t) do
        if callback and callback(i, v) then
            table.insert(tt, i)
            break
        end
    end
    return tt[1]
end

function TableExtention.Find(t: {any}, callback: function): any
    for i, v in ipairs(t) do
        if callback and callback(i, v) then
            return v
        end
    end
end

function TableExtention.RemoveObj(t: {any}, obj: any): any
    for i, v in ipairs(t) do
        if v == obj then
            return table.remove(t, i)
        end
    end
end

function TableExtention.Exists(t: {any}, callback: function): boolean
    for i, v in ipairs(t) do
        if callback and callback(i, v) then
            return true
        end
    end
    return false
end

function TableExtention.ContainsKey(t: {any:any}, key: any): boolean
    for k, _ in pairs(t) do
        if k == key then
            return true
        end
    end
    return false
end

function TableExtention.ContainsValue(t: {any:any}, value: any): boolean
    for _, v in pairs(t) do
        if v == value then
            return true
        end
    end
    return false
end

return TableExtention