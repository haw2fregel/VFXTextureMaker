float _BlurSampleCount;
float _BlurSize;
float _BlurGausianSigma;
float2 _BlurDirection;
float2 _BlurCenter;
bool _BlurRepeat;

inline float gaussWeight21(float x, float y)
{
    return 1.0f / ((2.0f * PI * _BlurGausianSigma * _BlurGausianSigma) * exp((x * x + y * y) / (2.0f * _BlurGausianSigma * _BlurGausianSigma)));
}

float4 GausianBlur(int2 id)
{
    float4 color = 0;
    float sum = 0;
    float weight = 0;
    float2 uv = IdToUV(id);
    for (float x = -_BlurSampleCount; x <= _BlurSampleCount; x++)
    {
        for (float y = -_BlurSampleCount; y <= _BlurSampleCount; y++)
        {
            float2 idOffset = float2(0, 0);
            idOffset.x = x * _BlurSize * 0.01;
            idOffset.y = y * _BlurSize * 0.01;
            idOffset = idOffset +uv;

            weight = gaussWeight21(x, y);
            int2 intId = UVToID(_BlurRepeat ? frac(idOffset) : saturate(idOffset));
            uint2 uintId = uint2(asuint(intId.x), asint(intId.y));
            color += _Buffer[uintId] * weight;
            sum += weight;
        }
    }
    color /= sum.xxxx;
    return color;
}

float4 DirectionBlur(float2 id)
{
    float4 color = 0;
    float sum = 0;
    float weight = 0;
    float2 uv = IdToUV(id);
    for (int x = 0; x < _BlurSampleCount; x++)
    {
        float2 idOffset = float2(0, 0);
        idOffset = _BlurDirection * float(x) * _BlurSize * 0.001;
        idOffset = idOffset + uv;
        weight = gaussWeight21(x, x);
        int2 intId = UVToID(_BlurRepeat ? frac(idOffset) : saturate(idOffset));
        color += _Buffer[intId] * weight;
        sum += weight;
    }
    color /= sum.xxxx;
    return color;
}

float4 ZoomBlur(int2 id)
{
    float4 color = 0;
    float sum = 0;
    float weight = 0;
    float2 uv = IdToUV(id);

    float2 vec = _BlurCenter - uv;
    float length = vec.x * vec.x + vec.y * vec.y;
    float scale = 0.0;
    float2 idOffset = int2(0, 0);
    int2 uintId = int2(0, 0);
    for (int i = 0; i < _BlurSampleCount; i++)
    {
        scale = i * _BlurSize * 0.001;
        weight = gaussWeight21(i, i);
        idOffset = uv + (vec * scale);
        uintId = UVToID(_BlurRepeat ? frac(idOffset) : saturate(idOffset));
        color += _Buffer[uintId] * weight;
        sum += weight;
    }

    color /= sum.xxxx;
    return color;
}

float4 RotateBlur(int2 id)
{
    float4 color = 0;
    float sum = 0;
    float2 uv = IdToUV(id);
    float2 idOffset;
    float weight;
    for (int x = -_BlurSampleCount; x <= _BlurSampleCount; x++)
    {
        idOffset = UVRotate(uv, _BlurCenter, x * _BlurSize);

        weight = gaussWeight21((float)x, (float)x);
        color += _Buffer[UVToID(_BlurRepeat ? frac(idOffset) : saturate(idOffset))] * weight;
        sum += weight;
    }
    color /= sum.xxxx;
    return color;
}