precision mediump float;

attribute vec3 vPosition;
attribute vec2 vUV;

uniform mat4 worldMat;
uniform mat4 viewMat;
uniform mat4 projMat;

uniform mat4 transMat;

varying vec2 vFrag;



void main(){
    vFrag = vUV;

    vec4 temp = vec4(vPosition,1.0);

    temp = transMat*temp;

    gl_Position = projMat * viewMat * worldMat* temp;
}