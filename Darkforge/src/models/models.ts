import { Asset } from "../app/renderer/formats";
import { hash_data } from "../utils/hash_maker";
export class ImageDTO{

    public imageData : string |null;
    public imageHash : string;
    public cluT_Size : number;
    public alpha :  number[]|null;
    public mode :   boolean;
    public texRow : boolean;
    public texX : number;
    public texY : number;
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
        this.texX = tx;
        this.texY = ty;

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
    public texPageX  :number;
    public texPageY :number;
    public texOffsetX : number;
    public texOffsetY :number;
    public texClutByteReduction : number;

    constructor(data: string, precision:number,fps:number,width:number,height:number,tpx:number,tpy:number,tox:number,toy:number,clt:number, hash:string) {
       
        this.modelData=data;
        this.precisionBits=precision;
        this.targetFPS=fps;
        this.texWidth=width-1;
        this.texHeight=height-1;
        this.modelHash=hash;
        this.texPageX = tpx;
        this.texPageY = tpy;
        this.texOffsetX = tox;
        this.texOffsetY = toy;
        this.texClutByteReduction = clt;
        
    }

    static async create(data:string, precision : number, fps:number,width:number,height:number,tpx:number,tpy:number,tox:number,toy:number,clt:number): Promise<ModelDTO> {
        const modelHash = await hash_data(data);
        return new ModelDTO(data, precision,fps,width,height,tpx,tpy,tox,toy,clt, modelHash);
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

    public texRow : boolean;
    public texX : number;
    public texY : number;
    public cluT_Row : boolean;
    public cluT_X : number;
    public cluT_Y : number;

    constructor(col:number,w:number,h:number,rpf:number,clut:number[],pxl:number[], del:boolean, trow:boolean,tx:number,ty:number,crow:boolean,cx:number,cy:number) {
        this.colours=col;
        this.width=w;
        this.height=h;
        this.rpF_ID=rpf;
        this.clut=clut;
        this.pixels=pxl;
        this.canDelete=del;
        this.texRow = trow;
        this.texX = tx;
        this.texY = ty;
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
    
    public canDelete:boolean;

    constructor(sR:number,channels:number,blocks:number,wl:number,data:number[],del:boolean){
           
        this.sampleRate=sR;
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