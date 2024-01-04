import { HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { glMatrix, mat4 } from 'gl-matrix';


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
   
    var verts = 
	[ // X, Y, Z           R, G, B
		// Top
		-1.0, 1.0, -1.0,   0.5, 0.5, 0.5,
		-1.0, 1.0, 1.0,    0.5, 0.5, 0.5,
		1.0, 1.0, 1.0,     0.5, 0.5, 0.5,
		1.0, 1.0, -1.0,    0.5, 0.5, 0.5,

		// Left
		-1.0, 1.0, 1.0,    0.75, 0.25, 0.5,
		-1.0, -1.0, 1.0,   0.75, 0.25, 0.5,
		-1.0, -1.0, -1.0,  0.75, 0.25, 0.5,
		-1.0, 1.0, -1.0,   0.75, 0.25, 0.5,

		// Right
		1.0, 1.0, 1.0,    0.25, 0.25, 0.75,
		1.0, -1.0, 1.0,   0.25, 0.25, 0.75,
		1.0, -1.0, -1.0,  0.25, 0.25, 0.75,
		1.0, 1.0, -1.0,   0.25, 0.25, 0.75,

		// Front
		1.0, 1.0, 1.0,    1.0, 0.0, 0.15,
		1.0, -1.0, 1.0,    1.0, 0.0, 0.15,
		-1.0, -1.0, 1.0,    1.0, 0.0, 0.15,
		-1.0, 1.0, 1.0,    1.0, 0.0, 0.15,

		// Back
		1.0, 1.0, -1.0,    0.0, 1.0, 0.15,
		1.0, -1.0, -1.0,    0.0, 1.0, 0.15,
		-1.0, -1.0, -1.0,    0.0, 1.0, 0.15,
		-1.0, 1.0, -1.0,    0.0, 1.0, 0.15,

		// Bottom
		-1.0, -1.0, -1.0,   0.5, 0.5, 1.0,
		-1.0, -1.0, 1.0,    0.5, 0.5, 1.0,
		1.0, -1.0, 1.0,     0.5, 0.5, 1.0,
		1.0, -1.0, -1.0,    0.5, 0.5, 1.0,
	];
   
  var inds =
	[
		// Top
		0, 1, 2,
		0, 2, 3,

		// Left
		5, 4, 6,
		6, 4, 7,

		// Right
		8, 9, 10,
		8, 10, 11,

		// Front
		13, 12, 14,
		15, 14, 12,

		// Back
		16, 17, 18,
		16, 18, 19,

		// Bottom
		21, 20, 22,
		22, 20, 23
	];

    var vBuffer = this.gl.createBuffer();
    this.gl.bindBuffer(this.gl.ARRAY_BUFFER, vBuffer);
    this.gl.bufferData(this.gl.ARRAY_BUFFER,new Float32Array(verts),this.gl.STATIC_DRAW);


    var iBuffer = this.gl.createBuffer();
    this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER,iBuffer);
    this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER,new Uint16Array(inds),this.gl.STATIC_DRAW);

      
      
      var pos = this.gl.getAttribLocation(GLProgram!,'vPosition');
      var clr = this.gl.getAttribLocation(GLProgram!,'vColor');



      this.gl.vertexAttribPointer(pos,3,this.gl.FLOAT,false,6*Float32Array.BYTES_PER_ELEMENT,0*Float32Array.BYTES_PER_ELEMENT);
      this.gl.vertexAttribPointer(clr,3,this.gl.FLOAT,false,6*Float32Array.BYTES_PER_ELEMENT,3*Float32Array.BYTES_PER_ELEMENT);


      this.gl.enableVertexAttribArray(pos);
      this.gl.enableVertexAttribArray(clr);

      

      this.gl.useProgram(GLProgram);

      

      var WMatLoc =this.gl.getUniformLocation(GLProgram!,'worldMat');
      var VMatLoc =this.gl.getUniformLocation(GLProgram!,'viewMat');
      var PMatLoc =this.gl.getUniformLocation(GLProgram!,'projMat');

      var wMat = new Float32Array(16);
      var vMat = new Float32Array(16);
      var pMat = new Float32Array(16);

      
    
      mat4.identity(wMat);
      mat4.lookAt(vMat,[0.0,0.0,-7.5],[0.0,0.0,0.0],[0.0,1.0,0.0]);
      mat4.perspective(pMat,glMatrix.toRadian(45),this.gl.canvas.width/this.gl.canvas.height,0.1,1000.0);

      this.gl.uniformMatrix4fv(WMatLoc,false,wMat);
      this.gl.uniformMatrix4fv(VMatLoc,false,vMat);
      this.gl.uniformMatrix4fv(PMatLoc,false,pMat);
        
      var ang = 0;
      var idM = new Float32Array(16);
      mat4.identity(idM);

      var self = this;
      var repeat = function(){

        ang=performance.now()/1000/6*2*Math.PI;
        mat4.rotate(wMat,idM,ang,[0.7,1.0,0.3]);
        self.gl!.uniformMatrix4fv(WMatLoc,false,wMat);

        self.gl!.clearColor(0.175,0.175,0.175,1.0);
        self.gl!.clear(self.gl!.COLOR_BUFFER_BIT|self.gl!.DEPTH_BUFFER_BIT);

        self.gl!.drawElements(self.gl!.TRIANGLES,inds.length,self.gl!.UNSIGNED_SHORT,0);


        requestAnimationFrame(repeat);
      }
      requestAnimationFrame(repeat);

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
    this.gl=canvas.getContext("webgl2",{antialias:true});
    if(this.gl){
      this.gl.viewport(0,0,this.gl.canvas.width,this.gl.canvas.height);
      
      this.gl.enable(this.gl.DEPTH_TEST);
      

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
