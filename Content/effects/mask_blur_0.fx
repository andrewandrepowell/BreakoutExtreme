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

static const int BlurRadius = 2;
static const int BlurWidth = BlurRadius * 2 + 1;
static const int BlurArea = BlurWidth * BlurWidth;
float2 SpriteTextureDimensions;
float Spread = 3.0f;
float Mask[BlurArea];

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 spriteOffsetUV = input.TextureCoordinates;
    float4 newSpriteColor = float4(0, 0, 0, 0);
    [unroll(BlurArea)]
    for (int bluri = 0; bluri < BlurArea; bluri++)
    {
        int blurOffsetX = (bluri % BlurWidth) - BlurRadius;
        int blurOffsetY = (bluri / BlurWidth) - BlurRadius;
        
        float maskValue = Mask[bluri];
        float2 blurOffsetPixels = float2(blurOffsetX, blurOffsetY) * Spread;
        float2 blurOffsetUV = blurOffsetPixels / SpriteTextureDimensions;
        float2 colorOffsetUV = clamp(spriteOffsetUV + blurOffsetUV, 0, 1);

        newSpriteColor += Mask[bluri] * tex2D(SpriteTextureSampler, colorOffsetUV);
    }
    return newSpriteColor * input.Color;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};