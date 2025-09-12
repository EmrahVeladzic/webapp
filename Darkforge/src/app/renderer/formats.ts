import { get_bit_mask, get_bits_per_index,get_pcm_value } from "../../utils/bitfield_helper";
import { flip_tex_state } from "../../assets/global_assets";
import { get_float, normalize_uv, time_float } from "../../utils/fixed_point";
import { mat4 } from "gl-matrix";
import { get_Mat } from "../../utils/transform";

export class Texture{

    public id!:number;
    public CLUT!:number[];
    public Indices!:number[];
    public Width! : number;
    public Height!: number;
    public Data! : Uint16Array;

    constructor(i:number,clut:number[],pixels:number[],width:number,height:number){

        this.reset(i,clut,pixels,width,height);
        
    }

    public reset(i:number,clut:number[],pixels:number[],width:number,height:number):void{

        this.id=i;
        this.CLUT=clut;

        for(let i =0; i<this.CLUT.length;i++){
           this.CLUT[i]=(this.CLUT[i]<<1|((this.CLUT[i]>>15)&1))&0xFFFF;
        }      

        this.Indices=pixels;
        this.Width=width+1;
        this.Height=height+1;

        let bpi = get_bits_per_index(this.CLUT.length);
        let data_length_mult = (8/bpi);

        let mask = get_bit_mask(bpi);

        this.Data = new Uint16Array(this.Width * this.Height);
      
        for(let i =0; i<this.Indices.length;i++){

            let base_byte = this.Indices[i];

            for(let j = data_length_mult-1; j>=0; j--){
                
                this.Data[((i*data_length_mult)+(data_length_mult-1-j))%(this.Width*this.Height)]=this.CLUT[((base_byte>>(j*bpi))&mask)];
                              
            }


        }
        
        this.CLUT=[];
        this.Indices=[];

    }

}

export class Audio{

    public id!:number;
    public BlockData!: number[];
    public Data!:Float32Array;
    public SampleRate! :number;
    public ChannelCount! : number;
    public BlocksPerChannel!:number;
    public Looping! :boolean;
    public ThresholdBits!:number;
    public Samples! : number[];

    constructor(i:number,data:number[],sample_rate:number,channels:number,blocks:number,threshold:number){

        this.reset(i,data,sample_rate,channels,blocks,threshold);

    }


    public reset(i:number,data:number[],sample_rate:number,channels:number,blocks:number,threshold:number):void{
        this.id=i;
        this.BlockData=data;
        this.SampleRate=sample_rate;
        this.ChannelCount=channels;
        this.BlocksPerChannel=blocks;
        
        this.ThresholdBits=threshold;
        this.Looping= this.BlockData[1]==6;
        this.Samples = [];


        for(let i = 0; i < (this.BlocksPerChannel*this.ChannelCount*16);i+=16){

            let total_shift = ((this.BlockData[i]>>4)&0xF) + this.ThresholdBits;
          

            for(let j = 0; j<14;j++){

                let samp = this.BlockData[(i+2+j)];

                this.Samples.push(get_pcm_value(((samp>>4)&0xF),total_shift));
               

                this.Samples.push(get_pcm_value((samp&0xF),total_shift));
               
              
            }


        }        


       

        for(let i = 0; i < this.Samples.length;i++){
             
            if(i>=(this.ChannelCount*3)){

                this.Samples[(i-(2*this.ChannelCount))]+=((this.Samples[i]-this.Samples[(i-(3*this.ChannelCount))])*0.25);
                this.Samples[(i-this.ChannelCount)]+=((this.Samples[i]-this.Samples[(i-(3*this.ChannelCount))])*0.75);

            }

            
        }

        this.BlockData=[];

        this.Data = new Float32Array(this.Samples);

        this.Samples=[];

    }

}

export class Vertex{
    public id:number;
    public vertices:number[];

    
    constructor(i:number, v:number[]) {
        this.id=i;
        this.vertices=v;        
    }
}

export class Index{
    public id:number;
    public indices:number[];

    
    constructor(i:number, ind:number[]) {
        this.id=i;
        this.indices=ind;        
    }
}

export class UV{
    public id:number;
    public textureCoordinates:number[];

    
    constructor(i:number, t:number[]) {
        this.id=i;
        this.textureCoordinates=t;        
    }
}


export class Normal{
    public id:number;
    public normals:number[];

    
    constructor(i:number, n:number[]) {
        this.id=i;
        this.normals=n;        
    }
}




