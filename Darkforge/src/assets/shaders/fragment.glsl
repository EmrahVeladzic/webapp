precision mediump float;

varying vec2 vFrag;

uniform sampler2D v_grid;


void main(){


vec4 simple_colour = vec4(texture2D(v_grid,vFrag));


if(simple_colour.a==0.0){
    discard;
}



gl_FragColor = simple_colour;
}