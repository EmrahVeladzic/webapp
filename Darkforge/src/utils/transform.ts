import { vec3,quat,mat4 } from "gl-matrix";

export function get_T(t:number[]):vec3{
    return vec3.fromValues(t[0],t[1],t[2]);
}
export function get_R(t:number[]):quat{

    let out = quat.fromValues(t[3],t[4],t[5],t[6]);
    quat.normalize(out,out);
    return out;
}
export function get_S(t:number[]):vec3{
    return vec3.fromValues(t[7],t[8],t[9]);
}
export function get_Mat(t:number[]):mat4{

    let out=mat4.create();

   mat4.fromRotationTranslationScale(out,get_R(t),get_T(t),get_S(t));
        
    return out;
}
