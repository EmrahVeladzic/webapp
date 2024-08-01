import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';
import { style } from '@angular/animations';
import { ImageJson } from '../../../models/models';

@Injectable({
  providedIn: 'root'
  
})
export class FileTransferService {
    private base_url = 'https://localhost:7032/api/';
    private cnv?:HTMLCanvasElement;
    private ctx? : CanvasRenderingContext2D;
    private preview? : HTMLImageElement;
    private reader? :FileReader;
    private img_text? : string;

  constructor(private http:HttpClient) {
    this.reader = new FileReader();
    this.img_text ="";
  
   }




  sendData(endpoint:string, data:JSON){


    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    const url = this.base_url+endpoint;

    return this.http.post(url,data,{headers:headers});

    
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
        
     
        this.toggle_visibility("rpf");
        this.ctx?.drawImage(this.preview!,0,0,this.cnv!.width,this.cnv!.height);

        
      }

      if(this.img_text!=null){
      let k =  new ImageJson(this.img_text,1,1,true,1);
      }

    };
     
   
    
  }



 
  toggle_visibility(visible:string){


    var to_make_visible = document.getElementById(visible) as HTMLDivElement;
    to_make_visible!.style.visibility="visible";

   
  }


}
