local MapExtention = require("LuaFramework.Common.MapExtention")
local TestVConfig = require("Game.UI.Test.TestVConfig")

local Config: {string: {string: WindowConfigTable}} = {
    TestVConfig = TestVConfig,
}

local WindowConfig = class("WindowConfig") as WindowConfigType

function WindowConfig.get:Config(): {string: WindowConfigTable}
    return self.config
end

function WindowConfig:ctor()
    self.config = {}
    MapExtention.Foreach(Config, function(_, configs: {string: WindowConfigTable})
        MapExtention.Foreach(configs, function(_, config: WindowConfigTable)
            self.config[config.Name] = config
        end)
    end)
end

function WindowConfig:__singleton(): WindowConfigType
    return WindowConfig.new()
end

return WindowConfig