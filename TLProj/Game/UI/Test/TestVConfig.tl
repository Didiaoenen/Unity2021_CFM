local WindowNames = require("Game.UI.Config.WindowNames")
local WindowEnum = require("LuaFramework.Views.WindowEnum")

local TestV = require("Game.UI.Test.TestV")
local TestVM = require("Game.UI.Test.TestVM")

local TestVConfig: WindowConfigTable = {
    Name = WindowNames.TestV,
    PrefabPath = "TestV",
    WindowType = WindowEnum.WindowType.FULL,
    View = TestV as ViewBaseType,
    ViewModel = TestVM as ViewModelBaseType,
}

local Config: {string: WindowConfigTable} = {
    [WindowNames.TestV] = TestVConfig,
}

return Config