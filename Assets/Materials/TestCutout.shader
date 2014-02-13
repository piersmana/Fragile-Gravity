// Upgrade NOTE: replaced 'PositionFog()' with multiply of UNITY_MATRIX_MVP by position
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'

Shader "PiersTest/TestCutout" {
Properties {   
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
	[PerRendererData] _Color ("Tint", Color) = (1,1,1,1)
    _RevealedTex ("Revealed Texture", 2D) = "white" {}
    _TextureScale ("TextureScale", Range (1, 100)) = 1  //slider for scaling the texture
 
}
 
SubShader {		
	Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

	Pass {
 
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
 
struct appdata_t
{
	float4 vertex   : POSITION;
	float4 color    : COLOR;
	float2 texcoord : TEXCOORD0;
};
			
struct v2f {
    float4 vertex : SV_POSITION;        
    fixed4  color : COLOR;
    half2  texcoord : TEXCOORD0;
    half2  old_texcoord : TEXCOORD1;
};
 
sampler2D _MainTex;
float _TextureScale;
fixed4 _Color;

v2f vert (appdata_t v)
{
    v2f o;
    o.vertex = mul (UNITY_MATRIX_MVP, v.vertex);
    float3 worldPos = mul ((float4x4)_Object2World, v.vertex );
    o.texcoord = worldPos.xy / _TextureScale;  //setting and scaling UVs  
    o.color = v.color * _Color;
    o.old_texcoord = v.texcoord;
    return o;
}

sampler2D _RevealedTex;
 
half4 frag (v2f i) : COLOR
{  
   half4 texcol = tex2D( _RevealedTex, i.texcoord ) * i.color; 
   texcol.a =  tex2D( _MainTex, i.old_texcoord).a;
   return texcol;
 
}
ENDCG
 
    }
}
Fallback "VertexLit"
}