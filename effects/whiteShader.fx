sampler2D SpriteTexture : register(s0);
float4 CustomColor; // Color passed from the code (item's rarity color)
float outlineWidth; // Width passed from the code (item's rarity outline width)

struct PixelInput
{
    float2 TexCoord : TEXCOORD0;
};

float4 Main(PixelInput input) : COLOR
{
    float4 color = tex2D(SpriteTexture, input.TexCoord);
    //if (color.a != 0)
    //{
    //    return CustomColor; // Set non-transparent pixels to the passed color
    //}
    //return color;
    if(color.a == 0)
    {
    
        float4 colorRight = tex2D(SpriteTexture, input.TexCoord + float2(outlineWidth, 0));
        float4 colorLeft = tex2D(SpriteTexture, input.TexCoord - float2(outlineWidth, 0));
        float4 colorUp = tex2D(SpriteTexture, input.TexCoord + float2(0, outlineWidth));
        float4 colorDown = tex2D(SpriteTexture, input.TexCoord - float2(0, outlineWidth));
    
        if (colorRight.a != 0 || colorLeft.a != 0 || colorUp.a != 0 || colorDown.a != 0)
        {
            return CustomColor;
        }
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
