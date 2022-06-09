local record Point3DType
    -- property
    x: number
    y: number
    z: number

    -- method
	new: function(...: any): Point3DType
    ctor: function(Point3DType, number, number, number)
    dtor: function(Point3DType)
	delete: function(Point3DType)
    PrintXY: function(Point3DType)
    PrintXYZ: function(Point3DType)
end

return Point3DType