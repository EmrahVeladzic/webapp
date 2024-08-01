import { hash_data } from "../utils/hash_maker";
export class ImageJson{
   
    public ImageData : string;
    public ImageHash : string;
    public CLUT_Size : number;
    public Alpha :  number;
    public Mode :   boolean;
    public ProtectedBufferSize : number;

    
    constructor(data:string, clut_size : number, alpha_c : number, c_mode : boolean, protected_bfr_size : number ) {
        this.ImageData=data;
        this.ImageHash="";
        (async()=>{

            this.ImageHash =await hash_data(data);
           
        })();
       this.CLUT_Size = clut_size-1;
       this.Alpha = alpha_c;
       this.Mode = c_mode;
       this.ProtectedBufferSize = protected_bfr_size;

    }
}