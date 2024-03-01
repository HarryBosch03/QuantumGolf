struct Attributes
{
    float4 positionOS : POSITION;
    float4 normalOS : NORMAL;
    float2 uv : TEXCOORD0;
};

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float3 positionWS : VAR_POSITION_WS;
    float3 normalWS : VAR_NORMAL;
    float2 uv : TEXCOORD0;
};
