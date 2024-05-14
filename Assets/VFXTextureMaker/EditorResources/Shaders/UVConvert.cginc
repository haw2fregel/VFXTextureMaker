inline float2 IdToUV(int2 id)
{
    float2 uv = float2(id);
    uv.x = uv.x == 0 ? 0 : uv.x / _Resolution.x;
    uv.y = uv.y == 0 ? 0 : uv.y / _Resolution.y;
    return uv;
}

inline float2 IdToUV(int2 id, int2 resolution)
{
    float2 uv = float2(id);
    uv.x = uv.x == 0 ? 0 : uv.x / resolution.x;
    uv.y = uv.y == 0 ? 0 : uv.y / resolution.y;
    return uv;
}

inline float2 IdToUV(uint2 id, uint2 resolution, uint2 textureSize)
{
    float2 scale = (float2)resolution / (float2)textureSize;
    return (float2)id / scale / (float2)resolution;
}

inline int2 UVToID(float2 uv)
{
    float2 id = uv * _Resolution;
    id.x = id.x >= _Resolution.x ? _Resolution.x - 1 : id.x;
    id.y = id.y >= _Resolution.y ? _Resolution.y - 1 : id.y;
    id.x = id.x <= 0 ? 0 : id.x;
    id.y = id.y <= 0 ? 0 : id.y;

    return int2(id);
}

inline int2 UVToID(float2 uv, int2 resolution)
{
    float2 id = uv * resolution;
    id.x = id.x >= resolution.x ? resolution.x - 1 : id.x;
    id.y = id.y >= resolution.y ? resolution.y - 1 : id.y;
    id.x = id.x <= 0 ? 0 : id.x;
    id.y = id.y <= 0 ? 0 : id.y;

    return int2(id);
}