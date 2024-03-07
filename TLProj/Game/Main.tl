local luaPanda = require("Debug.LuaPanda") as LuaPandaType
luaPanda.start("127.0.0.1", 8818)

require("CS.CS")

require("OOP.Class")

require("Tools.ToolFunction")

local TestV = require("Game.UI.Test.TestV")

local WindowNames = require("Game.UI.Config.WindowNames")
local WindowManager = require("LuaFramework.Views.WindowManager")

global function Main(): number
    -- TestV.new():Create()

    WindowManager.Instance:OpenWindow(WindowNames.TestV)

    return 1
end

local function __G__TRACKBACK__(msg: any)
    error("----------------------------------------")
    error("LUA ERROR: " .. tostring(msg) .. "\n")
    error(debug.traceback())
    error("----------------------------------------")
end

local status, msg = xpcall(Main, __G__TRACKBACK__)
if not status then
    print(msg)
end