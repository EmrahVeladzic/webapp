import { Asset } from "../app/renderer/formats";
import { hash_data } from "../utils/hash_maker";
export class ImageDTO{
   
    public imageData : string |null;
    public imageHash : string;
    public cluT_Size : number;
    public alpha :  number[]|null;
    public mode :   boolean;
    public texturePage_X : number;
    public texturePage_Y : number;
    public textureOffset_X : number;
    public textureOffset_Y  : number;

    
    
    constructor(data:string, clut_size : number, alpha_c : number[]|null, c_mode : boolean ,hash:string,tpx:number,tpy:number,tox:number,toy:number) {
        this.imageData=data;
        this.imageHash=hash;
       
        this.cluT_Size = clut_size-1;
        this.alpha = alpha_c;
        this.mode = c_mode;      

        this.texturePage_X = tpx;
        this.texturePage_Y = tpy;
        this.textureOffset_X = tox;
        this.textureOffset_Y = toy;

    }
    
    static async create(data: string, clut_size: number, alpha_c: number[] | null, c_mode: boolean, tpx:number,tpy:number,tox:number,toy:number): Promise<ImageDTO> {
        const imageHash = await hash_data(data);
        return new ImageDTO(data, clut_size, alpha_c, c_mode, imageHash, tpx,tpy,tox,toy);
    }

}

export class SoundDTO{
   
    public soundData : string |null;
    public soundHash : string;
    public thresholdBits : number;
    public channelCount : number;
    public looping :boolean;

    
    constructor(data:string, threshold : number, channels : number , loop:boolean, hash:string) {
        this.soundData=data;
        this.soundHash=hash;
        this.thresholdBits=threshold;
        this.channelCount=channels;
        this.looping = loop;
    }
    
    static async create(data:string, threshold : number, channels : number, loop:boolean): Promise<SoundDTO> {
        const soundHash = await hash_data(data);
        return new SoundDTO(data, threshold, channels, loop, soundHash);
    }

}

export class ModelDTO{

    public modelData : string |null;
    public modelHash : string;
    public precisionBits :number;
    public targetFPS : number;
    public texWidth : number;
    public texHeight : number;

    constructor(data: string, precision:number,fps:number,width:number,height:number, hash:string) {
       
        this.modelData=data;
        this.precisionBits=precision;
        this.targetFPS=fps;
        this.texWidth=width-1;
        this.texHeight=height-1;
        this.modelHash=hash;
       
        
    }

    static async create(data:string, precision : number, fps:number,width:number,height:number): Promise<ModelDTO> {
        const modelHash = await hash_data(data);
        return new ModelDTO(data, precision,fps,width,height, modelHash);
    }

}


export class TextureDTO{


public colours:number;
public width:number;
public height:number;

public rpF_ID:number;

public clut:number[];
public pixels:number[];

public canDelete:boolean;

public textureOffset_X :number;
public textureOffset_Y :number;
public texturePage_X :number;
public texturePage_Y :number;

    constructor(col:number,w:number,h:number,rpf:number,clut:number[],pxl:number[], del:boolean, tpx:number,tpy:number,tox:number,toy:number) {
        this.colours=col;
        this.width=w;
        this.height=h;
        this.rpF_ID=rpf;
        this.clut=clut;
        this.pixels=pxl;
        this.canDelete=del;
        this.texturePage_X = tpx;
        this.texturePage_Y = tpy;
        this.textureOffset_X = tox;
        this.textureOffset_Y = toy;
    }


}


export class AudioDTO{

    public sampleRate : number;
    public thresholdBits : number;
    public channelCount : number;
    public blockCountPerChannel :number;
    public audioData : number[];
    public wL_ID : number;
    
    public canDelete:boolean;

    constructor(sR:number,threshold:number,channels:number,blocks:number,wl:number,data:number[],del:boolean){
           
        this.sampleRate=sR;
        this.thresholdBits=threshold;
        this.channelCount=channels;
        this.blockCountPerChannel=blocks;
        this.wL_ID=wl;
        this.audioData=data;
        this.canDelete=del;

    }
    
    
 }



 export class AssetDTO{

    public asset:Asset;
    public asT_ID:number;
    public canDelete:boolean;

    constructor(a:Asset,a_id:number,del:boolean) {
        
        this.asset=a;
        this.asT_ID=a_id;
        this.canDelete=del;
    }

 }

 export class ExportDTO{

    public ast!:string|null;
    public rpf!:string|null;
    public wl!:string|null;

 }