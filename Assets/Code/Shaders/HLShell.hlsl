#include "./HalfLambertPass.hlsl"
#include "./Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"

Varyings HLShellVertex(Attributes input)
{
    input.positionOS = input.positionOS + input.normalOS * UNITY_GET_INSTANCE_ID(input) * _ShellSpacing;
    return HalfLambertVertex(input);
}

half4 HLShellFragment(Varyings input) : SV_Target
{
    float2 uv = input.positionCS.xy;
    float dither = Dither(uv);
    float percent = 1 - UNITY_GET_INSTANCE_ID(input) / (_Shells - 1.0f);
    clip(percent - dither);
    
    return HalfLambertFragment(input);
}

