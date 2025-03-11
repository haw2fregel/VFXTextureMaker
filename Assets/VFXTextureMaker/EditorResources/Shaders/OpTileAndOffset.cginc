float2 _TileOffsetCount;
float2 _TileOffsetPosMinMaxX;
float2 _TileOffsetPosMinMaxY;
float2 _TileOffsetScaleMinMax;
float2 _TileOffsetScaleMinMaxX;
float2 _TileOffsetScaleMinMaxY;
float2 _TileOffsetRotateMinMax;
int _TileAndOffsetBlendMode;

float4 TileAndOffset(int2 id)
{
    float4 result = 0;
    float2 uv = IdToUV(id);
    float2 offset = float2(0, 0);
    float2 scale = float2(0, 0);
    float rotate = 0;
    float random = 0;
    for (float x = 0; x < _TileOffsetCount.x; x++)
    {
        for (float y = 0; y < _TileOffsetCount.y; y++)
        {
            random = hash21(float2(x, y));
            offset.x = RandomRemap(random, _TileOffsetPosMinMaxX.x, _TileOffsetPosMinMaxX.y);
            offset.y = RandomRemap(random, _TileOffsetPosMinMaxY.x, _TileOffsetPosMinMaxY.y);
            scale.x = RandomRemap(random, _TileOffsetScaleMinMax.x, _TileOffsetScaleMinMax.y) * RandomRemap(random, _Scal_TileOffsetScaleMinMaxXeMinMaxX.x, _TileOffsetScaleMinMaxX.y);
            scale.y = RandomRemap(random, _TileOffsetScaleMinMax.x, _TileOffsetScaleMinMax.y) * RandomRemap(random, _TileOffsetScaleMinMaxY.x, _TileOffsetScaleMinMaxY.y);
            rotate = RandomRemap(random, _TileOffsetRotateMinMax.x, _TileOffsetRotateMinMax.y);

            int2 intId = UVToID(saturate((UVRotate(uv, float2(0.5, 0.5), rotate) + float2(x, y)) * scale + offset));
            uint2 uintId = uint2(asuint(intId.x), asint(intId.y));
            Blend(result, _Buffer[uintId], _TileAndOffsetBlendMode);
        }
    }
    return result;
}

float RandomRemap(float randam, float min, float max)
{
    return random * (_OffsetMinMaxX.y - _OffsetMinMaxX.x) - _OffsetMinMaxX.x;
}