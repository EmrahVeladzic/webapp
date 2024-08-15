import { hash_data } from "../utils/hash_maker";
export class ImageJson{
   
    public imageData : string;
    public imageHash : string;
    public cluT_Size : number;
    public alpha :  number[]|null;
    public mode :   boolean;
    public protectedBufferSize : number;

    
    constructor(data:string, clut_size : number, alpha_c : number[]|null, c_mode : boolean, protected_bfr_size : number ,hash:string) {
        this.imageData=data;
        this.imageHash=hash;
       
       this.cluT_Size = clut_size-1;
       this.alpha = alpha_c;
       this.mode = c_mode;
       this.protectedBufferSize = protected_bfr_size;

    }
    
    static async create(data: string, clut_size: number, alpha_c: number[] | null, c_mode: boolean, protected_bfr_size: number): Promise<ImageJson> {
        const imageHash = await hash_data(data);
        return new ImageJson(data, clut_size, alpha_c, c_mode, protected_bfr_size, imageHash);
    }

}

export class TextureJson{


public colours:number;
public width:number;
public height:number;

public rpF_ID:number;
public pgA_ID:number;
public plT_ID:number;

public clut:number[];
public pixels:number[];

    constructor(col:number,w:number,h:number,rpf:number,pga:number,plt:number,clut:number[],pxl:number[]){
        this.colours=col;
        this.width=w;
        this.height=h;
        this.rpF_ID=rpf;
        this.pgA_ID=pga;
        this.plT_ID=plt;
        this.clut=clut;
        this.pixels=pxl;
        
    }


}