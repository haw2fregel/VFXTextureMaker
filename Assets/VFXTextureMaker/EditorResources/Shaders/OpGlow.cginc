int _GlowIteration;
float _GlowSize;
float _GlowThreshold;
float _GlowThresholdSmooth;
float _GlowIntensity;
float4 _GlowColor;

inline float glowWeight21(float x, float y)
{
    return 1.0f / ((2.0f * PI * abs(_GlowIteration * _GlowSize)) * exp((x * x + y * y) / (2.0f * abs(_GlowIteration * _GlowSize))));
}

float4 Glow(int2 id)
{
    float weight = 0;
    float2 uv = IdToUV(id);
    float4 color = float4(0, 0, 0, 0);
    float4 result = float4(0, 0, 0, 0);
    float2 idOffset = float2(0, 0);
    for (float x = -_GlowIteration; x <= _GlowIteration; x++)
    {
        for (float y = -_GlowIteration; y <= _GlowIteration; y++)
        {
            idOffset.x = x * 0.001 * _GlowSize;
            idOffset.y = y * 0.001 * _GlowSize;
            idOffset = saturate(idOffset +uv);
            
            color = _Buffer[UVToID(idOffset)];
            weight = glowWeight21(x, y);
            weight *= smoothstep(_GlowThreshold - _GlowThresholdSmooth - EPSILON, _GlowThreshold, rgbToBright(color.xyz));
            result += color * _GlowColor * _GlowIntensity* weight;
        }
    }
    return saturate(result);
}