// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Swirl_01"
{
	Properties
	{
		_Offset("Offset", Range( 0 , 5)) = 1
		_Texture_01("Texture_01", 2D) = "white" {}
		_Texture_03("Texture_03", 2D) = "white" {}
		_Panning_Intensyty("Panning_Intensyty", Vector) = (0.1,-0.21,0,0)
		_Offset_Correction("Offset_Correction", Vector) = (0.1,-0.21,0,0)
		_Texture_02("Texture_02", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Texture_Main("Texture_Main", 2D) = "white" {}
		_TileableFire("TileableFire", 2D) = "white" {}
		_FireIntensity("FireIntensity", Range( 0 , 2)) = 0
		_Opacity("Opacity", Range( 0 , 2)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Mask;
		uniform sampler2D _TileableFire;
		uniform float _FireIntensity;
		uniform sampler2D _Texture_Main;
		uniform float2 _Offset_Correction;
		uniform float _Offset;
		uniform sampler2D _Texture_03;
		uniform sampler2D _Texture_02;
		uniform sampler2D _Texture_01;
		uniform float2 _Panning_Intensyty;
		uniform float _Opacity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord48 = i.uv_texcoord * float2( 0.2,0.4 );
			float2 panner34 = ( 1.0 * _Time.y * float2( 0.03,0.01 ) + uv_TexCoord48);
			float2 uv_TexCoord6 = i.uv_texcoord * float2( 0.4,0.3 );
			float2 panner16 = ( _Time.x * float2( -0.3,-0.1 ) + uv_TexCoord6);
			o.Emission = ( pow( ( ( tex2D( _Mask, panner34 ) + tex2D( _TileableFire, panner16 ) ) * _FireIntensity ) , 1.8 ) * i.vertexColor ).rgb;
			float2 uv_TexCoord26 = i.uv_texcoord * float2( 1.05,1.2 ) + float2( -0.15,-0.9 );
			float2 uv_TexCoord37 = i.uv_texcoord * float2( 0.2,0.3 );
			float2 panner36 = ( 0.4 * _Time.y * float2( -0.1,-0.2 ) + uv_TexCoord37);
			float2 uv_TexCoord44 = i.uv_texcoord * float2( 4,4 );
			float2 panner45 = ( 0.2 * _Time.y * float2( 1,-0.2 ) + uv_TexCoord44);
			float2 uv_TexCoord21 = i.uv_texcoord * _Panning_Intensyty;
			float2 panner22 = ( 1.0 * _Time.y * float2( 0.1,-0.2 ) + uv_TexCoord21);
			float4 lerpResult39 = lerp( pow( tex2D( _Texture_03, panner36 ) , 3.0 ) , tex2D( _Texture_02, panner45 ) , tex2D( _Texture_01, panner22 ).r);
			o.Alpha = ( ( i.vertexColor.a * tex2D( _Texture_Main, ( ( _Offset_Correction + uv_TexCoord26 ) + ( _Offset * (lerpResult39).rg ) ) ) ) * _Opacity ).r;
		}

		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
423;29;1461;1004;4093.425;-1159.725;1.184916;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;37;-3981.66,750.6338;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.2,0.3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;54;-3696.479,1660.945;Float;False;Property;_Panning_Intensyty;Panning_Intensyty;3;0;Create;True;0;0;False;0;0.1,-0.21;3,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-3459.879,1638.156;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-3675.818,1269.053;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;4,4;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;36;-3607.893,725.1041;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,-0.2;False;1;FLOAT;0.4;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;45;-3363.818,1252.053;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,-0.2;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;22;-3179.671,1620.445;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,-0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;35;-3367.893,677.104;Float;True;Property;_Texture_03;Texture_03;2;0;Create;True;0;0;False;0;None;c7572165342acd54cb7a26aa313146f2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;23;-2964.173,1593.445;Float;True;Property;_Texture_01;Texture_01;1;0;Create;True;0;0;False;0;None;3b583d8b056a89945ba9e0cdbdcee87e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;43;-2971.045,687.9497;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;46;-3123.818,1204.053;Float;True;Property;_Texture_02;Texture_02;5;0;Create;True;0;0;False;0;None;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;48;-1804.914,-247.3257;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.2,0.4;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;39;-2392.538,1320.661;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;6;-1809.406,-55.13571;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.4,0.3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;5;-1738.302,106.4443;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;24;-1568.087,1394.678;Float;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;16;-1435.842,-21.55825;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.3,-0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-1358.285,964.6754;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.05,1.2;False;1;FLOAT2;-0.15,-0.9;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;34;-1448.988,-252.5841;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.03,0.01;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1759.087,1159.778;Float;False;Property;_Offset;Offset;0;0;Create;True;0;0;False;0;1;0.07;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;19;-1430.454,754.717;Float;False;Property;_Offset_Correction;Offset_Correction;4;0;Create;True;0;0;False;0;0.1,-0.21;0.1,-0.29;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;2;-1147.865,-284.9574;Float;True;Property;_Mask;Mask;6;0;Create;True;0;0;False;0;None;803f695b6e8d359429eac08bc8148958;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-1048.686,793.0754;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-1141.818,-16.93191;Float;True;Property;_TileableFire;TileableFire;8;0;Create;True;0;0;False;0;None;803f695b6e8d359429eac08bc8148958;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1263.887,1301.078;Float;True;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-548.9897,1269.981;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;49;-746.3174,-169.1262;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-753.3718,148.5258;Float;False;Property;_FireIntensity;FireIntensity;9;0;Create;True;0;0;False;0;0;1.45;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;-252.69,1010.981;Float;True;Property;_Texture_Main;Texture_Main;7;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-414.2331,-34.79177;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;31;-435.136,598.0084;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;52;135.1977,952.9985;Float;False;Property;_Opacity;Opacity;10;0;Create;True;0;0;False;0;1;0.36;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;50;-65.96072,-15.47586;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;1.8;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;95.39899,719.0808;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;269.3022,66.8922;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;352.1977,548.9985;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;555.2441,163.9529;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Effects/SH_Wing;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;54;0
WireConnection;36;0;37;0
WireConnection;45;0;44;0
WireConnection;22;0;21;0
WireConnection;35;1;36;0
WireConnection;23;1;22;0
WireConnection;43;0;35;0
WireConnection;46;1;45;0
WireConnection;39;0;43;0
WireConnection;39;1;46;0
WireConnection;39;2;23;0
WireConnection;24;0;39;0
WireConnection;16;0;6;0
WireConnection;16;1;5;1
WireConnection;34;0;48;0
WireConnection;2;1;34;0
WireConnection;28;0;19;0
WireConnection;28;1;26;0
WireConnection;1;1;16;0
WireConnection;27;0;25;0
WireConnection;27;1;24;0
WireConnection;29;0;28;0
WireConnection;29;1;27;0
WireConnection;49;0;2;0
WireConnection;49;1;1;0
WireConnection;30;1;29;0
WireConnection;8;0;49;0
WireConnection;8;1;7;0
WireConnection;50;0;8;0
WireConnection;20;0;31;4
WireConnection;20;1;30;0
WireConnection;32;0;50;0
WireConnection;32;1;31;0
WireConnection;51;0;20;0
WireConnection;51;1;52;0
WireConnection;0;2;32;0
WireConnection;0;9;51;0
ASEEND*/
//CHKSM=581415D3908164D4D01782088EFC96889A156D52