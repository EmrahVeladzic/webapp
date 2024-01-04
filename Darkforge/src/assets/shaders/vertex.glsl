precision mediump float;

attribute vec3 vPosition;
attribute vec3 vColor;

uniform mat4 worldMat;
uniform mat4 viewMat;
uniform mat4 projMat;

varying vec3 fragColor;

void main(){
    fragColor = vColor;
    gl_Position = projMat * viewMat * worldMat * vec4(vPosition,1.0);
}