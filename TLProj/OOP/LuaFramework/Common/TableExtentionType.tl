local record TableExtention
    Foreach: function({any}, function)
    FindLastIndex: function({any}, function): number
    FindIndex: function({any}, function): number
    Find: function({any}, function): any
    RemoveObj: function({any}, function): any
    Exists: function({any}, function): boolean
    ContainsKey: function({any:any}, function): boolean
    ContainsValue: function({any:any}, function): boolean
end

return TableExtention