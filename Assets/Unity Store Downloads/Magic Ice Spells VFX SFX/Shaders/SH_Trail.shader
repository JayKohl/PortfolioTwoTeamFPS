// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Effects/SH_Trail"
{
	Properties
	{
		_Texture1("Texture 1", 2D) = "white" {}
		_Texture3("Texture 3", 2D) = "white" {}
		_Texture2("Texture 2", 2D) = "white" {}
		_Intensity("Intensity", Range( 0 , 5)) = 3
		_Tex3_Intensity("Tex3_Intensity", Range( 0 , 4)) = 1.5
		_Clamp("Clamp", Range( 0 , 5)) = 1
		_ColorDiffuse("Color Diffuse", Color) = (1,1,1,1)
		_Emmision("Emmision", Range( 0 , 5)) = 1
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
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float4 _ColorDiffuse;
		uniform float _Emmision;
		uniform sampler2D _Texture1;
		uniform sampler2D _Texture2;
		uniform sampler2D _Texture3;
		uniform float _Tex3_Intensity;
		uniform float _Intensity;
		uniform float _Clamp;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = ( ( _ColorDiffuse * i.vertexColor ) * _Emmision ).rgb;
			float2 panner47 = ( 1.0 * _Time.y * float2( -0.1,0 ) + i.uv_texcoord);
			float2 uv_TexCoord25 = i.uv_texcoord * float2( 0.25,1 );
			float2 panner26 = ( 1.0 * _Time.y * float2( -0.25,0 ) + uv_TexCoord25);
			float2 appendResult56 = (float2(i.vertexColor.r , 0.0));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float clampResult43 = clamp( ( (ase_screenPosNorm).y * _Clamp ) , 0.0 , 1.0 );
			o.Alpha = ( ( ( ( ( ( ( tex2D( _Texture1, panner47 ) * 0.3 ) * tex2D( _Texture2, panner26 ) ) * 1.485843 ) * ( tex2D( _Texture3, ( appendResult56 + i.uv_texcoord ) ) * _Tex3_Intensity ) ) * _Intensity ) * i.vertexColor.a ) * clampResult43 ).r;
		}

		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
700;118;1651;1004;2613.444;443.5243;1.70062;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;48;-3028.757,-554.4337;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-2841.501,-159.5009;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.25,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;16;-3104.101,183.1991;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-3021.703,410.3989;Float;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;47;-2761.956,-546.3348;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;26;-2561.202,-158.5016;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.25,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;46;-2570.957,-668.8333;Float;True;Property;_Texture1;Texture 1;0;0;Create;True;0;0;False;0;None;22d3bf82fc2abc84585d2d570fc84b1f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;-2491.094,-394.5974;Float;False;Constant;_Float0;Float 0;11;0;Create;True;0;0;False;0;0.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-2810.302,424.3988;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;56;-2778.694,178.7028;Float;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;27;-2374.202,-186.1012;Float;True;Property;_Texture2;Texture 2;2;0;Create;True;0;0;False;0;None;1217f4f8cbf64b34ca46d3473f601f0e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-2245.196,-569.2973;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-2486.302,280.3988;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2055.106,32.19816;Float;False;Constant;_Tex1_Tex2_Smooth;Tex1_Tex2_Smooth;3;0;Create;True;0;0;False;0;1.485843;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-2006.089,-305.4011;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-2214.917,539.336;Float;False;Property;_Tex3_Intensity;Tex3_Intensity;4;0;Create;True;0;0;False;0;1.5;1.42;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-2228.304,272.3986;Float;True;Property;_Texture3;Texture 3;1;0;Create;True;0;0;False;0;None;3ff7f4bc8c586ff42b4fd8707ff630f2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1782.804,-118.1018;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1890.602,275.4991;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;39;-1567.397,611.1005;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1464.302,146.3991;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1546.1,417.8984;Float;False;Property;_Intensity;Intensity;3;0;Create;True;0;0;False;0;3;2.06;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1325.598,699.4005;Float;False;Property;_Clamp;Clamp;5;0;Create;True;0;0;False;0;1;1.43;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;40;-1266.597,583.9005;Float;False;False;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;32;-1072.111,-48.3825;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;50;-1125.427,-344.2768;Float;False;Property;_ColorDiffuse;Color Diffuse;6;0;Create;True;0;0;False;0;1,1,1,1;0.6544118,0.8255115,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-978.5959,627.1008;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1184.5,165.0988;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;43;-705.5961,566.301;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-639.848,-242.9939;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-872.1991,164.9985;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-717.9286,8.578707;Float;False;Property;_Emmision;Emmision;7;0;Create;True;0;0;False;0;1;1.45;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-305.1033,81.86152;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-463.5957,398.7003;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;Effects/SH_Trail;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;True;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;47;0;48;0
WireConnection;26;0;25;0
WireConnection;46;1;47;0
WireConnection;56;0;16;1
WireConnection;56;1;17;0
WireConnection;27;1;26;0
WireConnection;54;0;46;0
WireConnection;54;1;55;0
WireConnection;20;0;56;0
WireConnection;20;1;19;0
WireConnection;49;0;54;0
WireConnection;49;1;27;0
WireConnection;21;1;20;0
WireConnection;28;0;49;0
WireConnection;28;1;29;0
WireConnection;22;0;21;0
WireConnection;22;1;23;0
WireConnection;24;0;28;0
WireConnection;24;1;22;0
WireConnection;40;0;39;0
WireConnection;42;0;40;0
WireConnection;42;1;41;0
WireConnection;30;0;24;0
WireConnection;30;1;31;0
WireConnection;43;0;42;0
WireConnection;57;0;50;0
WireConnection;57;1;32;0
WireConnection;33;0;30;0
WireConnection;33;1;32;4
WireConnection;59;0;57;0
WireConnection;59;1;53;0
WireConnection;44;0;33;0
WireConnection;44;1;43;0
WireConnection;0;2;59;0
WireConnection;0;9;44;0
ASEEND*/
//CHKSM=2627D94FED80209A82FA9CA9C5E143552E5DA174