sampler2D SpriteTexture : register(s0);
float4 CustomColor;

struct PixelInput
{
    float2 TexCoord : TEXCOORD0;
};

float4 Main(PixelInput input) : COLOR
{
    float4 color = tex2D(SpriteTexture, input.TexCoord);

    if (color.a > 0)
    {
        return CustomColor;
    }
    
    return float4(0, 0, 0, 0);
}

technique Basic
{
    pass P0
    {
        PixelShader = compile ps_2_0 Main();
    }
}