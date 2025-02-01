/*
https://godotshaders.com/shader/highlight-canvasitem/
"The shader code and all code snippets in this post are under CC0 license and can be used freely without the author's permission. Images and videos, and assets depicted in those, do not fall under this license. For more info, see our License terms."
*/
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


float LineSmoothness; //: hint_range(0, 0.1) = 0.045;
float LineWidth; //: hint_range(0, 0.2) = 0.09;
float Brightness; // = 3.0;
float Rotation; //: hint_range(-90, 90) = 30;
float Distortion; //: hint_range(1, 2) = 1.8;
float Speed; //= 0.7;
float Position; //: hint_range(0, 1) = 0;
float PositionMin; //= 0.25;
float PositionMax; //= 0.5;
float Alpha; //: hint_range(0, 1) = 1;
float GameTimeSeconds;


float2 rotate_uv(float2 uv, float2 center, float angle)
{
    float2x2 rotation = float2x2(
        float2(cos(angle), -sin(angle)),
        float2(sin(angle), cos(angle)));
    
    float2 delta = uv - center;
    delta = mul(rotation, delta);
    return delta + center;
}


struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};


float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 uv = input.TextureCoordinates;
    float2 center_uv = uv - float2(0.5, 0.5);
    float gradient_to_edge = max(abs(center_uv.x), abs(center_uv.y));
    gradient_to_edge = gradient_to_edge * Distortion;
    gradient_to_edge = 1.0 - gradient_to_edge;
    float2 rotated_uv = rotate_uv(uv, float2(0.5, 0.5), Rotation);
   
    float remapped_position;
    {
        float output_range = PositionMax - PositionMin;
        remapped_position = PositionMin + output_range * Position;
    }
   
    float remapped_time = GameTimeSeconds * Speed + remapped_position;
    remapped_time = frac(remapped_time);
    {
        float output_range = 2.0 - (-2.0);
        remapped_time = -2.0 + output_range * remapped_time;
    }
   
    float2 offset_uv = rotated_uv + float2(remapped_time, 0.0);
    float line_u = offset_uv.x;
    line_u = abs(line_u);
    line_u = gradient_to_edge * line_u;
    line_u = sqrt(line_u);
   
    float line_smoothness = clamp(LineSmoothness, 0.001, 1.0);
    float offset_plus = LineWidth + line_smoothness;
    float offset_minus = LineWidth - line_smoothness;
   
    float remapped_line;
    {
        float input_range = offset_minus - offset_plus;
        remapped_line = (line_u - offset_plus) / input_range;
    }
    remapped_line = clamp(remapped_line * Brightness, 0.0, 1.0);
    remapped_line = min(remapped_line, Alpha);
   
    // Sample the original texture
    float4 original_color = tex2D(SpriteTextureSampler, uv);
   
    // Apply the effect as an additive blend!
    float4 return_color = float4(remapped_line, remapped_line, remapped_line, original_color.a);
    return return_color;
}


technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};