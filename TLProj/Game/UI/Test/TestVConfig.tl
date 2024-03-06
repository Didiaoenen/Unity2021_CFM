local WindowNames = require("Game.UI.Config.WindowNames")

local TestV = require("Game.UI.Test.TestV")
local TestVM = require("Game.UI.Test.TestVM")

local TestUI: WindowConfigTable = {
    Name = WindowNames.TestV,
    Layer = 0,
    View = TestV,
    ViewModel = TestVM,
    PrefabName = "TestV",
}

local Config: {string: WindowConfigTable} = {
    TestUI = TestUI,
}

return Config