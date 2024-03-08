local WindowEnum = require("LuaFramework.Views.WindowEnum")
local WindowNames = require("Game.UI.Config.WindowNames")

local TestV = require("Game.UI.Test.TestV")
local TestVM = require("Game.UI.Test.TestVM")

local TestViewConfig: ViewConfigTable = {
    Name = WindowNames.TestV,
    PrefabPath = "TestV",
    ViewType = WindowEnum.ViewType.VIEW,
    View = TestV as ViewBaseType,
}

local TestWindowConfig: WindowConfigTable = {
    Name = WindowNames.Test,
    WindowType = WindowEnum.WindowType.FULL,
    ViewConfig = TestViewConfig,
    ViewModel = TestVM as ViewModelBaseType,
}

local WindwoConifg: {WindowConfigTable} = {
    TestWindowConfig,
}

local ViewConfig: {ViewConfigTable} = {
}

return {WindwoConifg, ViewConfig}