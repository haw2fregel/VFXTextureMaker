float _DisplacemantStrength;
float2 _DisplacemantChannel;
float4 _DisplacementWeight;
bool _DisplacementRepeat;

float4 Displacement(float2 uv, float2 displacement, float4 maskColor = float4(1, 1, 1, 1))
{
    float4 result = float4(0.0, 0.0, 0.0, 0.0);
    displacement = lerp(displacement.xx, displacement.yy, step(float2(1, 1), _DisplacemantChannel));
    
    float2 strength = _DisplacemantStrength.xx * rgbToBright(maskColor.rgb).xx;
    strength = lerp(strength.xx, float2(0, 0), step(float2(2, 2), _DisplacemantChannel));

    float2 uvX = uv + displacement * strength * _DisplacementWeight.xx;
    float2 uvY = uv + displacement * strength * _DisplacementWeight.yy;
    float2 uvZ = uv + displacement * strength * _DisplacementWeight.zz;
    float2 uvW = uv + displacement * strength * _DisplacementWeight.ww;

    result.x = _Buffer[UVToID(_DisplacementRepeat ? frac(uvX) : saturate(uvX))].x;
    result.y = _Buffer[UVToID(_DisplacementRepeat ? frac(uvY) : saturate(uvY))].y;
    result.z = _Buffer[UVToID(_DisplacementRepeat ? frac(uvZ) : saturate(uvZ))].z;
    result.w = _Buffer[UVToID(_DisplacementRepeat ? frac(uvW) : saturate(uvW))].w;
    return result;
}