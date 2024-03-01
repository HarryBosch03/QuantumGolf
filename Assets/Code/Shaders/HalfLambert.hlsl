#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

struct SurfaceData
{
    float3 albedo;
    float3 positionWS;
    float3 normalWS;
};

half3 LitHalfLambert(SurfaceData input)
{
    float4 shadowCoords = TransformWorldToShadowCoord(input.positionWS);
    float shadowAttenuation = MainLightRealtimeShadow(shadowCoords);

    float ndl = saturate(dot(input.normalWS, _MainLightPosition) * 0.5 + 0.5);
    return input.albedo * lerp(_SubtractiveShadowColor, _MainLightColor, ndl * shadowAttenuation);
}