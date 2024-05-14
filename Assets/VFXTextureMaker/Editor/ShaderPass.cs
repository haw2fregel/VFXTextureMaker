using System;

namespace VFXTextureMaker
{
    [Serializable]
    public enum ShaderPass
    {
        Clear,
        Draw_Texture,
        Displacement_Texture,
        Displacement_NormalMap,
        Draw_Noise,
        Draw_Shape,
        Displacement_Noise,
        Filter_Blur,
        Filter_GradationSample,
        Filter_Glow,
        Filter_ColorBalance,
        CustomShader,
        Draw_Draw,
        Displacement_Displacement
    }
}