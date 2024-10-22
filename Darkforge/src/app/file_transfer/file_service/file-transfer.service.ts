import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders, HttpResponse } from '@angular/common/http';
import { base_url,image_actions } from '../../app.routes';



@Injectable({
  providedIn: 'root'
  
})
export class FileTransferService {
    private cnv?:HTMLCanvasElement;
    private ctx? : CanvasRenderingContext2D;
    private preview? : HTMLImageElement;
    private reader? :FileReader;
    public img_text? : string;

  constructor(public http:HttpClient) {
    this.reader = new FileReader();
    this.img_text ="";
  
   }






  async process_bmp(file:File){

    this.cnv = document.getElementById("bmp_preview") as HTMLCanvasElement;
    this.ctx = this.cnv.getContext("2d") as CanvasRenderingContext2D;
    this.ctx!.imageSmoothingEnabled=false;


    this.reader!.readAsDataURL(file);
  

    this.reader!.onload = ($event:any)=>{

     
      let img_data = this.reader?.result;
    
      if(img_data!=undefined){
       this.img_text=img_data.toString();
      }
             
           

      this.preview! = new Image();

      this.preview!.src=URL.createObjectURL(file);

      this.preview!.onload = () =>{
        
     
        
        this.ctx?.drawImage(this.preview!,0,0,this.cnv!.width,this.cnv!.height);

        
      }

    };
     
   
    
  }

  async process_wav(file : File){

    


  }

 







}
