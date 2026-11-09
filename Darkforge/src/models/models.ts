import { Asset } from "../app/renderer/formats";
import { hash_data } from "../utils/hash_maker";

export class ImageDTO{

    public imageData : string |null;
    public imageHash : string;
    public cluT_Size : number;
    public alpha :  number[]|null;
    public mode :   boolean;
    public texRow : boolean;
    public tex_X : number;
    public tex_Y : number;
    public cluT_Row : boolean;
    public cluT_X : number;
    public cluT_Y : number;



    constructor(data:string, clut_size : number, alpha_c : number[]|null, c_mode : boolean,trow:number,tx:number,ty:number,crow:number,cx:number,cy:number ,hash:string) {
        this.imageData=data;
        this.imageHash=hash;

        this.cluT_Size = clut_size-1;
        this.alpha = alpha_c;
        this.mode = c_mode;

        this.texRow = trow == 1;
        this.tex_X = tx;
        this.tex_Y = ty;

        this.cluT_Row = crow == 1;
        this.cluT_X = cx;
        this.cluT_Y = cy;

    }

    static async create(data: string, clut_size: number, alpha_c: number[] | null, c_mode: boolean, trow:number,tx:number,ty:number,crow:number,cx:number,cy:number): Promise<ImageDTO> {
        const imageHash = await hash_data(data);
        return new ImageDTO(data, clut_size, alpha_c, c_mode,trow,tx,ty,crow,cx,cy,imageHash);
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
    public texOffsetX : number;
    public texOffsetY :number;
    public rpF_ID : number;

    constructor(data: string, precision:number,fps:number,width:number,height:number,tox:number,toy:number,rpf:number, hash:string) {
       
        this.modelData=data;
        this.precisionBits=precision;
        this.targetFPS=fps;
        this.texWidth=width-1;
        this.texHeight=height-1;
        this.modelHash=hash;
        this.texOffsetX = tox;
        this.texOffsetY = toy;
        this.rpF_ID = rpf;
        
    }

    static async create(data:string, precision : number, fps:number,width:number,height:number,tox:number,toy:number,rpf:number): Promise<ModelDTO> {
        const modelHash = await hash_data(data);
        return new ModelDTO(data, precision,fps,width,height,tox,toy,rpf, modelHash);
    }

}


export class TextureDTO{


    public colours:number;
    public width:number;
    public height:number;

    public rpF_ID:number;

    public clut:number[];
    public pixels:number[];


    public texRow : boolean;
    public tex_X : number;
    public tex_Y : number;
    public cluT_Row : boolean;
    public cluT_X : number;
    public cluT_Y : number;

    constructor(col:number,w:number,h:number,rpf:number,clut:number[],pxl:number[], trow:boolean,tx:number,ty:number,crow:boolean,cx:number,cy:number) {
        this.colours=col;
        this.width=w;
        this.height=h;
        this.rpF_ID=rpf;
        this.clut=clut;
        this.pixels=pxl;
        this.texRow = trow;
        this.tex_X = tx;
        this.tex_Y = ty;
        this.cluT_Row = crow;
        this.cluT_X = cx;
        this.cluT_Y = cy;
    }


}


export class AudioDTO{

    public sampleRate : number;
    public channelCount : number;
    public blockCountPerChannel :number;
    public audioData : number[];
    public wL_ID : number;
    

    constructor(sR:number,channels:number,blocks:number,wl:number,data:number[]){
           
        this.sampleRate=sR;
        this.channelCount=channels;
        this.blockCountPerChannel=blocks;
        this.wL_ID=wl;
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

export class ExportDTO{

    public ast!:string|null;
    public rpf!:string|null;
    public wl!:string|null;

}