import { HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class WebGLService {
  private gl: WebGL2RenderingContext | null = null;

  

  constructor() { 
   
  }

  

  initialise(canvas : HTMLCanvasElement){
    this.gl=canvas.getContext("webgl2");
    if(this.gl){
      this.gl.viewport(0,0,this.gl.canvas.width,this.gl.canvas.height);
      this.gl.clearColor(0.175,0.175,0.175,1.0);
      this.gl.clear(this.gl.COLOR_BUFFER_BIT);
         
      
      var vertReq = new XMLHttpRequest();
      vertReq.open('GET','shaders/vertex.glsl',true);
      

      vertReq.onreadystatechange = function () {
        if(vertReq.readyState === 4 && vertReq.status!=404){
        var vSource = vertReq.responseText;
        console.log(vSource);
        }
      }
      vertReq.send();

      var fragReq = new XMLHttpRequest();
      fragReq.open('GET','shaders/fragment.glsl',true);
     
      
      fragReq.onreadystatechange = function () {
        if(fragReq.readyState === 4 && fragReq.status!=404){
          var fSource = fragReq.responseText;
          console.log(fSource);
        }

      }
      fragReq.send();
     
      var vertexShad = this.gl.createShader(this.gl.VERTEX_SHADER);
      var fargmentShad = this.gl.createShader(this.gl.FRAGMENT_SHADER);


    }
    else{
      
    }
  }
}
