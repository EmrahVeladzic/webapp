import { Texture, Audio, Asset, SkeletalRig, Model, Animation, Track, Mesh, Vertex, Index, UV, Normal } from "../app/renderer/formats";
import { BehaviorSubject } from 'rxjs';

let current_anim$ = new BehaviorSubject<number | null>(null); 

function update_anim(a: number | null): void {
    current_anim$.next(a);
    
}

let global_time = 0;

function set_global_time(time:number){
    global_time=time;
}


let tex:Texture = new Texture([1,65535],[6],2,2);

let tex_update:boolean = true;

function flip_tex_state():void{

tex_update=!tex_update;

}

let ast_update:boolean = false;

function flip_ast_state():void{

ast_update=!ast_update;

}


let sfx:Audio = new Audio([48,6,2, 51,33,9,171,186,144,2,51,33,9,171,186,144,48,0,2, 51,33,9,171,186,144,2,51,33,9,171,186,144,48,3,2, 51,33,9,171,186,144,2,51,33,9,171,186,144],4000,1,1,8);

let ast:Asset = new Asset(0,12,new Model(0,[new Mesh(0,new Vertex(0,[-4096,4096,-4096,-4096,4096,4096,4096,4096,4096,4096,4096,-4096,-4096,4096,4096,-4096,-4096,4096,-4096,-4096,-4096,-4096,4096,-4096,4096,4096,4096,4096,-4096,4096,4096,-4096,-4096,4096,4096,-4096,4096,4096,4096,4096,-4096,4096,-4096,-4096,4096,-4096,4096,4096,4096,4096,-4096,4096,-4096,-4096,-4096,-4096,-4096,-4096,4096,-4096,-4096,-4096,-4096,-4096,-4096,4096,4096,-4096,4096,4096,-4096,-4096]),
new Index(0,[0,1,2,0,2,3,5,4,6,6,4,7,8,9,10,8,10,11,13,12,14,15,14,12,16,17,18,16,18,19,21,20,22,22,20,23]),null,
new UV(0,[0,0,0,2,2,2,2,0 ,2,0,2,2,0,2,0,0 ,0,0,0,2,2,2,2,0 ,2,0,2,2,0,2,0,0  ,0,0,0,2,2,2,2,0 ,2,0,2,2,0,2,0,0 ]),null)],1,1),null)

export {tex, tex_update,flip_tex_state, sfx,ast , ast_update,flip_ast_state, update_anim, current_anim$, global_time, set_global_time}