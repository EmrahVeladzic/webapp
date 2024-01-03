import { HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class WebGLService {
  private gl: WebGL2RenderingContext | null = null;
  private vertCode! :string;
  private fragCode! :string;
  private vertexShad :WebGLShader | null = null;
  private fragmentShad :WebGLShader | null = null;

  constructor(private http :HttpClient) { 

  }

  render(){

    if(this.gl){
    var GLProgram = this.gl.createProgram();
     

    if(GLProgram){

      if(this.vertexShad){
        this.gl.attachShader(GLProgram,this.vertexShad);
      }
    
      if(this.fragmentShad){
        this.gl.attachShader(GLProgram,this.fragmentShad);
      }                

      this.gl.linkProgram(GLProgram);
    }
   

    var triVerts = [
       0.0, 0.5, 1.0,0.0,1.0,
      -0.5,-0.5, 0.0,1.0,1.0,
       0.5,-0.5, 1.0,1.0,0.0
    ]      
    var triBuffer = this.gl.createBuffer();
    this.gl.bindBuffer(this.gl.ARRAY_BUFFER, triBuffer);
    this.gl.bufferData(this.gl.ARRAY_BUFFER,new Float32Array(triVerts),this.gl.STATIC_DRAW);

      this.gl.useProgram(GLProgram);
      
      var pos = this.gl.getAttribLocation(GLProgram!,'vPosition');
      var clr = this.gl.getAttribLocation(GLProgram!,'vColor');

      this.gl.vertexAttribPointer(pos,2,this.gl.FLOAT,false,5*Float32Array.BYTES_PER_ELEMENT,0*Float32Array.BYTES_PER_ELEMENT);
      this.gl.vertexAttribPointer(clr,3,this.gl.FLOAT,false,5*Float32Array.BYTES_PER_ELEMENT,2*Float32Array.BYTES_PER_ELEMENT);


      this.gl.enableVertexAttribArray(pos);
      this.gl.enableVertexAttribArray(clr);
    
      this.gl.drawArrays(this.gl.TRIANGLES,0,3);
      
      
        
      

    }
  }

  compile_vertex(){
    
    if(this.gl){
   
    
      
      if(this.vertexShad){
        
        this.gl.shaderSource(this.vertexShad,this.vertCode);
        
       
         
       

        this.gl.compileShader(this.vertexShad);

        if(!this.gl.getShaderParameter(this.vertexShad,this.gl.COMPILE_STATUS)){
          console.log(this.gl.getShaderInfoLog(this.vertexShad));
        }
       

      }
      
      
    }
  }  

  compile_fragment(){

    if(this.gl){

    
      
      if(this.fragmentShad){
      
        this.gl.shaderSource(this.fragmentShad,this.fragCode);

      
        this.gl.compileShader(this.fragmentShad);

        if(!this.gl!.getShaderParameter(this.fragmentShad,this.gl.COMPILE_STATUS)){
          console.log(this.gl.getShaderInfoLog(this.fragmentShad));
        }
      
      }

    }
  }

  initialise(canvas : HTMLCanvasElement){
    this.gl=canvas.getContext("webgl2");
    if(this.gl){
      this.gl.viewport(0,0,this.gl.canvas.width,this.gl.canvas.height);
      this.gl.clearColor(0.175,0.175,0.175,1.0);
      this.gl.clear(this.gl.COLOR_BUFFER_BIT);

      this.vertexShad = this.gl.createShader(this.gl.VERTEX_SHADER);
      this.fragmentShad = this.gl.createShader(this.gl.FRAGMENT_SHADER);
      
      this.http.get(`assets/shaders/vertex.glsl`, { responseType: 'text' })
      .subscribe({
        next: (content: string) => {
          this.vertCode = content;
          this.compile_vertex();
        },
        error: (error) => {
          console.error('Error loading shader:', error);
        }
      });

      this.http.get(`assets/shaders/fragment.glsl`, { responseType: 'text' })
      .subscribe({
        next: (content: string) => {
          this.fragCode = content;
          this.compile_fragment();
         
          this.render();
        },
        error: (error) => {
          console.error('Error loading shader:', error);
        }
      });
   
     
    }
    else{}
  }
}
