int _PosterizeSteps;

float4 Posterize(float4 col)
{
    return round(col / (1.0 / (float)_PosterizeSteps)) * (1.0 / (float)_PosterizeSteps);
}

