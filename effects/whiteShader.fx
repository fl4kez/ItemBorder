sampler2D SpriteTexture : register(s0);
float4 CustomColor; // Color passed from the code (item's rarity color)

struct PixelInput
{
    float2 TexCoord : TEXCOORD0;
};

float4 Main(PixelInput input) : COLOR
{
    float4 color = tex2D(SpriteTexture, input.TexCoord);
    if (color.a != 0)
    {
        return CustomColor; // Set non-transparent pixels to the passed color
    }
    return color;
}

technique Basic
{
    pass P0
    {
        PixelShader = compile ps_2_0 Main();
    }
}
