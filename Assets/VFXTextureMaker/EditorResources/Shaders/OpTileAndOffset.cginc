float2 _TileOffsetCount;
float2 _TileOffsetPosMinMaxX;
float2 _TileOffsetPosMinMaxY;
float2 _TileOffsetScaleMinMax;
float2 _TileOffsetScaleMinMaxX;
float2 _TileOffsetScaleMinMaxY;
float2 _TileOffsetRotateMinMax;
float2 _TileOffsetOpacityMinMax;
int _TileAndOffsetBlendMode;
int _TileAndOffsetRandomSeed;
float4 _TileAndOffsetBackColor;

float RandomRemap(float random, float min, float max)
{
    return random * (max - min) + min;
}

float4 TileAndOffset(int2 id)
{
    float4 result = _TileAndOffsetBackColor;
    float2 uv = IdToUV(id);
    float2 tileAnchor = 0;
    float2 halfcount = (_TileOffsetCount - 1) / 2;
    float2 offset = float2(0, 0);
    float2 scale = float2(0, 0);
    float rotate = 0;
    float opacity = 0;
    float2 randomseed = 0;
    int2 count = _TileOffsetCount;

    for (float x = 0; x < count.x; x++)
    {
        for (float y = 0; y < count.y; y++)
        {
            uv = IdToUV(id);
            randomseed = float2((x + count.x) * _TileAndOffsetRandomSeed, (y + count.y) * _TileAndOffsetRandomSeed);
            offset.x = RandomRemap(hash21(randomseed + 1), _TileOffsetPosMinMaxX.x, _TileOffsetPosMinMaxX.y);
            offset.y = RandomRemap(hash21(randomseed + 2), _TileOffsetPosMinMaxY.x, _TileOffsetPosMinMaxY.y);
            scale.x = RandomRemap(hash21(randomseed + 3), _TileOffsetScaleMinMax.x, _TileOffsetScaleMinMax.y) * RandomRemap(hash21(randomseed + 4), _TileOffsetScaleMinMaxX.x, _TileOffsetScaleMinMaxX.y);
            scale.y = RandomRemap(hash21(randomseed + 3), _TileOffsetScaleMinMax.x, _TileOffsetScaleMinMax.y) * RandomRemap(hash21(randomseed + 5), _TileOffsetScaleMinMaxY.x, _TileOffsetScaleMinMaxY.y);
            rotate = RandomRemap(hash21(randomseed + 6), _TileOffsetRotateMinMax.x, _TileOffsetRotateMinMax.y);
            opacity = RandomRemap(hash21(randomseed + 7), _TileOffsetOpacityMinMax.x, _TileOffsetOpacityMinMax.y);
            
            offset = float2(x, y) - 2 + (1 - count % 2.0) / 2.0 + offset;
            uv = ((uv - 0.5) / scale ) * count + 0.5 - (offset) / scale;
            uv = UVRotate(modulo(uv, count / scale), float2(0.5, 0.5) ,rotate);
            int2 intId = UVToID(saturate(uv));
            uint2 uintId = uint2(asuint(intId.x), asint(intId.y));
            result = Blend(result, _Buffer[uintId], _TileAndOffsetBlendMode, opacity);
        }
    }
    return result;
}