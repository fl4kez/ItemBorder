sampler2D SpriteTexture : register(s0);
float4 CustomColor;
float texelWidth; // 1.0 / texture.Width
float texelHeight; // 1.0 / texture.Height

struct PixelInput
{
    float2 TexCoord : TEXCOORD0;
};

// Hilfsfunktion für TexCoord-Grenzencheck
bool InBounds(float2 coord)
{
    return coord.x >= 0.0 && coord.x <= 1.0 && coord.y >= 0.0 && coord.y <= 1.0;
}

float4 Main(PixelInput input) : COLOR
{
    float4 color = tex2D(SpriteTexture, input.TexCoord);

    if (color.a == 0)
    {
        bool hasNeighbor = false;

        float2 offsets[4] =
        {
            float2(texelWidth, 0),
            float2(-texelWidth, 0),
            float2(0, texelHeight),
            float2(0, -texelHeight)
        };

        // Schleife über Nachbar-Pixel
        for (int i = 0; i < 4; i++)
        {
            float2 neighborCoord = input.TexCoord + offsets[i];
            if (InBounds(neighborCoord))
            {
                float4 neighborColor = tex2D(SpriteTexture, neighborCoord);
                if (neighborColor.a > 0)
                {
                    hasNeighbor = true;
                    break;
                }
            }
        }

        if (hasNeighbor)
        {
            return CustomColor; // Volldeckende Outline
        }
    }

    // Normal zurückgeben (transparent oder normaler Pixel)
    return float4(0, 0, 0, 0);
}


technique Basic
{
    pass P0
    {
        PixelShader = compile ps_2_0 Main();
    }
}
