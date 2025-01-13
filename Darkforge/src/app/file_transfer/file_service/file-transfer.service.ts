import { Injectable, numberAttribute } from '@angular/core';
import { HttpClient,HttpHeaders, HttpResponse } from '@angular/common/http';
import { base_url,image_actions } from '../../app.routes';
import { Subject } from 'rxjs';


@Injectable({
  providedIn: 'root'
  
})
export class FileTransferService {
    private reader? :FileReader;
    public file_text? : string;
    private file_data:  any;
    public mdl_name? :string;

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


  public accessBinaryFile(offset:number,bytes:number,sign:boolean = false, littleEndian:boolean=true):number|null{

 

    let out:number|null = null;

    if(this.file_data!=undefined){

      if(bytes != 1 && bytes != 2 && bytes != 4){
        bytes=1;
      }


      let bfr = new ArrayBuffer(bytes);
      let slice = new Uint8Array(bfr);

      const base64 = this.file_data.split(",")[1];
      const arrayBufferVal = atob(base64);


      for(let i = offset; i < (offset+bytes); i++){
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

    return out;
  }

   async process_bmp(file: File): Promise<void> {
   
    this.reader!.readAsDataURL(file);

    this.reader!.onload = ($event: any) => {
      this.file_data = undefined;
      this.file_data = this.reader?.result;

      if (this.file_data !== undefined) {
        this.file_text = this.file_data!.toString();
      }

   
      bmp_preview_url = URL.createObjectURL(file);

     
      this.bmpTaskSource.next();
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

   
      wav_preview_url = URL.createObjectURL(file);

     
      this.wavTaskSource.next();
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

      this.mdl_name=name;
     
      this.glbTaskSource.next();
    };

  }



}
export let bmp_preview_url :string;
export let wav_preview_url :string;