export class Mesh{
    public id:number;
    public bN_ID:number | null;
    public vt?:Vertex |null;
    public ind?:Index | null;
    public nrm?:Normal | null;
    public uv?:UV | null;

    
    constructor(id:number,v:Vertex | null,i:Index | null,n:Normal | null,u:UV | null,b:number | null) {
        
        this.id=id;
        this.bN_ID=b;
        this.vt=v;
        this.ind=i;
        this.nrm=n;
        this.uv=u;
    }

}

export class Model{

    public id:number;
    public meshes:Mesh[];
    public width:number;
    public height:number;
    
    constructor(i:number,m:Mesh[],w:number,h:number) {
        
        this.id=i;
        this.meshes=m;
        this.width=w;
        this.height=h;
    }


}

export class Track{

    public id:number;
    public bN_ID:number;
    public translations:number[];
    public rotations:number[];
    public scales:number[];
    public t_Frames:number[];
    public r_Frames:number[];
    public s_Frames:number[];

    public t_Index?:number;
    public r_Index?:number;
    public s_Index?:number;
    
    constructor(i:number, b:number, t:number[],r:number[],s:number[],tt:number[],rt:number[],st:number[]) {
        
        this.id=i;
        this.bN_ID=b;
        this.translations=t;
        this.rotations=r;
        this.scales=s;
        this.t_Frames=tt;
        this.r_Frames=rt;
        this.s_Frames=st;
                
        
    }

}

export class Animation{

    public id:number;
    public tracks:Track[];

    public duration?:number;

    constructor(i:number, t:Track[]) {
        
        this.id=i;
        this.tracks=t;

        this.duration=0;
                
    }

}

export class Bone{
    public id :number;
    public parent_ID?:number;
    public initialTransform:number[];

    public currentTRS?:mat4;

    constructor(i:number, t:number[],p?:number) {
        this.id=i;
        this.parent_ID=p;
        this.initialTransform=t;
        this.currentTRS=get_Mat(t);
    }

}



export class SkeletalRig{

    public id:number;
    public bones:Bone[];
    public animations:Animation[];
    public fps:number | null;
    public root:number;
    
    constructor(i:number,b:Bone[],a:Animation[],f:number|null,r:number) {
        
        this.id=i;
        this.bones=b;
        this.animations=a;
        this.fps=f;
        this.root=r;

    }

}

export class Asset{

    public id!:number;
    public mdl?:Model | null;
    public fkr?:SkeletalRig |null;

    public precisionBits!:number;

    public reset(i:number,p:number,m:Model | null,f:SkeletalRig | null) : void{

        this.id=i;
        this.precisionBits=p;

        if(m!=null){

            m.width++;
            m.height++

            for(let msh of m.meshes){

                if(msh.vt!=null){
                    msh.vt.vertices = msh.vt?.vertices.map(v=>get_float(v,this.precisionBits));     
                }
                if(msh.nrm!=null){
                    msh.nrm.normals = msh.nrm?.normals.map(n=>get_float(n,this.precisionBits));  
                }                                             
                if(msh.uv!=null){                  
                    normalize_uv(msh.uv.textureCoordinates,m.width,m.height);
                }        

            }
            
        }

        if(f!=null){

            for(let b of f.bones){
                
                b.initialTransform= b.initialTransform.map(i=>get_float(i,this.precisionBits));
                b.currentTRS=get_Mat(b.initialTransform);

            }

            
            for(let a of f.animations){

                for(let t of a.tracks){

                    t.translations= t.translations.map(tr=>get_float(tr,this.precisionBits));
                    t.rotations= t.rotations.map(ro=>get_float(ro,this.precisionBits));
                    t.scales= t.scales.map(sc=>get_float(sc,this.precisionBits));                   

                    time_float(t.t_Frames,f.fps!);
                    time_float(t.r_Frames,f.fps!);
                    time_float(t.s_Frames,f.fps!);
                }

                a.duration=0;
                for(let t of a.tracks){

                    for(let i =0; i <t.t_Frames.length; i++){
                        if(t.t_Frames[i]>a.duration){
                            a.duration=t.t_Frames[i];
                        }
                    }

                    for(let i =0; i <t.r_Frames.length; i++){
                        if(t.r_Frames[i]>a.duration){
                            a.duration=t.r_Frames[i];
                        }
                    }


                    for(let i =0; i <t.s_Frames.length; i++){
                        if(t.s_Frames[i]>a.duration){
                            a.duration=t.s_Frames[i];
                        }
                    }

                    t.t_Index=0;
                    t.r_Index=0;
                    t.s_Index=0;

                }


            }
            

        }

        
        this.mdl=m;
        this.fkr=f;
    }

    constructor(i:number,p:number,m:Model |null,f:SkeletalRig | null) {
      
        this.reset(i,p,m,f)
        
    }


}