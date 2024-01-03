precision mediump float;

attribute vec2 vPosition;
attribute vec3 vColor;

varying vec3 fragColor;

void main(){
    fragColor = vColor;
    gl_position = vec4(vPosition, 0.0, 1.0);
}