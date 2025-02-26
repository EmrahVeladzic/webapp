import { Texture, Audio, Asset, SkeletalRig, Model, Animation, Track, Mesh, Vertex, Index, UV, Normal } from "../app/renderer/formats";

let tex:Texture = new Texture([1,65535],[6],2,2);

let tex_update:boolean = false;

function flip_tex_state():void{

tex_update=!tex_update;

}

let sfx:Audio = new Audio([48,0,15,15,15,15,15,15,15,15,15,15,15,15,15,15],3000,1,1,8);

export {tex, tex_update,flip_tex_state, sfx}