precision mediump float;

attribute vec2 vPosition;
attribute vec3 vColor;

uniform mat4 worldMat;
uniform mat4 viewMat;
uniform mat4 projMat;

varying vec3 fragColor;

void main(){
    fragColor = vColor;
    gl_Position = vec4(vPosition, 0.0, 1.0);
}