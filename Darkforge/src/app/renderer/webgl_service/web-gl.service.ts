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
      this.gl.clearColor(1.0,1.0,1.0,1.0);
    }
    else{
      
    }
  }
}
