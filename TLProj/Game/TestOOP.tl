-----------------------------------------------
require("OOP.Class");
require("Game.Define");

local Point = class() as PointType;
Point.x = 0;
Point.y = 0;

function Point:ctor(x: number,y: number)
    if x and y then
        self.x = x;
        self.y = y;
    end
end

function Point:dtor()
    print(self,"Point已析构。")
end

function Point:PrintXY()
    print("x = " .. self.x);
    print("y = " .. self.y);
end

-- Point3D继承自Point。
local Point3D = class(Point) as Point3DType;
Point3D.z = 0;

function Point3D:ctor(x: number,y: number,z: number)
    Point.ctor(self,x,y);
    if z then
        self.z = z;
    end
end

function Point3D:dtor()
    -- 不要再调用Point的析构函数，它会被自动调用。
    print(self,"Point3D已析构。")
end

function Point3D:PrintXYZ()
    self:PrintXY();
    print("z = " .. self.z);
end

local p3d = Point3D.new(1,2,3);
p3d:PrintXY()-- x = 1 y = 2
p3d:PrintXYZ()-- x = 1 y = 2 z = 3

-- table: xxxxxxxx Point3D已析构。
-- table: xxxxxxxx Point已析构。
p3d:delete();


-------------------------------------------------------
local Test = class() as TestType;

-- 使用public修饰PrintMe方法和data成员。
Test.public.data = "123";
Test.public.state = State.None;

function Test.public:PrintMe()
    print(self.data);
end

local test = Test.new();
test:PrintMe();-- "123"
print(test.data);-- "123"
print(test.state);-- "123"


------------------------------------------------------------