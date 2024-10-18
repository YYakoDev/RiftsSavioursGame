Shader "Unlit/Hole Cutter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
   Subshader{
     Tags {Queue = Background}
     Pass{ColorMask 0}
    }
}
