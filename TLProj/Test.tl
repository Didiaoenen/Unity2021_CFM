
--
local _class: {any:{any:any}} = {}

-- 自定义类型
local record ClassType
	class: number
	instance: number
end

local classType: ClassType = {}
classType.class = 1
classType.instance = 2

local record Class_type
	__init: function(Class_type, ...: any)
	__delete: function(Class_type)
	__cname: string
	__ctype: number
	_class_type: Class_type

	super: any

	testRecord: any

	New: function(...: any): Class_type

	Delete: function(Class_type)

	ToString: function(Class_type)
end

function BaseClass(classname: string, super: any): Class_type
	assert(type(classname) == "string" and #classname > 0)

	-- 生成一个类类型
	local class_type: Class_type = {}

	-- 在创建对象的时候自动调用
	class_type.__cname = classname
	class_type.__ctype = classType.class

	class_type.super = super
	class_type.New = function(...): Class_type
		
		-- 生成一个类对象
		local obj: Class_type = {}
		obj._class_type = class_type
		obj.__ctype = classType.instance
		
		-- 在初始化之前注册基类方法
		setmetatable(obj, { 
			__index = _class[class_type],
		})
		
		-- 调用初始化方法
		do
			local create: function(Class_type, ...: any)
			create = function(c: Class_type, ...: any)
				if c.super then
					create(c.super as Class_type, ...)
				end
				if c.__init then
					c.__init(obj, ...)
				end
			end

			create(class_type, ...)
		end

		-- 注册一个delete方法
		obj.Delete = function(self: Class_type)
			local now_super = self._class_type 
			while now_super ~= nil do	
				if now_super.__delete then
					now_super.__delete(self)
				end
				now_super = now_super.super as Class_type
			end
		end

		return obj
	end

	local vtbl = {}
	_class[class_type] = vtbl

	setmetatable(class_type, {
		__newindex = function(t: any, k: any, v: any)
			vtbl[k] = v
		end
		, 
		__index = vtbl,
	})

	if super then
		setmetatable(vtbl, {
			__index = function(t: any, k: any): any
				local ret = _class[super][k]
				return ret
			end
		})
	end

	return class_type
end

local _1 = BaseClass("_1")

function _1:__init(testRecord)
	self.testRecord = testRecord
end

function _1:ToString()
	print("_11")
	print(self.testRecord)
end

local _11 = BaseClass("_11", _1)
local base = _1

function _11:ToString()
	base.ToString(self)	
end

local _11_ = _11.New("123")

_11_:ToString()


local record Point
	x: number
	y: number
end

local _p1: Point = {x = 1, y = 1}
local _p2: Point = {x = 2, y = 2}

local function keys<K,V>(xs: {K:V}):{V}
	local ks = {}
	for k, v in pairs(xs) do
		table.insert(ks, k)
	end
	return ks
end

local tt = {}
tt[_p1] = _p1
tt[_p2] = _p2

local s = keys(tt)

for k, v in ipairs(s) do
	print(k, v)
end

local type Vec = record
order: number
end

local _v1: Vec = {order = 1}
local _v2: Vec = {order = 2}
local _v3: Vec = {order = 3}

local function mysort<A>(arr: {A}, cmp: function<T>(T, T): boolean)
	table.sort(arr, cmp)
end

local cmp = function(v1: Vec, v2: Vec): boolean
	return v1.order > v2.order
end

local arr = {_v1, _v2, _v3}

mysort(arr, cmp)

for k, v in ipairs(arr) do
	print(k, v.order)
end

print("---------------")