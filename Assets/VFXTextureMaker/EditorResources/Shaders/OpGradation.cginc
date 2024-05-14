float4 _GradientArray[1024];
int _GradationReference;

float4 sampleGradation(float4 baceColor, float2 uv)
{
    float reference = lerp(baceColor.x, baceColor.y, step(1, _GradationReference));
    reference = lerp(reference, baceColor.z, step(2, _GradationReference));
    reference = lerp(reference, baceColor.w, step(3, _GradationReference));
    reference = lerp(reference, rgbToBright(baceColor.xyz), step(4, _GradationReference));
    reference = lerp(reference, uv.x, step(5, _GradationReference));
    reference = lerp(reference, uv.y, step(6, _GradationReference));
    int samplePoint = (int) (saturate(reference) * 1023);

    float4 drawColor1 = _GradientArray[samplePoint];
    float4 drawColor2 = _GradientArray[min(1023, samplePoint + 1)];
    float4 drawColor = lerp(drawColor1, drawColor2, frac(reference * 1023));
    
    return drawColor;
}
