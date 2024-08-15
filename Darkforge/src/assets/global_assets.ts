import { Texture } from "../app/renderer/formats";

let tex = new Texture([1,65535],[6],2,2);

let tex_update = false;

function flip_tex_state():void{

tex_update=!tex_update;

}

export {tex, tex_update,flip_tex_state}