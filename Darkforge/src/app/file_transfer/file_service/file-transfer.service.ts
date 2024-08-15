import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders, HttpResponse } from '@angular/common/http';
import { style } from '@angular/animations';
import { ImageJson,TextureJson } from '../../../models/models';
import { base_url,image_actions } from '../../app.routes';
import { tex } from '../../../assets/global_assets';

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

    };
     
   
    
  }



 
  toggle_visibility(visible:string){


    var to_make_visible = document.getElementById(visible) as HTMLDivElement;
    to_make_visible!.style.visibility="visible";

   
  }


  async create_image_json() : Promise<ImageJson>{   

    

    let CLUT_ctrl = document.getElementById("CLUT") as HTMLInputElement;
    
    let r_out = document.getElementById("r_out") as HTMLOutputElement;
    let g_out = document.getElementById("g_out") as HTMLOutputElement;
    let b_out = document.getElementById("b_out") as HTMLOutputElement; 
   
    let mode_slc = document.getElementById("Mode") as HTMLSelectElement;

    let BFR_ctrl = document.getElementById("BFR") as HTMLInputElement;

    let CHK = document.getElementById("use_alpha") as HTMLInputElement;

    const $instance = await ImageJson.create(this.img_text!,parseInt(CLUT_ctrl.value),(CHK.checked==true)?[parseInt(r_out.value),parseInt(g_out.value),parseInt(b_out.value)]:null,(mode_slc.selectedIndex==1),parseInt(BFR_ctrl.value));

    return $instance;

  }



  post_image(){

   (this.create_image_json()).then($result=>{



    let post_url = `${base_url}/${image_actions}`;


    this.http.post(post_url,$result).subscribe($response=>{

      let TextureResponse = $response as TextureJson;
      
      tex.reset(TextureResponse.clut,TextureResponse.pixels,(TextureResponse.width+1),(TextureResponse.height+1));
    

    });
  



    });



  }



}
