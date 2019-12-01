//@author: antokhio
//@help: template for geometry shaders
//@tags: GS, StreamOut, template
//@credits:

SamplerState s0: IMMUTABLE
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};

float4x4 tW : WORLD;
float4x4 tTex <string uiname="Texture Transform"; bool uvspace=true; >;
struct VS_IN
{
	float4 pos : POSITION;
	float4 normal : NORMAL;
	float4 uv : TEXCOORD0;
};

struct VS_OUT
{
	float3 pos : POSITION;
	float3 normal : NORMAL;
	float2 uv : TEXCOORD0;
};

VS_OUT VS(VS_IN input)
{
	VS_OUT output;
	
	output.pos = mul(input.pos,tW).xyz;
	output.normal = input.normal.xyz;
	output.uv = mul(float4(input.uv.xy,0,1),tTex).xy;
	
    return output;
}


GeometryShader StreamOutGS = ConstructGSWithSO( CompileShader( vs_4_0, VS() ), "POSITION.xyz;NORMAL.xyz;TEXCOORD.xy", NULL,NULL,NULL,-1 );
//if the above does not work, try this line instead
//GeometryShader StreamOutGS = ConstructGSWithSO( CompileShader( vs_4_0, VS() ), "POSITION.xyz;NORMAL.xyz;TEXCOORD.xy" );

technique10 PassMesh
{
    pass PP0
    {
        SetVertexShader( CompileShader( vs_4_0, VS() ) );
        SetGeometryShader( StreamOutGS );
    }  
}