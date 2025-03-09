// Credit goes to the following blogger:
// Chilliant - Personal Blog of Ian Taylor
//
// Link to the exact blog entry where the formulas were taken: 
// https://www.chilliant.com/rgb2hsv.html
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


static const float Epsilon = 1e-10;
float3 HSVOffset;
 

float wrap(float x, float m)
{
    return (m + x % m) % m;
}


float3 wrap(float3 x, float m)
{
    return float3(wrap(x.x, m), wrap(x.y, m), wrap(x.z, m));
}


float3 HUEtoRGB(in float H)
{
    float R = abs(H * 6 - 3) - 1;
    float G = 2 - abs(H * 6 - 2);
    float B = 2 - abs(H * 6 - 4);
    return saturate(float3(R, G, B));
}


float3 RGBtoHCV(in float3 RGB)
{
    // Based on work by Sam Hocevar and Emil Persson
    float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
    float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
    float C = Q.x - min(Q.w, Q.y);
    float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
    return float3(H, C, Q.x);
}


float3 RGBtoHSV(in float3 RGB)
{
    float3 HCV = RGBtoHCV(RGB);
    float S = HCV.y / (HCV.z + Epsilon);
    return float3(HCV.x, S, HCV.z);
}


float3 HSVtoRGB(in float3 HSV)
{
    float3 RGB = HUEtoRGB(HSV.x);
    return ((RGB - 1) * HSV.y + 1) * HSV.z;
}


struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 oldRGBAColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    float3 oldRGBColor = oldRGBAColor.rgb;
    float3 oldHSVColor = RGBtoHSV(oldRGBColor);
    float3 newHSVColor = wrap(oldHSVColor + HSVOffset, 1);
    float3 newRGBColor = HSVtoRGB(newHSVColor);
    float4 newRGBAColor = float4(newRGBColor, oldRGBAColor.a);
    return newRGBAColor * input.Color;
}


technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};