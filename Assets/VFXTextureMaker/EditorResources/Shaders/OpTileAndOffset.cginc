float2 _TileOffsetCount;
float2 _TileOffsetPosMinMaxX;
float2 _TileOffsetPosMinMaxY;
float2 _TileOffsetScaleMinMax;
float2 _TileOffsetScaleMinMaxX;
float2 _TileOffsetScaleMinMaxY;
float2 _TileOffsetRotateMinMax;
int _TileAndOffsetBlendMode;
int _TileAndOffsetRandomSeed;

float RandomRemap(float random, float min, float max)
{
    return random * (max - min) + min;
}

float4 TileAndOffset(int2 id)
{
    float4 result = 0;
    float2 uv = IdToUV(id);
    float2 halfcount = (_TileOffsetCount - 1) / 2;
    float2 offset = float2(0, 0);
    float2 scale = float2(0, 0);
    float rotate = 0;
    float random = 0;
    for (float x = -_TileOffsetCount.x; x < _TileOffsetCount.x; x++)
    {
        for (float y = -_TileOffsetCount.y; y < _TileOffsetCount.y; y++)
        {
            random = frac(hash21(float2((x + 1) * _TileAndOffsetRandomSeed, (y + 1) * _TileAndOffsetRandomSeed)));
            offset.x = RandomRemap(random, _TileOffsetPosMinMaxX.x, _TileOffsetPosMinMaxX.y);
            offset.y = RandomRemap(random, _TileOffsetPosMinMaxY.x, _TileOffsetPosMinMaxY.y);
            scale.x = RandomRemap(random, _TileOffsetScaleMinMax.x, _TileOffsetScaleMinMax.y) * RandomRemap(random, _TileOffsetScaleMinMaxX.x, _TileOffsetScaleMinMaxX.y);
            scale.y = RandomRemap(random, _TileOffsetScaleMinMax.x, _TileOffsetScaleMinMax.y) * RandomRemap(random, _TileOffsetScaleMinMaxY.x, _TileOffsetScaleMinMaxY.y);
            rotate = RandomRemap(random, _TileOffsetRotateMinMax.x, _TileOffsetRotateMinMax.y);

            int2 intId = UVToID(saturate((UVRotate(uv - 0.5, float2(0.0, 0.0), rotate) / scale +offset)));
            uint2 uintId = uint2(asuint(intId.x), asint(intId.y));
            result = BlendColor(result, _Buffer[uintId], _TileAndOffsetBlendMode);
            //result += _Buffer[uintId];
        }
    }
    return result;
}