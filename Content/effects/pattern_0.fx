#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

Texture2D PatternTexture;
sampler2D PatternTextureSampler = sampler_state
{
    Texture = <PatternTexture>;
    AddressU = Clamp;
    AddressV = Clamp;
};

static const float4 Transparent = float4(0, 0, 0, 0);
float2 SpriteTextureDimensions;
float2 SpriteRegionDimensions;
float2 PatternTextureDimensions;
float2 PatternRegionPosition;
float2 PatternRegionDimensions;


float wrap(float x, float m)
{
    return (m + x % m) % m;
}


float2 wrap(float2 x, float2 m)
{
    return float2(wrap(x.x, m.x), wrap(x.y, m.y));
}

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 spriteTextureColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    if (spriteTextureColor.a == 0)
        return Transparent;
    float2 spriteTexturePosition = input.TextureCoordinates * SpriteTextureDimensions;
    float2 spriteRegionPosition = wrap(spriteTexturePosition, SpriteRegionDimensions);
    float2 patternRegionPosition = wrap(spriteRegionPosition, PatternRegionDimensions);
    float2 patternTexturePosition = patternRegionPosition + PatternRegionPosition;
    float2 patternTextureCoordinates = patternTexturePosition / PatternTextureDimensions;
    float4 patternTextureColor = tex2D(PatternTextureSampler, patternTextureCoordinates);
    return patternTextureColor * input.Color;
}


technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};