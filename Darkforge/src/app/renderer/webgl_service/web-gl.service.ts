import { HttpRequest } from '@angular/common/http';
import { HostListener, Injectable, numberAttribute } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { glMatrix, mat4 } from 'gl-matrix';
import { withNoHttpTransferCache } from '@angular/platform-browser';


@Injectable({
  providedIn: 'root'
})
export class WebGLService {
  private gl: WebGL2RenderingContext | null = null;
  private vertCode! :string;
  private fragCode! :string;
  private vertexShad :WebGLShader | null = null;
  private fragmentShad :WebGLShader | null = null;
  private GLProgram : WebGLProgram  | null  = null;
  private WMatLoc :any;
  private VMatLoc :any;
  private PMatLoc :any;
  private wMat  : any;
  private vMat  : any;
  private pMat  : any;
  private verts : any;
  private inds  : any;

  constructor(private http :HttpClient) { 

  }

  @HostListener('window:resize',['$event'])
  onresize(event:Event):void{

    

    if(this.gl){
    this.gl.canvas.width=window.innerWidth;
    this.gl.canvas.height=window.innerHeight;

    this.gl.viewport(0,0,this.gl.canvas.width,this.gl.canvas.height);
    
    

    }   
  }

  setup(){

    if(this.gl){
    this.GLProgram = this.gl.createProgram();   
    
   

    if(this.GLProgram){

      if(this.vertexShad){
        this.gl.attachShader(this.GLProgram,this.vertexShad);
      }
    
      if(this.fragmentShad){
        this.gl.attachShader(this.GLProgram,this.fragmentShad);
      }                

      this.gl.linkProgram(this.GLProgram);
    }
   
    this.verts = 
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
   
  this.inds =
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
    this.gl.bufferData(this.gl.ARRAY_BUFFER,new Float32Array(this.verts),this.gl.STATIC_DRAW);


    var iBuffer = this.gl.createBuffer();
    this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER,iBuffer);
    this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER,new Uint16Array(this.inds),this.gl.STATIC_DRAW);

      
      
      var pos = this.gl.getAttribLocation(this.GLProgram!,'vPosition');
      var clr = this.gl.getAttribLocation(this.GLProgram!,'vColor');



      this.gl.vertexAttribPointer(pos,3,this.gl.FLOAT,false,6*Float32Array.BYTES_PER_ELEMENT,0*Float32Array.BYTES_PER_ELEMENT);
      this.gl.vertexAttribPointer(clr,3,this.gl.FLOAT,false,6*Float32Array.BYTES_PER_ELEMENT,3*Float32Array.BYTES_PER_ELEMENT);


      this.gl.enableVertexAttribArray(pos);
      this.gl.enableVertexAttribArray(clr);

      

      this.gl.useProgram(this.GLProgram);

      

       this.WMatLoc =this.gl.getUniformLocation(this.GLProgram!,'worldMat');
       this.VMatLoc =this.gl.getUniformLocation(this.GLProgram!,'viewMat');
       this.PMatLoc =this.gl.getUniformLocation(this.GLProgram!,'projMat');

       this.wMat = new Float32Array(16);
       this.vMat = new Float32Array(16);
       this.pMat = new Float32Array(16);

      
    
     

      this.render();
      

    }


  }

  render(){

    if(this.gl){    
    

      var vBuffer = this.gl.createBuffer();
      this.gl.bindBuffer(this.gl.ARRAY_BUFFER, vBuffer);
      this.gl.bufferData(this.gl.ARRAY_BUFFER,new Float32Array(this.verts),this.gl.STATIC_DRAW);


      var iBuffer = this.gl.createBuffer();
      this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER,iBuffer);
      this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER,new Uint16Array(this.inds),this.gl.STATIC_DRAW);

        
        
      var pos = this.gl.getAttribLocation(this.GLProgram!,'vPosition');
      var clr = this.gl.getAttribLocation(this.GLProgram!,'vColor');



      this.gl.vertexAttribPointer(pos,3,this.gl.FLOAT,false,6*Float32Array.BYTES_PER_ELEMENT,0*Float32Array.BYTES_PER_ELEMENT);
      this.gl.vertexAttribPointer(clr,3,this.gl.FLOAT,false,6*Float32Array.BYTES_PER_ELEMENT,3*Float32Array.BYTES_PER_ELEMENT);


      this.gl.enableVertexAttribArray(pos);
      this.gl.enableVertexAttribArray(clr);

        

      this.gl.useProgram(this.GLProgram);

    
        
      
      mat4.identity(this.wMat);
      mat4.lookAt(this.vMat,[0.0,0.0,-7.5],[0.0,0.0,0.0],[0.0,1.0,0.0]);
      mat4.perspective(this.pMat,glMatrix.toRadian(45),this.gl.canvas.width/this.gl.canvas.height,0.1,1000.0);

      this.gl.uniformMatrix4fv(this.WMatLoc,false,this.wMat);
      this.gl.uniformMatrix4fv(this.VMatLoc,false,this.vMat);
      this.gl.uniformMatrix4fv(this.PMatLoc,false,this.pMat);
          
      var ang = 0;
      var idM = new Float32Array(16);
      mat4.identity(idM);   
       

      ang=performance.now()/1000/6*2*Math.PI;
      mat4.rotate(this.wMat,idM,ang,[0.7,1.0,0.3]);
      this.gl!.uniformMatrix4fv(this.WMatLoc,false,this.wMat);

      this.gl!.clearColor(0.175,0.175,0.175,1.0);
      this.gl!.clear(this.gl!.COLOR_BUFFER_BIT|this.gl!.DEPTH_BUFFER_BIT);

          
      this.gl!.drawElements(this.gl!.TRIANGLES,this.inds.length,this.gl!.UNSIGNED_SHORT,0);


        
        
      requestAnimationFrame(this.render.bind(this));

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
      this.gl.canvas.width=window.innerWidth;
      this.gl.canvas.height=window.innerHeight;
      this.gl.viewport(0,0,this.gl.canvas.width,this.gl.canvas.height);
      
      this.gl.enable(this.gl.DEPTH_TEST);         

      window.addEventListener('resize', (event) => this.onresize(event));

      

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
         
          this.setup();
        },
        error: (error) => {
          console.error('Error loading shader:', error);
        }
      });
   
     
    }
    else{}
  }

  

}

