import { HttpRequest } from '@angular/common/http';
import { HostListener, Injectable, numberAttribute } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { glMatrix, mat4, vec3, quat} from 'gl-matrix';
import { withNoHttpTransferCache } from '@angular/platform-browser';
import { flip_tex_state, tex,tex_update ,ast, ast_update, flip_ast_state} from '../../../assets/global_assets';

@Injectable({
  providedIn: 'root'
})
export class WebGLService {
  public V:number=0;
  public H:number=Math.PI;
  public D:number=-7.5;
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

  private out_tex :any;


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
    

      if(tex_update==true){
        this.gl.texImage2D(this.gl.TEXTURE_2D,0,this.gl.RGBA,tex.Width,tex.Height,0,this.gl.RGBA,this.gl.UNSIGNED_SHORT_5_5_5_1,tex.Data);
        flip_tex_state();

      }

  


      this.gl.useProgram(this.GLProgram);

    
        
      
      mat4.identity(this.wMat);
      mat4.lookAt(this.vMat,[0.0,0.0,this.D],[0.0,0.0,0.0],[0.0,1.0,0.0]);
      mat4.perspective(this.pMat,glMatrix.toRadian(45),this.gl.canvas.width/this.gl.canvas.height,0.1,1000.0);

      this.gl.uniformMatrix4fv(this.WMatLoc,false,this.wMat);
      this.gl.uniformMatrix4fv(this.VMatLoc,false,this.vMat);
      this.gl.uniformMatrix4fv(this.PMatLoc,false,this.pMat);
        
      let  vq = quat.create();
      let hq = quat.create();
      quat.setAxisAngle(vq, [1, 0, 0], this.V);
      quat.setAxisAngle(hq, [0, 1, 0], this.H);
      
      let rq = quat.create();

      quat.multiply(rq,vq,hq);
      quat.normalize(rq,rq);

      mat4.fromQuat(this.wMat,rq);

      this.gl!.uniformMatrix4fv(this.WMatLoc,false,this.wMat);

      this.gl!.clearColor(0.2,0.2,0.2,1.0);
      this.gl!.clear(this.gl!.COLOR_BUFFER_BIT|this.gl!.DEPTH_BUFFER_BIT);


      if(ast_update==false){

        for(let i = 0; i < ast.mdl?.meshes.length!;i++){

       
          
          
          const vBuffer = this.gl.createBuffer();
          this.gl.bindBuffer(this.gl.ARRAY_BUFFER, vBuffer);
          this.gl.bufferData(this.gl.ARRAY_BUFFER,new Float32Array(ast.mdl?.meshes[i].vt?.vertices??[]),this.gl.STATIC_DRAW);
      
    
    
          const iBuffer = this.gl.createBuffer();
          this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER,iBuffer);
          this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER,new Uint16Array(ast.mdl?.meshes[i].ind?.indices??[]),this.gl.STATIC_DRAW);
        
            
            
          const uBuffer = this.gl.createBuffer();
          this.gl.bindBuffer(this.gl.ARRAY_BUFFER, uBuffer);
          this.gl.bufferData(this.gl.ARRAY_BUFFER,new Float32Array(ast.mdl?.meshes[i].uv?.textureCoordinates??[]),this.gl.STATIC_DRAW);
        
    
    
          const pos = this.gl.getAttribLocation(this.GLProgram!,'vPosition');
          this.gl.bindBuffer(this.gl.ARRAY_BUFFER,vBuffer);
          this.gl.vertexAttribPointer(pos,3,this.gl.FLOAT,false,3*Float32Array.BYTES_PER_ELEMENT,0*Float32Array.BYTES_PER_ELEMENT);
          this.gl.enableVertexAttribArray(pos);
    
    
          const uv = this.gl.getAttribLocation(this.GLProgram!,'vUV');
          this.gl.bindBuffer(this.gl.ARRAY_BUFFER,uBuffer);  
          this.gl.vertexAttribPointer(uv,2,this.gl.FLOAT,false,2*Float32Array.BYTES_PER_ELEMENT,0*Float32Array.BYTES_PER_ELEMENT);
          this.gl.enableVertexAttribArray(uv);
    
          
          this.gl!.drawElements(this.gl!.TRIANGLES,ast.mdl?.meshes[i].ind?.indices!.length??0,this.gl!.UNSIGNED_SHORT,0);
        
        }
      }

       
        
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
      this.gl.pixelStorei(this.gl.UNPACK_ALIGNMENT, 1);


      this.out_tex = this.gl.createTexture();

      this.gl.bindTexture(this.gl.TEXTURE_2D,this.out_tex);
      this.gl.texParameteri(this.gl.TEXTURE_2D,this.gl.TEXTURE_WRAP_S, this.gl.REPEAT);
      this.gl.texParameteri(this.gl.TEXTURE_2D,this.gl.TEXTURE_WRAP_T, this.gl.REPEAT);
      this.gl.texParameteri(this.gl.TEXTURE_2D,this.gl.TEXTURE_MIN_FILTER, this.gl.NEAREST);
      this.gl.texParameteri(this.gl.TEXTURE_2D,this.gl.TEXTURE_MAG_FILTER, this.gl.NEAREST);

   
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

