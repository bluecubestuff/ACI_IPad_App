// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Gradient/Radial/Alpha" {

	//Set up the shader to receive external inputs from Unity
	Properties {
		_Color ("Color", Color) = (1,1,1,1)				//Receive input from a fixed Color
		_EndColor ("End Color", Color) = (1,1,1,1)		//Receive input from a fixed Color
		_UVXScale ("UV X Scale", float) = 1.0			//Receive input from UV X scale
		_UVYScale ("UV Y Scale", float) = 1.0			//Receive input from UV Y scale
		_StartAlpha ("Starting Alpha", float) = 1.0
		_EndAlpha ("Ending Alpha", float) = 0.0
		_ColorDistance ("Color Distance", float) = 0.0
	}

	//Define a shader
	SubShader {

		//Define what queue/order to render this shader in
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}		//Background | Geometry | AlphaTest | Transparent | Overlay

		//Define a pass
		Pass {
			//Set up blending and other operations
			Cull Back
			ZTest LEqual
			ZWrite On
			AlphaTest Off
			Lighting Off
			ColorMask RGBA
			Blend SrcAlpha OneMinusSrcAlpha
			
			//Start a program in the CG language
			CGPROGRAM
			#pragma target 2.0		
			#pragma fragment frag	
			#pragma vertex vert		
			#include "UnityCG.cginc"

			//Unity variables to be made accessible to Vertex and/or Fragment shader
			uniform fixed4 _Color;
			uniform fixed4 _EndColor;
			uniform float _UVXScale;
			uniform float _UVYScale;
			uniform float _StartAlpha;
			uniform float _EndAlpha;
			uniform float _ColorDistance;
		
			// Data that unity gives the vertex shader
			struct AppData {
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
			};

			// The vertex shader will send these things to the fragment shader
			struct VertexToFragment {
				float4 pos : POSITION;
				half2 uv : TEXCOORD0;
			};

			//Vertex shader
			VertexToFragment vert(AppData v) {
				VertexToFragment vtf;
				vtf.pos = UnityObjectToClipPos(v.vertex);
				vtf.uv = half2((v.texcoord.x) * 1 / _UVXScale * 0.5, (v.texcoord.y ) * 1 / _UVYScale * 0.5);
				return vtf;
			}

			//Fragment shader
			fixed4 frag(VertexToFragment vtf) : COLOR {
				float Alpha = sqrt((vtf.uv.x * vtf.uv.x) + (vtf.uv.y * vtf.uv.y)) / 3;
				return fixed4(lerp(fixed4((_Color.rgb + _EndColor.rgb * _ColorDistance) * 0.5, _StartAlpha),fixed4(_EndColor.rgb * Alpha, _EndAlpha), Alpha));
			}

			ENDCG

		}
	}
}