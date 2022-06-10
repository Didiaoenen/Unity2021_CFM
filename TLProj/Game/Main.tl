

require("Debug.LuaPanda").start("127.0.0.1", 8818)

require("CS.CS")

require("OOP.Class")

require("LuaFramework.Global")

function Main(): any
    -- require("Game.TestOOP")

    WindowManager:Open("Game.TestV")
    WindowManager:Open("Game.TestV")

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