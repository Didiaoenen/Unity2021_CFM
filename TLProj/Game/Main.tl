require("Debug.LuaPanda").start("127.0.0.1", 8818)

require("CS.CS")

require("LuaFramework.System")

local TestV = require("Game.UI.Test.TestV")

function Main(): number
    TestV.new():Create()
    -- TestV.new():Create()

    return 1
end

function __G__TRACKBACK__(msg: any)
    error("----------------------------------------")
    error("LUA ERROR: " .. tostring(msg) .. "\n")
    error(debug.traceback())
    error("----------------------------------------")
end

local status, msg = xpcall(Main, __G__TRACKBACK__)
if not status then
    print(msg)
end