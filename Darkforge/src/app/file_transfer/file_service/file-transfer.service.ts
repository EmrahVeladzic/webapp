import { Injectable, Input, numberAttribute } from '@angular/core';
import { HttpClient,HttpHeaders, HttpResponse } from '@angular/common/http';
import { base_url,image_actions } from '../../app.routes';
import { Subject } from 'rxjs';


@Injectable({
  providedIn: 'root'
  
})
export class FileTransferService {
    public Menu : string = "NONE";
    private reader? :FileReader;
    public file_text? : string;
    private file_data:  any;
    public mdl_name? :string="";

    private bmpTaskSource = new Subject<void>();
    public bmpTaskCompleted$ = this.bmpTaskSource.asObservable();

    private wavTaskSource = new Subject<void>();
    public wavTaskCompleted$ = this.wavTaskSource.asObservable();


    public wlTaskSource = new Subject<void>();
    public wlTaskCompleted$ = this.wlTaskSource.asObservable();

    
    public glbTaskSource = new Subject<void>();
    public glbTaskCompleted$ = this.glbTaskSource.asObservable();

  constructor(public http:HttpClient) {
    this.reader = new FileReader();
    this.file_text ="";
  
   }


  public accessBinaryFile(offset:number,bytes:number,sign:boolean = false, littleEndian:boolean=true):number|string|null{

 

    let out:number|null = null;

    if(this.file_data!=undefined){

      if(bytes <1){
        bytes=1;
      }


     

      const base64 = this.file_data.split(",")[1];
      const arrayBufferVal = atob(base64);

      if (offset + bytes > arrayBufferVal.length) {return null;}

      else{       

        if(bytes>4){

          return arrayBufferVal.substring(offset, offset + bytes).replace(/\0/g, "");

        }
        
        else{

          let bfr = new ArrayBuffer(bytes+(4%bytes));
          let slice = new Uint8Array(bfr);
  
          for(let i = offset; i < (offset+bytes+(4%bytes)); i++){
            slice[(i-offset)]=arrayBufferVal.charCodeAt(i);
          }
  
          let dataView = new DataView(bfr);

          switch(bytes){

            case 1:
              if(sign){
                out = dataView.getInt8(0) as number;
              }
              else{
                out = dataView.getUint8(0) as number;
              }
            break;
          
            case 2:
              if(sign){
                out = dataView.getInt16(0,littleEndian) as number;
              }
              else{
                out = dataView.getUint16(0,littleEndian) as number;
              }
            break;
            case 3:
              if(sign){
                out = dataView.getInt32(0,littleEndian)&0x00FFFFFF as number;          
                
                if(out>>23===1){
                  out |= 0xFF000000;
                }

              }
              else{
                out = dataView.getUint32(0,littleEndian)&0x00FFFFFF as number;
              }
            break;
            case 4:
              if(sign){
                out = dataView.getInt32(0,littleEndian) as number;
              }
              else{
                out = dataView.getUint32(0,littleEndian) as number;
              }
            break;
            
            default:
              
            break;
      
          }
          
        }   
      
      }
   
    }

    return out;
  }

  private validate_bmp():boolean{

    let v = this.accessBinaryFile(0,2) as number;

    if(v!=null && v===19778){

      let w = this.accessBinaryFile(18,4) as number;
      let h = this.accessBinaryFile(22,4) as number;

      if(w!=null && h!=null && w%8===0 && h%8===0 && w<=256 && h<=256){

      
        let b = this.accessBinaryFile(28,2) as number;

        
        if(b!=null && b===24){
          return true;
        }

        else{
          alert("Image needs to have exactly 24 bits per pixel.");
          return false;
        }


      }
      else{
        alert("Image dimensions need to be divisible by 8 and no more than 256x256. There may be other errors.");
        return false;
      }

    }

    else{
      alert("File appears to not be a BMP-format image.")
      return false;
    }
    
  }

  private validate_wav():boolean{

    let v = this.accessBinaryFile(0,4) as number;

    if(v!=null && v ===1179011410){

      let s = this.accessBinaryFile(34,2) as number;

      if(s!=null && s===16){
        return true;

      }
      else{
        alert("Audio needs to have exactly 16 bits per sample.");
        return false;
      }


    }
    else{
      alert("File appears to not be a WAV-format audio.")
      return false;
    }

  }

  private validate_glb():boolean{

    let v = this.accessBinaryFile(0,4) as number;

    if(v!=null && v===1179937895){

      let j = this.accessBinaryFile(16,4) as number;

      if(j!=null && j===1313821514){

        let j_l = this.accessBinaryFile(12,4) as number;

        let json = this.accessBinaryFile(20,j_l) as string;       

        let b = this.accessBinaryFile((24+j_l),3) as number;

        if(b!=null && b === 5130562){

          let meta = JSON.parse(json);

          if(Array.isArray(meta.meshes)){

            if(Array.isArray(meta.skins) && Array.isArray(meta.skins[0].joints) && meta.skins[0].inverseBindMatrices!=undefined){

              for(let m of meta.meshes){

                if(m.primitives[0].attributes.JOINTS_0==undefined){

                  alert("All meshes require to have at least 1 associated joint.")
                  return false;

                }

              }

              return true;

            }
            else{
              return true;
            }

          }
          else{
            alert("Model is missing meshes.")
            return false;
          }         
         
        }

        else{
          alert("BLOB is missing. Model is not parseable.");
        return false;
        }
        
      }
      else{
        alert("Metadata is missing. Model is not parseable.");
        return false;
      }

    }
    else{
      alert("File appears to not be a GLB-format model.")
      return false;
    }

   
  }

   async process_bmp(file: File): Promise<void> {
   
    this.reader!.readAsDataURL(file);

    this.reader!.onload = ($event: any) => {
      this.file_data = undefined;
      this.file_data = this.reader?.result;

      if (this.file_data !== undefined) {
        this.file_text = this.file_data!.toString();
      }

      if(this.validate_bmp()){     
   
        bmp_preview_url = URL.createObjectURL(file);     
        this.bmpTaskSource.next();
        this.Menu='rpf';
      }

    };
  }

  async process_wav(file : File): Promise<void>{

    this.reader!.readAsDataURL(file);

    this.reader!.onload = ($event: any) => {
      this.file_data = undefined;
      this.file_data = this.reader?.result;

      if (this.file_data !== undefined) {
        this.file_text = this.file_data!.toString();
      }

      if(this.validate_wav()){
        wav_preview_url = URL.createObjectURL(file);     
        this.wavTaskSource.next();
        this.Menu='wl';
      }
      
    };

  }

  
  async process_glb(file : File, name:string): Promise<void>{

    this.reader!.readAsDataURL(file);

    this.reader!.onload = ($event: any) => {
      this.file_data = undefined;
      this.file_data = this.reader?.result;

      if (this.file_data !== undefined) {
        this.file_text = this.file_data!.toString();
      }

      if(this.validate_glb()){
      this.mdl_name=name;     
      this.glbTaskSource.next();
      this.Menu='ast';
      }

    };

  }



}
export let bmp_preview_url :string;
export let wav_preview_url :string;
