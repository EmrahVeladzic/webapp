import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';
import { style } from '@angular/animations';

@Injectable({
  providedIn: 'root'
  
})
export class FileTransferService {
    private base_url = 'https://localhost:7032/api/';
    private cnv?:HTMLCanvasElement;
    private ctx? : CanvasRenderingContext2D;
    private preview? : HTMLImageElement;
    private reader? :FileReader;


  constructor(private http:HttpClient) {
    this.reader = new FileReader();

  
   }




  sendData(endpoint:string, data:JSON){


    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    const url = this.base_url+endpoint;

    return this.http.post(url,data,{headers:headers});

    
  }


  process_bmp(file:File){

    this.cnv = document.getElementById("bmp_preview") as HTMLCanvasElement;
    this.ctx = this.cnv.getContext("2d") as CanvasRenderingContext2D;
    this.ctx!.imageSmoothingEnabled=false;
   
  
    this.toggle_visibility("rpf");
    
  

    this.reader!.readAsDataURL(file);

    this.reader!.onload = ($event:any)=>{

      this.preview! = new Image();

      this.preview!.src=URL.createObjectURL(file);

      this.preview!.onload = () =>{
        
       

        var selectH = document.getElementById("Horizontal") as HTMLSelectElement;
        var H_Value = Number(selectH.options[selectH.selectedIndex].text);


        var selectV = document.getElementById("Vertical") as HTMLSelectElement;
        var V_Value = Number(selectV.options[selectV.selectedIndex].text);

        this.calculate_chunks();

      }

    };
 
  }

  calculate_chunks(){
    var selectH = document.getElementById("Horizontal") as HTMLSelectElement;
        var H_Value = Number(selectH.options[selectH.selectedIndex].text);


        var selectV = document.getElementById("Vertical") as HTMLSelectElement;
        var V_Value = Number(selectV.options[selectV.selectedIndex].text);



        if(this.ctx && this.cnv && this.preview){

          

        this.draw_chunk_overlay(H_Value,V_Value);

      }


  }

  draw_chunk_overlay(H:number, V:number){

  this.ctx?.clearRect(0,0,this.cnv!.width,this.cnv!.height);
  this.ctx?.beginPath();


    if(H==16||V==16){
      this.ctx!.font="25px Arial";
    }
    else if(H==8||V==8){
      this.ctx!.font="30px Arial";
    }
    else if(H==4||V==4){
      this.ctx!.font="35px Arial";
    }
    else if(H==2||V==2){
      this.ctx!.font="40px Arial";
    }
    else{
      this.ctx!.font="45px Arial";
    }

    this.ctx?.drawImage(this.preview!,0,0,this.cnv!.width,this.cnv!.height);


    for(var i= 1; i < H; i++){

      var H_divisor = i*this.cnv!.width/H;

      this.ctx?.moveTo(H_divisor,0);
      this.ctx?.lineTo(H_divisor,this.cnv!.height);
      this.ctx?.stroke();

    }

    for(var i= 1; i < V; i++){

      var V_divisor = i*this.cnv!.height/V;

      this.ctx?.moveTo(0,V_divisor);
      this.ctx?.lineTo(this.cnv!.width,V_divisor);
      this.ctx?.stroke();

    }

    for(var i= 1; i <= H; i++){

      for(var j= 1; j <= V; j++){

        var H_divisor = (i-1)*this.cnv!.width/H;
        var V_divisor = j*this.cnv!.height/V;

        var CHK_Number = (j-1)*H+(i-1);

        
        this.ctx?.fillText(String(CHK_Number),H_divisor,V_divisor);
        this.ctx?.stroke();

      }    

    }   

  }

  toggle_visibility(visible:string){


    var to_make_visible = document.getElementById(visible) as HTMLDivElement;
    to_make_visible!.style.visibility="visible";

   
  }


}
