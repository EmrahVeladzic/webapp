precision mediump float;

attribute vec3 vPosition;
attribute vec2 vUV;

uniform mat4 worldMat;
uniform mat4 viewMat;
uniform mat4 projMat;

varying vec2 vFrag;



void main(){
    vFrag = vUV;
    gl_Position = projMat * viewMat * worldMat * vec4(vPosition,1.0);
}