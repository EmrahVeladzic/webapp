import { Asset } from "../app/renderer/formats";
import { hash_data } from "../utils/hash_maker";
export class ImageDTO{
   
    public imageData : string |null;
    public imageHash : string;
    public cluT_Size : number;
    public alpha :  number[]|null;
    public mode :   boolean;
    public protectedBufferSize : number;
    public creator_ID: number;
    
    constructor(data:string, clut_size : number, alpha_c : number[]|null, c_mode : boolean, protected_bfr_size : number ,hash:string, creator:number) {
        this.imageData=data;
        this.imageHash=hash;
       
       this.cluT_Size = clut_size-1;
       this.alpha = alpha_c;
       this.mode = c_mode;
       this.protectedBufferSize = protected_bfr_size;
       this.creator_ID=creator;

    }
    
    static async create(data: string, clut_size: number, alpha_c: number[] | null, c_mode: boolean, protected_bfr_size: number, creator:number): Promise<ImageDTO> {
        const imageHash = await hash_data(data);
        return new ImageDTO(data, clut_size, alpha_c, c_mode, protected_bfr_size, imageHash,creator);
    }

}

export class SoundDTO{
   
    public soundData : string |null;
    public soundHash : string;
    public thresholdBits : number;
    public channelCount : number;
    public looping :boolean;
    public creator_ID :number;
    
    constructor(data:string, threshold : number, channels : number , loop:boolean, hash:string, creator:number) {
        this.soundData=data;
        this.soundHash=hash;
        this.thresholdBits=threshold;
        this.channelCount=channels;
        this.looping = loop;
        this.creator_ID=creator;
    }
    
    static async create(data:string, threshold : number, channels : number, loop:boolean, creator:number): Promise<SoundDTO> {
        const soundHash = await hash_data(data);
        return new SoundDTO(data, threshold, channels, loop, soundHash, creator);
    }

}

export class ModelDTO{

    public modelData : string |null;
    public modelHash : string;
    public precisionBits :number;
    public targetFPS : number;
    public texWidth : number;
    public texHeight : number;
    public creator_ID: number;

    constructor(data: string, precision:number,fps:number,width:number,height:number, hash:string, creator:number) {
       
        this.modelData=data;
        this.precisionBits=precision;
        this.targetFPS=fps;
        this.texWidth=width-1;
        this.texHeight=height-1;
        this.modelHash=hash;
        this.creator_ID=creator;
        
    }

    static async create(data:string, precision : number, fps:number,width:number,height:number, creator:number): Promise<ModelDTO> {
        const modelHash = await hash_data(data);
        return new ModelDTO(data, precision,fps,width,height, modelHash, creator);
    }

}


export class TextureDTO{


public colours:number;
public width:number;
public height:number;

public rpF_ID:number;

public clut:number[];
public pixels:number[];

    constructor(col:number,w:number,h:number,rpf:number,clut:number[],pxl:number[]){
        this.colours=col;
        this.width=w;
        this.height=h;
        this.rpF_ID=rpf;
        this.clut=clut;
        this.pixels=pxl;
        
    }


}


export class AudioDTO{

    public sampleRate : number;
    public thresholdBits : number;
    public channelCount : number;
    public blockCountPerChannel :number;
    public audioData : number[];
    public wl_ID : number;
    
    constructor(sR:number,threshold:number,channels:number,blocks:number,wl:number,data:number[]){
           
      this.sampleRate=sR;
      this.thresholdBits=threshold;
      this.channelCount=channels;
      this.blockCountPerChannel=blocks;
      this.wl_ID=wl;
      this.audioData=data;
            
    }
    
    
 }



 export class AssetDTO{

    public asset:Asset;
    public asT_ID:number;
   

    constructor(a:Asset,a_id:number) {
        
        this.asset=a;
        this.asT_ID=a_id;
        
    }

 }